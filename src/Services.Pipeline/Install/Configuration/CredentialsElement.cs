namespace Services.Pipeline.Install.Configuration
{
    using System;
    using System.Configuration;
    using System.ServiceProcess;

    public class CredentialsElement : ConfigurationElement
    {
        [ConfigurationProperty("account", DefaultValue = "3", IsRequired = true)]
        public ServiceAccount Account
        {
            get
            {
                return (ServiceAccount)Convert.ToInt32(this["name"]);
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("username", IsRequired = true)]
        public string Username
        {
            get
            {
                return Convert.ToString(this["username"]);
            }
            set
            {
                this["username"] = value;
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                return Convert.ToString(this["password"]);
            }
            set
            {
                this["password"] = value;
            }
        }
    }
}
