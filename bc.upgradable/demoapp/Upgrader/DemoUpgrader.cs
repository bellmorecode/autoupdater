using bc.upgradable;
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
    public sealed class DemoAppMetadata : IAppMetadata
    {
        public DemoAppMetadata()
        {
            DisplayName = "Demo App";
            AppFolderName = "demoapp";
            Agent = new DemoUpgrader( AppFolderName );
        }



        public IUpgrader Agent {get; private set;}

        public string AppFolderName { get; set; }

        public string DisplayName { get; set; }

        public void Download()
        {
            throw new NotImplementedException();
        }

        public void Install()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            Task.Factory.StartNew(_agent =>
            {
                if (_agent != null)
                {
                    IUpgrader upgrader = (IUpgrader)_agent;
                    var task = upgrader.Init();
                    task.Wait();

                }
            }, this.Agent);
        }

        public void Rollback()
        {
            if (Agent != null && Agent.CanRollback)
            {
                var rollbackTask = Agent.Rollback();
                if (rollbackTask != null)
                {
                    rollbackTask.Wait();
                    var rollbackSucceeded = rollbackTask.Result;
                    // TODO: report somehow
                }
            }
        }

        public void Uninstall()
        {
            throw new NotImplementedException();
        }
    }

    public sealed class DemoUpgrader : UpgraderBase
    {
        public DemoUpgrader(string appFolderName)
        {
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
            // check folders exists.
            if (!Directory.Exists(this.MediaLocation))
            {
                Directory.CreateDirectory(this.MediaLocation);
            }
            if (!Directory.Exists(this.InstallLocation))
            {
                Directory.CreateDirectory(this.InstallLocation);
            }
            // check the registry for new versions.
            if (!string.IsNullOrWhiteSpace(MediaUrl))
            {
                var cl = new HttpClient();
                try
                {
                    var response = await cl.GetAsync(MediaUrl);
                    if (response != null)
                    {

                        var entries = await response.Content.ReadFromJsonAsync<MediaRegistryEntry[]>();
                        if (entries != null)
                        {
                            MediaRegistry.AddRange(entries);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Trace.TraceError($"{ex}");
                }
            }

            await base.Init();
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
        public override async Task<bool> Upgrade()
        {
            return await Task.FromResult(true);
        }
    }
}
