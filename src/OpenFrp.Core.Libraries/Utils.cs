﻿
using OpenFrp.Core.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenFrp.Core
{
    public class Utils
    {

        /// <summary>
        /// 应用的运行目录
        /// </summary>
        public static string ApplicationExecutePath { get => AppDomain.CurrentDomain.BaseDirectory; }
        /// <summary>
        /// 应用的数据文件存放处
        /// </summary>
        public static string ApplicatioDataPath { get => Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData).CombinePath("OpenFrp.Launcher"); }
        /// <summary>
        /// 配置文件
        /// </summary>
        public static string ConfigFile { get => ApplicatioDataPath.CombinePath("config.json"); }
        /// <summary>
        /// 管道的名称
        /// </summary>
        public static string PipesName { get => $"{ApplicatioDataPath.GetMD5()}_Of2023_rel.app".GetMD5().ToUpper(); }
        /// <summary>
        /// 是否以系统服务模式运行
        /// </summary>
        public static bool IsWindowsService { get => !Environment.UserInteractive; }
        /// <summary>
        /// 应用主窗口
        /// </summary>
        public static Window? MainWindow { get => Application.Current?.MainWindow;  }
        /// <summary>
        /// FRPC 对应平台的名称
        /// </summary>
        public static string FrpcPlatform { get => $"frpc_windows_{Platform()}"; }
        /// <summary>
        /// FRPC
        /// </summary>
        public static string Frpc { get => $"{ApplicationExecutePath}\\frpc\\{FrpcPlatform}.exe"; }
        /// <summary>
        /// 开机自启的快捷方式
        /// </summary>
        public static string AutoLaunchLink { get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), $"{PipesName}.lnk"); }
        /// <summary>
        /// 启动器版本
        /// </summary>
        public static string LauncherVersion { get => "of.launcher.relVer+3.9.4"; }//_devNight@1

        private static string Platform()
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X64 => "amd64",
                Architecture.X86 => "386",
                Architecture.Arm64 => "arm64",
                _ => throw new NotSupportedException("本软件暂不支持 ARMv7 等其他平台。"),
            };
            
        }




    }
}
