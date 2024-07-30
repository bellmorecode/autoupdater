namespace bc.upgradable
{
    public interface IUpgrader: IDisposable
    {
        List<MediaRegistryEntry> MediaRegistry { get; set; }
        public DateTime LastInstallDate { get; set; }

        public string InstalledVersion { get; set; }

        public string LatestVersion { get; set; }

        public string DownloadedVersion { get; set; }

        public bool CanRollback { get; set; }

        public Task Init();
        public Task<bool> Upgrade();
        public Task<bool> Rollback();

    }
}
