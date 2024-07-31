namespace bc.upgradable
{
    public interface IUpgrader: IDisposable
    {
        public List<MediaRegistryEntry> MediaRegistry { get; set; }
        public List<MediaRegistryEntry> DownloadedRegistry { get; set; }
        public DateTime LastInstallDate { get; set; }

        public string InstalledVersion { get; set; }

        public string LatestVersion { get; set; }

        public string DownloadedVersion { get; set; }

        public bool IsInstalled { get; set; }

        public bool CanRollback { get; set; }

        public string MediaUrl { get; }
        public string MediaLocation { get;  }
        public string InstallLocation { get; }

        public Task Init();
        public Task<bool> Install(Guid entryId);
        public Task<bool> Rollback();
    }
}
