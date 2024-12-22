using bc.upgradable;
using System.Diagnostics;

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
