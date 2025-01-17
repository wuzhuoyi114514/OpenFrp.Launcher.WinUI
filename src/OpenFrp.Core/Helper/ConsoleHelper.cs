﻿using CommunityToolkit.WinUI.Notifications;
using Google.Protobuf;
using OpenFrp.Core.Libraries.Api;
using OpenFrp.Core.Libraries.Pipe;
using OpenFrp.Core.Libraries.Protobuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFrp.Core.Helper
{
    public class ConsoleHelper
    {

        public class ConsoleWrapper
        {
            public Libraries.Api.Models.ResponseBody.UserTunnelsResponse.UserTunnel? Tunnel { get; set; }

            public Process? Process { get; set; }

            public int RestartCount { get; set; } = 0;

            public int ReconnectCount { get; set; } = 0;
        }

        public static Dictionary<int, ConsoleWrapper> Wrappers = new();

        public static (bool,Exception?) Launch(Libraries.Api.Models.ResponseBody.UserTunnelsResponse.UserTunnel tunnel)
        {
            if (ApiRequest.HasAccount)
            {
                if (File.Exists(Utils.Frpc))
                {
                    try
                    {
                        Process process = new()
                        {
                            EnableRaisingEvents = true,
                            StartInfo = new()
                            {
                                UseShellExecute = false,
                                CreateNoWindow = false,
                                RedirectStandardError = true,
                                RedirectStandardOutput = true,
                                StandardOutputEncoding = Encoding.UTF8,
                                StandardErrorEncoding = Encoding.UTF8,
                                FileName = Utils.Frpc,
                                Arguments = $"-n -u {ApiRequest.UserInfo!.UserToken} -p {tunnel.TunnelId} {(ConfigHelper.Instance.ForceTLS ? "--force_tls " : "")}{(ConfigHelper.Instance.DebugMode ? "--debug" : "")}",
                            }
                        };
                        LogHelper.Add(0, $"传入参数: -u ************ -p {tunnel.TunnelId} {(ConfigHelper.Instance.ForceTLS ? "--force_tls " : "")}{(ConfigHelper.Instance.DebugMode ? "--debug" : "")}", TraceLevel.Info, true);
                        process.OutputDataReceived += (_, args) => Output(tunnel.TunnelId, args.Data);
                        process.ErrorDataReceived += (_, args) => Output(tunnel.TunnelId, args.Data, TraceLevel.Error);
                        process.Start();
                        process.BeginErrorReadLine();
                        process.BeginOutputReadLine();
                        process.Exited += async (sender, args) =>
                        {
                            if (Wrappers.ContainsKey(tunnel.TunnelId))
                            {
                                if (Wrappers[tunnel.TunnelId].RestartCount > 5)
                                {
                                    Output(tunnel.TunnelId, $"[进程已重启超过五次,Exit Code: {process.ExitCode}]", TraceLevel.Warning);
                                    if (Program.PushClient is not null && Program.PushClient.Pipe?.IsConnected is true)
                                    {
                                        LogHelper.Add(0, $"服务端请求刷新客户端隧道列表。", TraceLevel.Warning, true);
                                        await Program.PushClient.SendAsync(new RequestBase()
                                        {
                                            Action = RequestType.ServerUpdateTunnels
                                        }.ToByteArray());
                                    }
                                }
                                else
                                {
                                    Output(tunnel.TunnelId, $"[进程已退出,Exit Code: {process.ExitCode},等待 1500ms 后重启。]", TraceLevel.Warning);
                                    await Task.Delay(1500);
                                    Launch(tunnel);
                                    Wrappers[tunnel.TunnelId].RestartCount++;
                                }
                            }

                        };
                        if (!Wrappers.ContainsKey(tunnel.TunnelId))
                        {
                            Wrappers.Add(tunnel.TunnelId, new()
                            {
                                Tunnel = tunnel,
                                Process = process
                            });
                        }
                        else
                        {
                            Wrappers[tunnel.TunnelId].Process = process;
                        }
                        return (true, default);

                    }
                    catch (UnauthorizedAccessException ex)
                    {

                        try
                        {
                            if (!Utils.IsWindowsService)
                                Process.Start("https://docs.openfrp.net/use/desktop-launcher.html#%E5%8A%A0%E5%85%A5%E7%B3%BB%E7%BB%9F%E7%99%BD%E5%90%8D%E5%8D%95");
                        }
                        catch
                        {

                        }
                        LogHelper.Add(0, ex.ToString(), System.Diagnostics.TraceLevel.Warning, true);
                        LogHelper.Add(0, ex, TraceLevel.Warning, true);
                        return (false, ex);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Add(0, ex.ToString(), System.Diagnostics.TraceLevel.Warning, true);
                        LogHelper.Add(0, ex, TraceLevel.Warning, true);
                        return (false, ex);
                    }
                }
                return (false, new Exception("请重启启动器。"));
            }
            return (false,new Exception("账户未登录"));

        }

        public static bool Kill(Libraries.Api.Models.ResponseBody.UserTunnelsResponse.UserTunnel tunnel)
        {
            try
            {
                if (Wrappers.ContainsKey(tunnel.TunnelId))
                {
                    if (Wrappers[tunnel.TunnelId].Process is Process process && !process.HasExited)
                    {
                        process.EnableRaisingEvents = false;
                        process.Kill();
                    }
                    Wrappers.Remove(tunnel.TunnelId);
                }
                return true;
            }
            catch(Exception ex)
            {
                LogHelper.Add(0, ex.ToString(), System.Diagnostics.TraceLevel.Warning, true);
                return false;
            }
        }


        static async void Output(int id,object data,TraceLevel level = TraceLevel.Info)
        {
            
            if (data?.ToString().Contains("[E]") is true) level = TraceLevel.Error;
            else if (data?.ToString().Contains("[W]") is true)
            {
                level = TraceLevel.Warning;
                if (data?.ToString().Contains("正在尝试重连到服务器") is true && Wrappers.ContainsKey(id))
                {
                    Wrappers[id].ReconnectCount++;

                    if (Wrappers[id].ReconnectCount > 10)
                    {
                        LogHelper.Add(0, $"隧道 {id} 重连次数过多，正在尝试重新打开。。", TraceLevel.Warning, true);
                    }
                }
            }

            if (data?.ToString().Contains("面板强制下线") ?? false)
            {
                if (Wrappers.ContainsKey(id))
                {
                    Kill(Wrappers[id].Tunnel!);
                    if (Program.PushClient is not null && Program.PushClient.Pipe?.IsConnected is true)
                    {
                        LogHelper.Add(0, $"隧道 {id} 已从面板强制下线，已向客户端发送请求包。",TraceLevel.Warning,true);
                        await Program.PushClient.SendAsync(new RequestBase()
                        {
                            Action = RequestType.ServerUpdateTunnels
                        }.ToByteArray());
                    }

                }
            }
            else if (data?.ToString().Contains("OpenFRP API 拒绝请求或响应异常 (状态码: 403, 信息: Proxy disabled)") ?? false)
            {
                if (Wrappers.ContainsKey(id))
                {
                    Kill(Wrappers[id].Tunnel!);
                    if (Program.PushClient is not null && Program.PushClient.Pipe?.IsConnected is true)
                    {
                        LogHelper.Add(0, $"隧道 {id} 被禁用，已向客户端发送请求包。", TraceLevel.Warning, true);
                        await Program.PushClient.SendAsync(new RequestBase()
                        {
                            Action = RequestType.ServerUpdateTunnels
                        }.ToByteArray());
                    }

                }
            }
            else if (data?.ToString().Contains("OpenFRP API 拒绝请求或响应异常 (状态码: 403, 信息: 节点不存在或不可用)") ?? false)
            {
                if (Wrappers.ContainsKey(id))
                {
                    Kill(Wrappers[id].Tunnel!);
                    if (Program.PushClient is not null && Program.PushClient.Pipe?.IsConnected is true)
                    {
                        LogHelper.Add(0, $"隧道 {id} 所在的节点不可用 / 被禁用，已向客户端发送请求包。", TraceLevel.Warning, true);
                        await Program.PushClient.SendAsync(new RequestBase()
                        {
                            Action = RequestType.ServerUpdateTunnels
                        }.ToByteArray());
                    }

                }
            }
            else if (data?.ToString().Contains("隧道ID可能有误") ?? false)
            {
                if (Wrappers.ContainsKey(id))
                {
                    Kill(Wrappers[id].Tunnel!);
                    if (Program.PushClient is not null && Program.PushClient.Pipe?.IsConnected is true)
                    {
                        LogHelper.Add(0, $"隧道 {id} 可能已被删除，已向客户端发送请求包。", TraceLevel.Warning, true);
                        await Program.PushClient.SendAsync(new RequestBase()
                        {
                            Action = RequestType.ServerUpdateTunnels
                        }.ToByteArray());
                    }

                }
            }
            else if (!Utils.IsWindowsService && (data?.ToString().Contains("启动成功") is true || data?.ToString().Contains("启动失败") is true))
            {
                Console.WriteLine(Enum.GetName(typeof(ConfigHelper.TnMode), ConfigHelper.Instance.MessagePullMode));

                if (Program.PushClient is not null)
                {
                    var req = new RequestBase()
                    {
                        Action = RequestType.ServerSendNotifiy,
                        NotifiyRequest = new()
                        {
                            TunnnelJson = Wrappers[id].Tunnel?.JSON(),
                            Flag = data?.ToString().Contains("启动成功") ?? false
                        }
                    };
                    if (!req.NotifiyRequest.Flag) { req.NotifiyRequest.Content = data?.ToString() ?? "请前往日志查看。"; }
                    await Program.PushClient.SendAsync(req.ToByteArray());
                }
                    
                // 逻辑需在客户端处理
            }
            LogHelper.Add(id, data ?? "", level);
            LogHelper.Add(0, data ?? "", level);
        }
    }
}
