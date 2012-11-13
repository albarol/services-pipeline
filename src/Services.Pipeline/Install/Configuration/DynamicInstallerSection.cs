namespace Services.Pipeline.Install.Configuration
{
    using System.Configuration;

    public class DynamicInstallerSection : ConfigurationSection
    {
        [ConfigurationProperty("credentials")]
        public CredentialsElement Credentials
        {
            get
            {
                return (CredentialsElement)this["credentials"];
            }
            set
            {
                this["credentials"] = value;
            }
        }

        [ConfigurationProperty("service")]
        public ServiceInfoElement ServiceInfo
        {
            get
            {
                return (ServiceInfoElement)this["info"];
            }
            set
            {
                this["info"] = value;
            }
        }

    }
}
