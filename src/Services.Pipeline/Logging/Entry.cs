namespace Services.Pipeline.Logging
{
    using System;
    using System.Diagnostics;

    public class Entry
    {
        public string Message { get; set; }
        public EventLogEntryType EntryType { get; set; }
        public DateTime TimeGenerated { get; set; }
    }
}
