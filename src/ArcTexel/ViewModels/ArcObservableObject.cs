using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ArcTexel.Extensions.CommonApi.UserPreferences.Settings;
using ArcTexel.Models.Commands;

namespace ArcTexel.ViewModels;

public class ArcObservableObject : ObservableObject
{
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        CommandController.Current.NotifyPropertyChanged(e.PropertyName);
    }

    protected void SubscribeSettingsValueChanged<T>(Setting<T> settingStore, string propertyName) =>
        settingStore.ValueChanged += (_, _) => OnPropertyChanged(propertyName);
}
