namespace Services.Pipeline.StateControl
{
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Xml.Serialization;

    public class ServiceControl
    {
        private static readonly IsolatedStorageFile Storage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

        public static void KeepState<T>(T serviceInfo) where T : ServiceInfo
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new StringWriter())
            {
                serializer.Serialize(stream, serviceInfo);
                stream.Flush();
                CreateOrUpdate(serviceInfo.Name, stream.ToString());
            }
        }

        public static void RemoveState(string serviceName)
        {
            string fileName = GetFileName(serviceName);
            if (FileExists(serviceName))
            {
                Storage.DeleteFile(fileName);
            }
        }

        public static T RecoverState<T>(string name) where T : ServiceInfo
        {
            if (!FileExists(name))
            {
                return null;
            }

            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(new IsolatedStorageFileStream(GetFileName(name), FileMode.Open, Storage)))
            {
                try
                {
                    return (T)serializer.Deserialize(reader);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(string.Format("Failed to {0} create object from xml string", ex));
                }
            }
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
