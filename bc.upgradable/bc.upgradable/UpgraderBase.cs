namespace bc.upgradable
{
    public abstract class UpgraderBase : IUpgrader
    {
        public string MediaUrl { get; protected set; } = @"https://gfd.blob.core.windows.net/autoupdater/[appfolder]/registry.json";
        public string MediaLocation { get; protected set; } = @"c:\temp\";
        public string InstallLocation { get; protected set; } = @"c:\program files\";

        public string InstalledVersion { get; set; } = "0.0.0.0";
        public string LatestVersion { get; set; } = "0.0.0.0";
        public string DownloadedVersion { get; set; } = "0.0.0.0";
        public DateTime LastInstallDate { get; set; }
        public bool CanRollback { get; set; } = false;

        public List<MediaRegistryEntry> MediaRegistry { get; set; } = new List<MediaRegistryEntry>();
        public List<MediaRegistryEntry> DownloadedRegistry { get; set; } = new List<MediaRegistryEntry>();

        public abstract Task<bool> Rollback();

        public abstract Task<bool> Install();

        public virtual async Task Init()
        {
            // nothing yet
            await Task.Yield();
        }

        public virtual void Dispose() { }
    }
}
