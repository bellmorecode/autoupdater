namespace bc.upgradable
{
    public interface IUpgrader
    {
        public DateTime LastInstallDate { get; set; }
        public string InstalledVersion { get; set; }

        public string LatestVersion { get; set; }

        public string DownloadedVersion { get; set; }

        public bool CanRollback { get; set; }

        public Task<bool> Upgrade();
        public Task<bool> Rollback();

    }
}
