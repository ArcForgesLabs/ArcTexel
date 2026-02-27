using Avalonia.Controls;
using ArcTexel.Models.ExceptionHandling;
using CrashReportViewModel = ArcTexel.ViewModels.CrashReportViewModel;
using ViewModels_CrashReportViewModel = ArcTexel.ViewModels.CrashReportViewModel;

namespace ArcTexel.Views.Dialogs;

/// <summary>
/// Interaction logic for CrashReportDialog.xaml
/// </summary>
internal partial class CrashReportDialog : Window
{
    public CrashReportDialog(CrashReport report)
    {
        DataContext = new ViewModels_CrashReportViewModel(report);
        InitializeComponent();
    }
}
