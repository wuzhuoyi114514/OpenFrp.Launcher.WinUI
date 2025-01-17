﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenFrp.Core.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace OpenFrp.Launcher.ViewModels
{
    public partial class LogsModel : ObservableObject
    {
        public ConfigHelper.FontSetting FontSetting
        {
            get => Core.Helper.ConfigHelper.Instance.FontSet;
            set => Core.Helper.ConfigHelper.Instance.FontSet = value;
        }



        public bool DebugMode
        {
            get => ConfigHelper.Instance.DebugMode;
        }

        public List<Core.Libraries.Api.Models.ResponseBody.UserTunnelsResponse.UserTunnel?>? UserTunnels { get; set; }

        [ObservableProperty]
        public ObservableCollection<LogHelper.LogContent?>? logContent;


        [ObservableProperty]
        public int selectLogIndex;

        public ICollectionView LogsHeaders { get => CollectionViewSource.GetDefaultView(UserTunnels); }
        //public ICollectionView LogsViewer { get => CollectionViewSource.GetDefaultView(LogContent); }

        private int Count { get; set; }


        public async void GetLogs(bool reset = false)
        {

            var request = new Core.Libraries.Protobuf.RequestBase()
            {
                Action = Core.Libraries.Protobuf.RequestType.ClientGetRunningtunnel,
                LogsRequest = new() { Id = 0 }
            };
            if (UserTunnels?.Count() is not 0 && UserTunnels?.Count() >= SelectLogIndex)
            {
                request.LogsRequest.Id = UserTunnels?[SelectLogIndex]?.TunnelId ?? 0;
            }

            var response = await Helper.AppShareHelper.PipeClient.Request(request);

            if (response.Success)
            {
                var u2serTunnels = response.LogsViewJson.Select(x => x.PraseJson<Core.Libraries.Api.Models.ResponseBody.UserTunnelsResponse.UserTunnel>()).ToList();
                if (reset) LogContent = default;
                UserTunnels ??= new();
                LogContent ??= new();


                

                if (response.LogsJson.Count > 0)
                {
                    //bool refreshed = false;
                    OpenFrp.Launcher.ExtendsUI.VoidAsync(App.Current.Dispatcher.BeginInvoke(() =>
                    {
                        response.LogsJson.Select(x => x.PraseJson<LogHelper.LogContent>()).ToList().ForEach(x =>
                        {

                            if (!LogContent.Select(x => x?.HashContent).Contains(x?.HashContent))
                            {
                                //if (LogContent.Count > 150)
                                //{
                                //    LogContent.RemoveRange(0, LogContent.Count - 150);
                                //}

                                LogContent.Add(x);

                                //if (refreshed)
                                //{
                                //    LogsViewer.Refresh();
                                //    refreshed = true;
                                    
                                //}
                                OnPropertyChanged(nameof(LogContent));


                            }

                        });
                    }, System.Windows.Threading.DispatcherPriority.Background));
                }
                else
                {
                    if (LogContent.Count is not 0)
                    {
                        // 数量不为0 需要刷新 View
                        LogContent?.Clear();
                        //LogsViewer?.Refresh();
                        //OnPropertyChanged(nameof(LogsViewer));
                        OnPropertyChanged(nameof(LogContent));
                    }


                }

                //if (response.LogsJson.Count > 0)
                //{
                //    if (LogContent.Count > 150)
                //    {
                //        LogContent.RemoveRange(0, LogContent.Count - 150);
                //    }
                //    if (response.LogsJson.Count < LogContent.Count)
                //    {
                //        LogContent?.Clear();
                //    }

                //    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                //    {
                //        response.LogsJson.Select(x => x.PraseJson<LogHelper.LogContent>()).ToList().ForEach(x =>
                //        {

                //            if (!LogContent.Select(x => x?.HashContent).Contains(x?.HashContent))
                //            {
                //                if (LogContent.Count > 150)
                //                {
                //                    LogContent.RemoveRange(0, LogContent.Count - 150);
                //                }
                //                LogContent.Add(x);

                //                LogsViewer.DeferRefresh();
                //                OnPropertyChanged(nameof(LogsViewer));
                //            }

                //        });
                //    }, System.Windows.Threading.DispatcherPriority.Background);
                //}
                //else
                //{
                //    LogContent?.Clear();
                //    LogsViewer.DeferRefresh();
                //    OnPropertyChanged(nameof(LogsViewer));
                //}


                if (response.LogsViewJson.Count > 0)
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        if (u2serTunnels.Count > Count)
                        {
                            u2serTunnels.ForEach(x =>
                            {
                                if (!UserTunnels.Select(a => a?.TunnelId).Contains(x?.TunnelId))
                                {
                                    UserTunnels.Add(x);
                                }

                            });
                            LogsHeaders.Refresh();
                        }
                        else if (u2serTunnels.Count < Count)
                        {
                            var u3sert = UserTunnels.ToList();
                            u3sert.ForEach(x =>
                            {
                                if (!u2serTunnels.Select(a => a?.TunnelId).Contains(x?.TunnelId))
                                {
                                    UserTunnels.Remove(x);
                                    if (SelectLogIndex > Count || !u2serTunnels.Select(a => a?.TunnelId).Contains(u3sert[SelectLogIndex]?.TunnelId))
                                    {
                                        SelectLogIndex = 0;
                                    }
                                }
                            });
                            LogsHeaders.Refresh();
                        }

                        Count = u2serTunnels.Count;





                    }, System.Windows.Threading.DispatcherPriority.Background);
                }

                OnPropertyChanged(nameof(LogsHeaders));

                //await App.Current.Dispatcher.InvokeAsync(() =>
                //{
                //    LogsViewer.Refresh();
                //    LogsHeaders.Refresh();

                //    OnPropertyChanged(nameof(LogsViewer));
                //    OnPropertyChanged(nameof(LogsHeaders));
                //});



            }


        }

        [RelayCommand]
        void Refresh() => GetLogs(true);

        [RelayCommand]
        async void ClearLogs()
        {
            var request = new Core.Libraries.Protobuf.RequestBase()
            {
                Action = Core.Libraries.Protobuf.RequestType.ClientClearLogs,
                LogsRequest = new()
                {
                    Id = UserTunnels?[SelectLogIndex]?.TunnelId ?? 0
                }
            };
            var response = await Helper.AppShareHelper.PipeClient.Request(request);

            if (response.Success)
            {
                LogContent?.Clear();
                OnPropertyChanged(nameof(LogContent));
                //LogsViewer.Refresh();
                GetLogs(true);
            }
        }

        [RelayCommand]
        async void SaveLogs()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "日志文件(*.log)|*.log",
                CheckPathExists = true,
            };
            if (dialog.ShowDialog() is true)
            {
                try
                {
                    var writer = dialog.FileName.GetStreamWriter(autoFlush: true);
                    foreach (LogHelper.LogContent? item in LogContent!)
                    {
                        await writer.WriteLineAsync(item?.Content);
                    }
                    writer.Close();
                    var dialog2 = new ContentDialog()
                    {
                        Title = "保存日志",
                        Content = new TextBlock()
                        {
                            Width = 200,
                            Height = 50,
                            Text = "保存成功!",
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                        },
                        CloseButtonText = "确定",
                        PrimaryButtonText = "打开"
                    };
                    if (await dialog2.ShowDialogFixed() is ContentDialogResult.Primary)
                    {
                        Process.Start(dialog.FileName);
                    }
                }
                catch
                {

                }
            }
        }
    }
}
