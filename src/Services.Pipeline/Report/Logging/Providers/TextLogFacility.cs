namespace Services.Pipeline.Report.Logging.Providers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using Services.Pipeline.Report.Logging;

    public class TextLogFacility : ILogFacility
    {
        private static readonly object LockObject = new object();
        private readonly string fileName;
        
        public TextLogFacility(string filename)
        {
            this.fileName = filename;
        }
        
        public void WriteError(string message, params object[] args)
        {
            this.Write(string.Format("{0}\t[ERROR]\t{1}", DateTime.Now, string.Format(message, args)));
        }

        public void WriteSuccessAudit(string message, params object[] args)
        {
            this.Write(string.Format("{0}\t[SUCCESS_AUDIT]\t{1}", DateTime.Now, string.Format(message, args)));
        }

        public void WriteInformation(string message, params object[] args)
        {
            this.Write(string.Format("{0}\t[INFORMATION]\t{1}", DateTime.Now, string.Format(message, args)));
        }

        public void WriteInformation<T>(T obj)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var builder = new StringBuilder();
            foreach (var property in properties)
            {
                builder.AppendFormat("{0}: {1} - ", property.Name, property.GetValue(obj, null));
            }

            builder.Remove(builder.Length - 2, 2);
            this.WriteInformation(builder.ToString());
        }

        public void WriteWarning(string message, params object[] args)
        {
            this.Write(string.Format("{0}\t[WARNING]\t{1}", DateTime.Now, string.Format(message, args)));
        }

        public void WriteFailureAudit(string message, params object[] args)
        {
            this.Write(string.Format("{0}\t[FAILURE_AUDIT]\t{1}", DateTime.Now, string.Format(message, args)));
        }

        public void Clear()
        {
            lock (LockObject)
            {
                using (var stream = File.Open(this.fileName, FileMode.OpenOrCreate))
                {
                    stream.SetLength(0);
                }
            }
        }

        public IList<Entry> ReadAll()
        {
            var entries = new List<Entry>();
            lock (LockObject)
            {
                using (var reader = new StreamReader(File.Open(this.fileName, FileMode.OpenOrCreate)))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] data = reader.ReadLine().Split('\t');
                        entries.Add(new Entry
                        {
                            Message = data[2],
                            EntryType = this.ConvertTo(data[1]),
                            TimeGenerated = Convert.ToDateTime(data[0])
                        });
                    }
                }
            }

            return entries;
        }

        private EntryType ConvertTo(string type)
        {
            switch (type)
            {
                case "[INFORMATION]":
                    return EntryType.Information;
                case "[WARNING]":
                    return EntryType.Warning;
                case "[ERROR]":
                    return EntryType.Error;
                case "[SUCCESS_AUDIT]":
                    return EntryType.SuccessAudit;
                case "[FAILURE_AUDIT]":
                    return EntryType.FailureAudit;
                default:
                    return EntryType.Information;
            }
        }

        private void Write(string message)
        {
            lock (LockObject)
            {
                using (var stream = File.Open(this.fileName, FileMode.OpenOrCreate))
                {
                    stream.Position = stream.Length;
                    var writer = new StreamWriter(stream);
                    writer.WriteLine(message);
                    writer.Close();
                }
            }
        }
    }
}
