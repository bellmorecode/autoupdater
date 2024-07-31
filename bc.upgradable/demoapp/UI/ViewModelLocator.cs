using System.Diagnostics;

namespace demoapp.UI
{
    public sealed class ViewModelLocator
    {
        public ViewModelLocator()
        {
            Trace.TraceInformation("Create ViewModel");
            Demo = new DemoViewModel();
        }
        public DemoViewModel Demo { get; set; }
    }
}
