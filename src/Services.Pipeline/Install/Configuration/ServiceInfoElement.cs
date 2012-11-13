namespace Services.Pipeline.Install.Configuration
{
    using System;
    using System.Configuration;
    using System.ServiceProcess;

    public class ServiceInfoElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return Convert.ToString(this["name"]);
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("displayName")]
        public string DisplayName
        {
            get
            {
                return Convert.ToString(this["displayName"]);
            }
            set
            {
                this["displayName"] = value;
            }
        }

        [ConfigurationProperty("description", IsRequired = true)]
        public string Description
        {
            get
            {
                return Convert.ToString(this["description"]);
            }
            set
            {
                this["password"] = value;
            }
        }

        [ConfigurationProperty("startType", DefaultValue = "2", IsRequired = true)]
        public ServiceStartMode StartType
        {
            get
            {
                return (ServiceStartMode)Convert.ToInt32(this["startType"]);
            }
            set
            {
                this["startType"] = value;
            }
        }
    }
}
