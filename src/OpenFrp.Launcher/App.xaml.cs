﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Google.Protobuf;
using ModernWpf.Controls.Primitives;
using Newtonsoft.Json;
using OpenFrp.Core;
using OpenFrp.Core.Helper;
using OpenFrp.Core.Libraries.Api;
using OpenFrp.Core.Libraries.Pipe;
using OpenFrp.Launcher.Helper;
using OpenFrp.Launcher.Views;

namespace OpenFrp.Launcher
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 加载事件 - Redo
        /// </summary>
        protected override async void OnStartup(StartupEventArgs e)
        {
            if (UxThemeHelper.IsSupportDarkMode)
            {
                UxThemeHelper.AllowDarkModeForApp(true);
                UxThemeHelper.ShouldSystemUseDarkMode();
            }
            JsonConvert.DefaultSettings = () => new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            await ConfigHelper.ReadConfig();
            Microsoft.Win32.SystemEvents.SessionEnding += async (sender, e) =>
            {
                // 保存 Config
                e.Cancel = true;
                await ConfigHelper.Instance.WriteConfig();
                e.Cancel = false;
            };


            AutoLogin();
            CreateWindow();
            PipeIOStart();


        }

        /// <summary>
        /// 创建窗口
        /// </summary>
        private void CreateWindow()
        {
            var wind = new MainPage()
            {
                Width = 1366,
                Height = 768,
                Title = $"{new Random().NextDouble()} - Worker"
            };
            WindowHelper.SetSystemBackdropType(wind, ConfigHelper.Instance.BackdropSet);
            ThemeManager.SetRequestedTheme(wind, ConfigHelper.Instance.ThemeSet);
            wind.Show();
        }
        
        private async void AutoLogin()
        {
            if (ConfigHelper.Instance.Account.HasAccount)
            {
                await Task.Delay(1500);
                await AppShareHelper.LoginAndGetUserInfo(ConfigHelper.Instance.Account.UserName!, ConfigHelper.Instance.Account.Password!);
            }
        }

        private void PipeIOStart(bool restartup = false)
        {
            AppShareHelper.PipeClient.Start();
            // 服务端推送到客户端
            var pushClient = new PipeClient();
            pushClient.Start(true);
            pushClient.OnPushStart = async worker =>
            {
                AppShareHelper.HasDeamonProcess = true;
                if (ConfigHelper.Instance.Account.HasAccount && restartup)
                {
                    var resp =  await AppShareHelper.LoginAndGetUserInfo(ConfigHelper.Instance.Account.UserName, ConfigHelper.Instance.Account.Password);
                }
                while (worker.IsConnected && worker.IsPushMode)
                {
                    int count;
                    try
                    {
                        count = await worker.ReadAsync();
                        if (count > 0)
                        {
                            var request = Core.Libraries.Protobuf.RequestBase.Parser.ParseFrom(worker.Buffer, 0, worker.EnsureMessageComplete(count));
                            Core.Libraries.Protobuf.ResponseBase response = request.Action switch
                            {
                                _ => new()
                                {
                                    Message = "Action not found",
                                    Success = false
                                },
                            };
                            await worker.SendAsync(response.ToByteArray());
                        }
                        else
                        {
                            AppShareHelper.PipeClient.Disconnect();
                            AppShareHelper.HasDeamonProcess = false;
                            ApiRequest.ClearAuth();
                            ((ViewModels.MainPageModel)App.Current.MainWindow.DataContext).UpdateProperty("UserInfo");
                            if((((Views.MainPage)App.Current.MainWindow)).Of_nViewFrame.Content is Setting setting && setting.DataContext is ViewModels.SettingModel settingModel)
                            {
                                settingModel.HasAccount = false;
                            }
                            Utils.Log("Service IO Closed.", true);
                            // 在PipeServer被关闭时，会发送一个 长度为 0 的数据包
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Utils.Log(ex, true);
                        break;
                    }
                }
                worker.Client?.Close();
                worker.IsPushMode = false;
                // 先等待1500秒
                await Task.Delay(1500);
                PipeIOStart(true);
            };

        }


        /// <summary>
        /// Launcher 退出时
        /// </summary>
        protected override async void OnExit(ExitEventArgs e)
        {
            await ConfigHelper.Instance.WriteConfig();
        }
        /// <summary>
        /// 链接被点击时
        /// </summary>
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var h = (Hyperlink)sender;
            if (h.NavigateUri is not null &&
                h.Parent.GetType() != typeof(HyperlinkButton))
            {
                Process.Start(h!.NavigateUri.ToString());
            }
        }
    }

    public static class ExtendsUI
    {
        /// <summary>
        /// 修复了一个窗口可以弹出两个的问题
        /// </summary>
        public async static ValueTask ShowDialogFixed(this ContentDialog dialog)
        {
            if (!AppShareHelper.HasDialog)
            {
                AppShareHelper.HasDialog = true;
                await dialog.ShowAsync();
                AppShareHelper.HasDialog = false;
            }
        }
    }
}