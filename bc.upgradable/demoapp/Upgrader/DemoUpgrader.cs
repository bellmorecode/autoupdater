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
    }
}
