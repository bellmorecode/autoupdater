using bc.upgradable;
using demoapp.Instrumentation;
using demoapp.Upgrader;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace demoapp.UI
{
    public class DemoViewModel : ViewModelBase
    {
        /// <summary>
        /// This is the Demo View Model Constructor
        /// </summary>
        public DemoViewModel ()
        {
            // initialize Commands
            RefreshButtonClick = new SimpleCommand(this.Refresh);
            DownloadButtonClick = new SimpleCommand(this.Download);
            InstallButtonClick = new SimpleCommand(this.Install);
            RollbackButtonClick = new SimpleCommand(this.Rollback);
            UninstallButtonClick = new SimpleCommand(this.Uninstall);
            UploadButtonClick = new SimpleCommand(this.Upload);
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

        private MediaRegistryEntry? _SelectedEntry = null;
        public MediaRegistryEntry? SelectedEntry
        {
            get { return _SelectedEntry; }
            set
            {
                if (_SelectedEntry == value) return;
                _SelectedEntry = value;
                RaisePropertyChanged("SelectedEntry");
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


        private string _VersionString = "X.X.X.X";
        public string VersionString
        {
            get { return _VersionString; }
            set
            {
                if (_VersionString == value) return;
                _VersionString = value;
                RaisePropertyChanged("VersionString");
            }
        }

        public SimpleCommand RefreshButtonClick { get; set; }
        public SimpleCommand DownloadButtonClick { get; set; }
        public SimpleCommand InstallButtonClick { get; set; }
        public SimpleCommand RollbackButtonClick { get; set; }
        public SimpleCommand UninstallButtonClick { get; set; }
        public SimpleCommand UploadButtonClick { get; set; }

        public ObservableCollection<MediaRegistryEntry> CloudRegistry { get; set; } = new ObservableCollection<MediaRegistryEntry>();
        public ObservableCollection<MediaRegistryEntry> LocalRegistry { get; set; } = new ObservableCollection<MediaRegistryEntry>();

        // implementations
        private void Refresh(object? state)
        {
            try
            {
                Trace.TraceInformation("refresh-button-click");
                if (AppMetadata != null)
                {
                    AppMetadata.Refresh();
                    Trace.TraceInformation("registry loaded");
                    App.Current.Dispatcher.BeginInvoke(new Action<object?>(iup =>
                    {
                        LocalRegistry.Clear();
                        CloudRegistry.Clear();
                        if (iup != null && iup is IUpgrader)
                        {
                            var agent = (IUpgrader)iup;
                            foreach (var item in agent.MediaRegistry)
                            {
                                CloudRegistry.Add(item);
                            }
                            foreach (var item in agent.DownloadedRegistry)
                            {
                                LocalRegistry.Add(item);
                            }
                            Trace.TraceInformation("view-model loaded");
                        }
                    }), AppMetadata.Agent);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Refresh: {ex}");
            }
        }

        private void Download(object? state)
        {
            try
            {
                AppMetadata?.Download();
                Refresh(state);
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
                var selectedEntry = SelectedEntry;
                if (selectedEntry != null)
                {
                    AppMetadata?.Install(selectedEntry.Id);
                }
                else
                {
                    MessageBox.Show("Select an app to install.");
                }

            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Any())
                {
                    var io = ex.InnerExceptions.First() as InvalidOperationException;
                    if (io != null)
                    {
                        MessageBox.Show(io.Message);
                        return;
                    }
                }

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

        private void Upload(object? state)
        {
            try
            {
                AppMetadata?.Upload();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }
    }
}
