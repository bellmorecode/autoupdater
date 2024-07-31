using Azure.Storage.Blobs;
using bc.upgradable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace demoapp.Upgrader
{

    public sealed class DemoUpgrader : UpgraderBase
    {
        public DemoUpgrader(string appFolderName)
        {
            Trace.TraceInformation("DemoUpgrader::ctor");
            MediaUrl = $@"https://gfd.blob.core.windows.net/autoupdater/{appFolderName}/registry.json";
            MediaLocation = $@"c:\temp\{appFolderName}\";
            InstallLocation = $@"c:\program files\{appFolderName}\";
        }

        public DemoUpgrader(string downloadSource, string downloadPath, string installPath)
        {
            MediaUrl = downloadSource;
            MediaLocation = downloadPath;
            InstallLocation = installPath;
            CanRollback = false;
        }

        public override async Task Init()
        {
            Trace.TraceInformation("DemoUpgrader.Init");
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
                    foreach(var entry in local_entries)
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

        public override async Task<bool> Rollback()
        {
            var completedSuccessfully = false;
            if (this.CanRollback)
            {
                // TODO: try rollback?
                throw new NotImplementedException("Rollback not ready yet.");
            }
            return await Task.FromResult(completedSuccessfully);
        }
        public override async Task<bool> Install()
        {
            Trace.TraceInformation("Install!");
            return await Task.FromResult(true);
        }
    }
}
