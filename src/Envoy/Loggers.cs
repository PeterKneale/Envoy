using System;

namespace Envoy
{
    public class TraceLogger : ILogger
    {
        public void LogDebug(string message) => System.Diagnostics.Trace.WriteLine(message);

        public void LogInfo(string message) => System.Diagnostics.Trace.WriteLine(message);

        public void LogWarn(string message) => System.Diagnostics.Trace.TraceWarning(message);

        public void LogError(string message) => System.Diagnostics.Trace.TraceError(message);

        public void LogException(string message, Exception ex) => System.Diagnostics.Trace.TraceError("{0} {1}", message, ex?.StackTrace);
    }

    public class NullLogger : ILogger
    {
        public void LogDebug(string message)
        {
        }

        public void LogError(string message)
        {
        }

        public void LogException(string message, Exception ex)
        {
        }

        public void LogInfo(string message)
        {
        }

        public void LogWarn(string message)
        {
        }
    }
}