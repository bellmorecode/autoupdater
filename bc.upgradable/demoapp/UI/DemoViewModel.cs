using demoapp.Instrumentation;
using demoapp.Upgrader;
using System;
using System.Windows;

namespace demoapp.UI
{
    public class DemoViewModel : ViewModelBase
    {
        /// <summary>
        /// This is the Demo View Model Constructor
        /// </summary>
        public DemoViewModel () {

            // initialize Commands
            RefreshButtonClick = new SimpleCommand(this.Refresh);
            DownloadButtonClick = new SimpleCommand(this.Download);
            InstallButtonClick = new SimpleCommand(this.Install);
            RollbackButtonClick = new SimpleCommand(this.Rollback);
            UninstallButtonClick = new SimpleCommand(this.Uninstall);
        }
        private string _Title = "Demo App";
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title == value) return;
                _Title = value;
                RaisePropertyChanged("Title");
            }
        }


        private DemoAppMetadata _AppMetadata = new DemoAppMetadata();
        public DemoAppMetadata AppMetadata
        {
            get { return _AppMetadata; }
            set
            {
                if (_AppMetadata == value) return;
                _AppMetadata = value;
                RaisePropertyChanged("AppMetadata");
            }
        }

        public SimpleCommand RefreshButtonClick { get; set; }
        public SimpleCommand DownloadButtonClick { get; set; }
        public SimpleCommand InstallButtonClick { get; set; }
        public SimpleCommand RollbackButtonClick { get; set; }
        public SimpleCommand UninstallButtonClick { get; set; }

        // implementations
        private void Refresh(object? state)
        {
            try
            {
                AppMetadata?.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        private void Download(object? state)
        {
            try
            {
                AppMetadata?.Download();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        private void Install(object? state)
        {
            try
            {
                AppMetadata?.Install();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        private void Rollback(object? state)
        {
            try
            {
                AppMetadata?.Rollback();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        private void Uninstall(object? state)
        {
            try
            {
                AppMetadata?.Uninstall();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }
    }
}
