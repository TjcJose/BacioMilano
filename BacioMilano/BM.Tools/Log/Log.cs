using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net.Core;

namespace BM.Log
{
    internal class Log : ILog
    {
       protected log4net.ILog log;
     
       public Log(string name)
       {
           this.log = log4net.LogManager.GetLogger(name);
       }

       public Log(Type type)
       {
           this.log = log4net.LogManager.GetLogger(type);
       }

       public Log(string repository, string name)
       {
           this.log = log4net.LogManager.GetLogger(repository, name);
       }

       public Log(string repository, Type type)
       {
           this.log = log4net.LogManager.GetLogger(repository, type);
       }

       public Log(Assembly repositoryAssembly, Type type)
       {
           this.log = log4net.LogManager.GetLogger(repositoryAssembly, type);
       }

       public Log(Assembly repositoryAssembly, string name)
       {
           this.log = log4net.LogManager.GetLogger(repositoryAssembly, name);
       }

        #region ILog Members

        public bool IsDebugEnabled
        {
            get { return this.log.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get
            {
               return this.log.IsErrorEnabled;
            }
        }

        public bool IsFatalEnabled
        {
            get { return this.log.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return this.log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return  this.log.IsWarnEnabled; }
        }

        public void Debug(object message)
        {
            this.log.Debug(message); ;
        }

        public void Debug(object message, Exception exception)
        {
            this.log.Debug(message, exception);
        }

        public void DebugFormat(string format, object arg0)
        {
            this.log.DebugFormat(format, arg0);
        }

        public void DebugFormat(string format, params object[] args)
        {
            this.log.DebugFormat(format, args);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.DebugFormat(provider, format, args);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            this.log.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            this.log.DebugFormat(format, arg0, arg1, arg2);
        }

        public void Error(object message)
        {
            this.log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            this.log.Error(message, exception);
        }

        public void ErrorFormat(string format, object arg0)
        {
            this.log.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            this.log.ErrorFormat(format, args);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.ErrorFormat(provider, format, args);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            this.log.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            this.log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void Fatal(object message)
        {
            this.log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            this.log.Fatal(message, exception);
        }

        public void FatalFormat(string format, object arg0)
        {
            this.log.FatalFormat(format, arg0);
        }

        public void FatalFormat(string format, params object[] args)
        {
            this.log.FatalFormat(format, args);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.FatalFormat(provider, format, args);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            this.log.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            this.log.FatalFormat(format, arg0, arg1, arg2);
        }

        public void Info(object message)
        {
            this.log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            this.log.Info(message, exception);
        }

        public void InfoFormat(string format, object arg0)
        {
            this.log.InfoFormat(format, arg0);
        }

        public void InfoFormat(string format, params object[] args)
        {
            this.log.InfoFormat(format, args);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.InfoFormat(provider, format, args);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            this.log.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            this.log.InfoFormat(format, arg0, arg1, arg2);
        }

        public void Warn(object message)
        {
            this.log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            this.log.Warn(message, exception);
        }

        public void WarnFormat(string format, object arg0)
        {
            this.log.WarnFormat(format, arg0);
        }

        public void WarnFormat(string format, params object[] args)
        {
            this.log.WarnFormat(format, args);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.WarnFormat(provider, format, args);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            this.log.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            this.log.WarnFormat(format, arg0, arg1, arg2);
        }

        #endregion
    }
}
