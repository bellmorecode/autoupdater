using bc.upgradable;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Networking.NetworkOperators;

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
            await base.Init();

            // check for installed.
            if (!string.IsNullOrWhiteSpace(this.MediaLocation) && !string.IsNullOrWhiteSpace(this.InstallLocation))
            {
               
            }
        }
    }
}
