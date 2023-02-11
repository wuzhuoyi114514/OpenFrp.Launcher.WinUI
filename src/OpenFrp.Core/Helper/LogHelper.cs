﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFrp.Core.Helper
{
    public class LogHelper
    {
        public static Dictionary<int, List<LogContent>> Logs { get; set; } = new();

        public class LogContent
        {
            public LogContent() { }

            public LogContent(string? content,TraceLevel level, bool isDebug)
            {
                Content = content;
                Level = level;
                IsDebug = isDebug;
            }

            public string? Content { get; set; } 

            public TraceLevel Level { get; set; }

            public bool IsDebug { get; set; }
        }

        public static void Add(int id,string contnet,TraceLevel level = TraceLevel.Info,bool isDebug = false)
        {
            if (id is not 0) Add(0,contnet,level,isDebug);

            if (Logs.ContainsKey(id))
            {
                Logs[id].Add(new(contnet, level,isDebug));
            }
            else
            {
                Logs.Add(id, new()
                {
                    new(contnet, level,isDebug)
                });
            }
        }
        public static void Add(int id, object content, TraceLevel level = TraceLevel.Info, bool isDebug = false) => Add(id, content?.ToString() ?? "", level, isDebug);

        public static void Remove(int id)
        {
            if (Logs.ContainsKey(id)) Logs.Remove(id);
        }
    }
}
