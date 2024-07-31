using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace demoapp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Trace.AutoFlush = true;

            Trace.Listeners.Add(new TextWriterTraceListener("demoapp.log"));
            Trace.TraceInformation("");
            Trace.TraceInformation("############################################################");
            Trace.TraceInformation($"Start Demo App - {Version} - {DateTime.Now:MM/dd hh:mm tt}");

            base.OnStartup(e);
        }

        public string Version { get; set; } = $"1.0.2.{DateTime.Now.Ticks}";
    }
}
