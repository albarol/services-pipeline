namespace Services.Pipeline.StateControl
{
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Text;
    using System.Xml;

    public class ServiceControl
    {
        private static readonly IsolatedStorageFile Storage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

        public static void KeepState(ServiceInfo serviceInfo)
        {
            var builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\"?>");
            builder.AppendFormat("<{0}>", serviceInfo.Name);
            builder.AppendFormat("<State>{0}</State>", (int)serviceInfo.State);
            builder.AppendFormat("<LatestExecution>{0}</LatestExecution>", serviceInfo.LatestExecution.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.AppendFormat("</{0}>", serviceInfo.Name);
            CreateOrUpdate(serviceInfo.Name, builder.ToString());
        }

        public static void RemoveState(string serviceName)
        {
            string fileName = GetFileName(serviceName);
            if (FileExists(serviceName))
            {
                Storage.DeleteFile(fileName);
            }
        }

        public static ServiceInfo RecoverState(string name)
        {
            if (!FileExists(name))
            {
                return null;
            }

            var document = new XmlDocument();
            using (var reader = new StreamReader(new IsolatedStorageFileStream(GetFileName(name), FileMode.Open, Storage)))
            {
                document.LoadXml(reader.ReadToEnd());
            }

            return new ServiceInfo
            {
                Name = name,
                LatestExecution = Convert.ToDateTime(document.ChildNodes[1].ChildNodes[1].InnerText),
                State = (ServiceState)Convert.ToInt32(document.ChildNodes[1].ChildNodes[0].InnerText)
            };
        }

        private static void CreateOrUpdate(string name, string content)
        {
            var isolatedStream = new IsolatedStorageFileStream(GetFileName(name), FileMode.OpenOrCreate, Storage);
            isolatedStream.SetLength(0);
            var writer = new StreamWriter(isolatedStream);
            writer.WriteLine(content);
            writer.Close();
        }

        private static bool FileExists(string name)
        {
            string[] fileNames = Storage.GetFileNames(GetFileName(name));
            return fileNames.Length > 0;
        }

        private static string GetFileName(string serviceName)
        {
            return string.Format("{0}.txt", serviceName.ToLower());
        }
    }
}
