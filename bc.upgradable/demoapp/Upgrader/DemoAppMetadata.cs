using bc.upgradable;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
            foreach(var item in Agent.MediaRegistry.OrderBy(x => x.Name).ThenBy(y => y.Version))
            {
                Trace.TraceWarning($"{item}");
                var locRelPath = item.Path.Substring(1).Replace("/", "\\");
                var cloudPath = Agent.MediaUrl.Replace("/registry.json", item.Path);
                var fileListDownloadPath = $"{Agent.MediaLocation}{locRelPath}";
                var registryDownloadPath = fileListDownloadPath.Replace("files.txt", "registryentry.json");
                var installVersionDir = Path.GetDirectoryName(fileListDownloadPath);
                if (!Directory.Exists(installVersionDir) && installVersionDir != null)
                {
                    Directory.CreateDirectory(installVersionDir);
                }

                File.WriteAllText(registryDownloadPath, JsonConvert.SerializeObject(item));

                using (var client = new HttpClient())
                {
                    var downloadTask = client.GetAsync(cloudPath);
                    downloadTask.Wait();

                    var res = downloadTask.Result;
                    var readTask = res.Content.ReadAsByteArrayAsync();
                    readTask.Wait();
                    File.WriteAllBytes(fileListDownloadPath, readTask.Result);

                }
                var content_files = File.ReadAllLines(fileListDownloadPath);
                foreach(var file in content_files)
                {
                    var contentUrl = Agent.MediaUrl.Replace("/registry.json", $"/{item.Version}/{file}");
                    var contentDownloadPath = $@"{Agent.MediaLocation}\{item.Version}\{file}";
                    using (var client = new HttpClient())
                    {
                        var downloadTask = client.GetAsync(contentUrl);
                        downloadTask.Wait();

                        var res = downloadTask.Result;
                        var readTask = res.Content.ReadAsByteArrayAsync();
                        readTask.Wait();
                        File.WriteAllBytes(contentDownloadPath, readTask.Result);
                    }
                }
                App.Current.Dispatcher.BeginInvoke(new Action<object?>(arg =>
                {
                    if (arg != null && arg is MediaRegistryEntry)
                    {

                    }
                }), item);
            }
        }

        public bool Install(Guid entryId)
        {
            var result = false;
            var installerTask = this.Agent.Install(entryId);
            installerTask.Wait();
            result = installerTask.Result;
            return result;
        }

        public void Refresh()
        {
            var q = Task.Factory.StartNew(_agent =>
            {
                if (_agent != null)
                {
                    Trace.TraceInformation("Init Upgrader... ");
                    IUpgrader upgrader = (IUpgrader)_agent;
                    var task = upgrader.Init();
                    task.Wait();
                    Trace.TraceInformation("Init Upgrader... DONE");
                }
            }, this.Agent);

            q.Wait();
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

        public void Upload()
        {
            var entry = new MediaRegistryEntry {  Name = AppFolderName, Path = "/1.0.0.0/files.txt", Version = "1.0.0.0" };
            var json = JsonConvert.SerializeObject(entry, Formatting.Indented);
        }

        //public void Upload(MediaRegistryEntry entry, byte[] file)
        //{

        //}
    }
}
