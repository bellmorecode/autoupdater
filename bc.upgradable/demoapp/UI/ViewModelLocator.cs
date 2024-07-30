namespace demoapp.UI
{
    public sealed class ViewModelLocator
    {
        public ViewModelLocator()
        {
            Demo = new DemoViewModel();
        }
        public DemoViewModel Demo { get; set; }
    }
}
