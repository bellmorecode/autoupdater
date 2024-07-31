using demoapp.UI;
using Microsoft.Extensions.Azure;
using System;
using System.Diagnostics;
using System.Windows;
namespace demoapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is MainWindow)
            {
                MainWindow w = (MainWindow)sender;
                if (w.DataContext != null && w.DataContext is DemoViewModel)
                {
                    Trace.TraceInformation("Window_Loaded");
                    ((DemoViewModel)w.DataContext).RefreshButtonClick.Execute(null);
                }
            }
        }
    }
}
