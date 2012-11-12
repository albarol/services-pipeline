namespace Services.Pipeline.Report.Logging
{
    using System;

    public class Entry
    {
        public string Message { get; set; }
        public EntryType EntryType { get; set; }
        public DateTime TimeGenerated { get; set; }
    }
}
