using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Log
{
    public class LogHelper<T>
    {
        private static object lockObject;
        private static Dictionary<Type, ILog> loggers;
        private static Type type;

        // Methods
        static LogHelper()
        {
            loggers = new Dictionary<Type, ILog>();
            lockObject = new object();
            type = typeof(T);
        }

        /// <summary>
        /// 获取日志
        /// </summary>
        /// <returns>日志操作接口</returns>
        public static ILog GetLogger()
        {
            lock (lockObject)
            {
                if (loggers.ContainsKey(type))
                {
                    return loggers[type];
                }
                ILog log = new Log(type);
                loggers.Add(type, log);
                return log;
            }
        }
    }
}
