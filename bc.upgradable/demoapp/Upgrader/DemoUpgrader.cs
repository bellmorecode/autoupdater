using bc.upgradable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoapp.Upgrader
{
    public sealed class DemoUpgrader : UpgraderBase
    {
        public DemoUpgrader()
        {
            this.CanRollback = false;
            // this.InstallLocation
            // this.DownloadedVersion
        }
        public override async Task<bool> Rollback()
        {
            throw new NotImplementedException();
        }
        public override async Task<bool> Upgrade()
        {
            throw new NotImplementedException();
        }
    }
}
