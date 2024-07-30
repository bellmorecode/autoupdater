namespace bc.upgradable
{
    public interface IAppMetadata
    {
        public IUpgrader Agent { get; }

        public void Refresh();
        public void Download();
        public void Install();
        public void Rollback();
        public void Uninstall();
    }

    public sealed class MediaRegistryEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = "";

        public string Version { get; set; } = "0.0.0.0";

        public string Path { get; set; } = "/";
    }
}
