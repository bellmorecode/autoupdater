using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;

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
        public bool IsInstalled { get; set; } = false;

        public List<MediaRegistryEntry> MediaRegistry { get; set; } = new List<MediaRegistryEntry>();
        public List<MediaRegistryEntry> DownloadedRegistry { get; set; } = new List<MediaRegistryEntry>();

        public virtual async Task<bool> Install(Guid entryId)
        {
            var installed = false;
            var entry = DownloadedRegistry.FirstOrDefault(q => q.Id == entryId);
            if (entry != null)
            {
                var files_to_copy = Directory.GetFiles($@"{MediaLocation}\{entry.Name}\{entry.Version}", "*.*");
            }
            return await Task.FromResult(installed);
        }

        public virtual async Task Init()
        {
            // check folders exists.
            if (!Directory.Exists(this.MediaLocation))
            {
                Directory.CreateDirectory(this.MediaLocation);
            }
            if (!Directory.Exists(this.InstallLocation))
            {
                Directory.CreateDirectory(this.InstallLocation);
            }

            MediaRegistry.Clear();
            DownloadedRegistry.Clear();

            // check the registry for new versions.
            if (!string.IsNullOrWhiteSpace(MediaUrl))
            {
                using (var cl = new HttpClient())
                {
                    try
                    {
                        var response = await cl.GetAsync(MediaUrl);
                        if (response != null)
                        {
                            var entries = await response.Content.ReadFromJsonAsync<MediaRegistryEntry[]>();
                            if (entries != null)
                            {
                                if (entries.Length > 0)
                                {
                                    MediaRegistry.AddRange(entries);
                                }
                                else
                                {
                                    MediaRegistry.Add(new MediaRegistryEntry { Name = "None Found", Path = "/", Version = "999.999.999.999" });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError($"{ex}");
                    }
                }

                var local_entries = Directory.GetFiles(MediaLocation, "*registryentry.json", SearchOption.AllDirectories);
                if (local_entries != null)
                {
                    foreach (var entry in local_entries)
                    {
                        var data_entry = JsonConvert.DeserializeObject<MediaRegistryEntry>(File.ReadAllText(entry));
                        if (data_entry != null)
                        {
                            DownloadedRegistry.Add(data_entry);
                        }
                    }
                }
            }
        }

        public virtual async Task<bool> Rollback()
        {
            var completedSuccessfully = false;
            if (this.CanRollback)
            {
                // TODO: try rollback?
                throw new NotImplementedException("Rollback not ready yet.");
            }
            return await Task.FromResult(completedSuccessfully);
        }

        public virtual void Dispose() { }
    }
}
