using System.Runtime.InteropServices;
using Microsoft.UI.Xaml.Controls;

using rgr.ViewModels;

namespace rgr.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel { get; }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }
}
