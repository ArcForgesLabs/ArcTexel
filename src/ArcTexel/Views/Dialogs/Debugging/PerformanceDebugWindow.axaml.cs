using ArcTexel.Models.AnalyticsAPI;

namespace ArcTexel.Views.Dialogs.Debugging;

public partial class PerformanceDebugWindow : ArcTexelPopup
{
    public StartupPerformance StartupPerformance
    {
        get => MainWindow.Current.StartupPerformance;
    }
    
    public PerformanceDebugWindow()
    {
        InitializeComponent();
    }
}

