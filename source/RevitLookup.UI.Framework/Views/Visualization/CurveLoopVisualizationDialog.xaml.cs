// RevitLookup.UI.Framework/Views/Visualization/CurveLoopVisualizationDialog.xaml.cs
using RevitLookup.Abstractions.Services.Appearance;
using RevitLookup.Abstractions.ViewModels.Visualization;
using Wpf.Ui;

namespace RevitLookup.UI.Framework.Views.Visualization
{
    public sealed partial class CurveLoopVisualizationDialog: Wpf.Ui.Controls.ContentDialog
    {
        private readonly ICurveLoopVisualizationViewModel _viewModel;

        public CurveLoopVisualizationDialog(
            IContentDialogService dialogService,
            ICurveLoopVisualizationViewModel viewModel,
            IThemeWatcherService themeWatcherService)
            : base(dialogService.GetDialogHost())
        {
            _viewModel = viewModel;

            DataContext = _viewModel;
            InitializeComponent();

            themeWatcherService.Watch(this);
        }

        public async Task ShowDialogAsync(object curveLoop)
        {
            _viewModel.RegisterServer(curveLoop);
            MonitorServerConnection();

            await ShowAsync();
        }

        private void MonitorServerConnection()
        {
           Unloaded += (_, _) => _viewModel.UnregisterServer();
        }
    }
}