namespace MetricsConfiguration.Infrastructure.Environment
{
    public class AuthorizationApps
    {
        public Apps[] Apps { get; set; }
    }

    public class Apps
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
