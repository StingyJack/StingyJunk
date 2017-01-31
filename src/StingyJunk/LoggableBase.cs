namespace StingyJunk
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using log4net;
     
    /// <summary>
    ///     Base that provides logging
    /// </summary>
    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    public abstract class LoggableBase
    {
        private static ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected virtual void ReinitializeLogger(ILog nonDefaultLogImpelementation)
        {
            if (_Log.Equals(nonDefaultLogImpelementation))
            {
                return;
            }
            _Log = nonDefaultLogImpelementation;
        }

        protected virtual void LogDebug(string message, Exception ex = null)
        {
            _Log.Debug(message, ex);
        }

        protected virtual void LogInfo(string message, Exception ex = null)
        {
            _Log.Info(message, ex);
        }

        protected virtual void LogWarn(string message, Exception ex = null)
        {
            _Log.Warn(message, ex);
        }

        protected virtual void LogErr(string message, Exception ex = null)
        {
            _Log.Error(message, ex);
        }

        protected virtual void LogFatal(string message, Exception ex = null)
        {
            _Log.Fatal(message, ex);
        }
    }
}
