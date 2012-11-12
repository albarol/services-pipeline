namespace Services.Pipeline.Report.Logging
{
    using System.Collections.Generic;

    public interface ILogFacility
    {
        void WriteError(string message, params object[] args);
        void WriteSuccessAudit(string message, params object[] args);
        void WriteInformation(string message, params object[] args);
        void WriteInformation<T>(T obj);
        void WriteWarning(string message, params object[] args);
        void WriteFailureAudit(string message, params object[] args);
        void Clear();
        IList<Entry> ReadAll();
    }
}