namespace Services.Pipeline.StateControl
{
    using System;

    public class ServiceInfo
    {
        public string Name { get; set; }
        public ServiceState State { get; set; }
        public DateTime LatestExecution { get; set; }
    }
}
