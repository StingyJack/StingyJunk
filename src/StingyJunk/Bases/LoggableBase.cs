namespace StingyJunk.Bases
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
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected virtual void ReinitializeLogger(ILog nonDefaultLogImpelementation)
        {
            if (_log.Equals(nonDefaultLogImpelementation))
            {
                return;
            }
            _log = nonDefaultLogImpelementation;
        }

        protected virtual void LogDebug(string message, Exception ex = null)
        {
            _log.Debug(message, ex);
        }

        protected virtual void LogInfo(string message, Exception ex = null)
        {
            _log.Info(message, ex);
        }

        protected virtual void LogWarn(string message, Exception ex = null)
        {
            _log.Warn(message, ex);
        }

        protected virtual void LogErr(string message, Exception ex = null)
        {
            _log.Error(message, ex);
        }

        protected virtual void LogFatal(string message, Exception ex = null)
        {
            _log.Fatal(message, ex);
        }
    }
}