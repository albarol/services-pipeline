namespace Services.Pipeline.Install
{
    using System.Collections;
    using System.Configuration.Install;
    using System.ServiceProcess;

    using Services.Pipeline.Install.Configuration;

    public class DynamicInstaller : Installer
    {
        protected readonly ServiceProcessInstaller ProcessInstaller;
        protected readonly ServiceInstaller ServiceInstaller;

        protected DynamicInstaller()
        {
            this.ProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalService,
                Username = null,
                Password = null
            };
            this.ServiceInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Automatic,
                ServiceName = "DynamicService",
                DisplayName = string.Empty,
                Description = string.Empty
            };
            Installers.AddRange(new Installer[] {this.ProcessInstaller, this.ServiceInstaller});
        }
    
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);
            var config = (DynamicInstallerSection)System.Configuration.ConfigurationManager.GetSection("dynamicInstallerGroup/dynamicInstaller");
            this.ServiceInstaller.ServiceName = config.ServiceInfo.Name;
            this.ServiceInstaller.Description = config.ServiceInfo.Description;
            this.ServiceInstaller.StartType = config.ServiceInfo.StartType;
            this.ServiceInstaller.DisplayName = config.ServiceInfo.DisplayName;
            this.ProcessInstaller.Account = config.Credentials.Account;

            if (config.Credentials.Account == ServiceAccount.User)
            {
                this.ProcessInstaller.Username = config.Credentials.Username;
                this.ProcessInstaller.Password = config.Credentials.Password;
            }
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);
            var config = (DynamicInstallerSection)System.Configuration.ConfigurationManager.GetSection("dynamicInstallerGroup/dynamicInstaller");
            this.ServiceInstaller.ServiceName = config.ServiceInfo.Name;
        }
    }
}
