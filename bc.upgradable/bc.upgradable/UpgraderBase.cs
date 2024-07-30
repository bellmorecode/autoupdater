namespace bc.upgradable
{
    public abstract class UpgraderBase : IUpgrader
    {
        protected string MediaUrl { get; set; } = @"https://gfd.blob.core.windows.net/autoupdater/[appfolder]/registry.json";
        protected string MediaLocation { get; set; } = @"c:\temp\";
        protected string InstallLocation { get; set; } = @"c:\program files\";

        public string InstalledVersion { get; set; } = "0.0.0.0";
        public string LatestVersion { get; set; } = "0.0.0.0";
        public string DownloadedVersion { get; set; } = "0.0.0.0";
        public DateTime LastInstallDate { get; set; }
        public bool CanRollback { get; set; } = false;

        public List<MediaRegistryEntry> MediaRegistry { get; set; } = new List<MediaRegistryEntry>();

        public abstract Task<bool> Rollback();

        public abstract Task<bool> Upgrade();

        public virtual async Task Init()
        {
            // nothing yet
            await Task.Yield();
        }

        public virtual void Dispose() { }
    }
}
