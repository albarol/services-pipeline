namespace Services.Pipeline.Report.Logging.Providers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Services.Pipeline.Report.Logging;

    public class EventLogFacility : ILogFacility
    {
        private readonly EventLog eventLog;

        public EventLogFacility(string sourceName, string logName)
        {
            if (EventLog.SourceExists(sourceName))
            {
                EventLog.DeleteEventSource(sourceName);
            }

            var creationData = new EventSourceCreationData(sourceName, logName)
            {
                MachineName = System.Environment.MachineName 
            };
            EventLog.CreateEventSource(creationData);
            this.eventLog = new EventLog(logName, System.Environment.MachineName, sourceName);
        }

        public void WriteError(string message, params object[] args)
        {
            this.WriteInEventViewer(message, EventLogEntryType.Error, args);
        }

        public void WriteSuccessAudit(string message, params object[] args)
        {
            this.WriteInEventViewer(message, EventLogEntryType.SuccessAudit, args);
        }

        public void WriteInformation(string message, params object[] args)
        {
            this.WriteInEventViewer(message, EventLogEntryType.Information, args);
        }
        
        public void WriteInformation<T>(T obj)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var builder = new StringBuilder();
            foreach (var property in properties)
            {
                builder.AppendFormat("{0}: {1}\r\n", property.Name, property.GetValue(obj, null));
            }

            this.WriteInformation(builder.ToString());
        }
        
        public void WriteWarning(string message, params object[] args)
        {
            this.WriteInEventViewer(message, EventLogEntryType.Warning, args);
        }
        
        public void WriteFailureAudit(string message, params object[] args)
        {
            this.WriteInEventViewer(message, EventLogEntryType.FailureAudit, args);
        }

        public void Clear()
        {
            this.eventLog.Clear();
        }

        public IList<Entry> ReadAll()
        {
            return (from EventLogEntry entry in this.eventLog.Entries
            select new Entry
            {
                Message = entry.Message,
                EntryType = this.ConvertTo(entry.EntryType), 
                TimeGenerated = entry.TimeGenerated
            }).ToList();
        }

        private EntryType ConvertTo(EventLogEntryType type)
        {
            switch (type)
            {
                case EventLogEntryType.Information:
                    return EntryType.Information;
                case EventLogEntryType.Warning:
                    return EntryType.Warning;
                case EventLogEntryType.Error:
                    return EntryType.Error;
                case EventLogEntryType.SuccessAudit:
                    return EntryType.SuccessAudit;
                case EventLogEntryType.FailureAudit:
                    return EntryType.FailureAudit;
                default:
                    return EntryType.Information;
            }
        }

        private void WriteInEventViewer(string message, EventLogEntryType type, params object[] args)
        {
            this.eventLog.WriteEntry(string.Format(message, args), type);
        }
    }
}
