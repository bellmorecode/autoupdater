using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Security.Permissions;
using System.Security.Principal;

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

        public void ExecuteAsAdmin(string filename)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = filename;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(filename);
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }

        private bool? _isAdmin = null;

        public bool IsAdmininstrator
        {
            get
            {
                if (_isAdmin == null)
                {
                    // elevate to install
                    WindowsIdentity identity = WindowsIdentity.GetCurrent();
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    _isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
                return _isAdmin.GetValueOrDefault();
            }
            set { ; }
        }


        public virtual async Task<bool> Install(Guid entryId)
        {


            if (!IsAdmininstrator)
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var name = AppDomain.CurrentDomain.FriendlyName;

                var filename = $"{dir}{name}.exe";
                ExecuteAsAdmin(filename);

                return true;
            }

            var installed = false;
            var entry = DownloadedRegistry.FirstOrDefault(q => q.Id == entryId);
            if (entry != null)
            {
                var files_to_copy = Directory.GetFiles($@"{MediaLocation}\{entry.Version}", "*.*", SearchOption.AllDirectories);
                foreach(var file in files_to_copy)
                {
                    var dest_path = $@"{InstallLocation}\{Path.GetFileName(file)}";
                    File.Copy(file, dest_path, true);
                }

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
