using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using rgr.Behaviors;
using rgr.ViewModels;

namespace rgr.Views;

public sealed partial class PNumberPage : Page
{
    public PNumberViewModel ViewModel { get; }

    public PNumberPage()
    {
        ViewModel = App.GetService<PNumberViewModel>();
        InitializeComponent();
        SetBinding(
            NavigationViewHeaderBehavior.HeaderContextProperty,
            new Binding { Source = ViewModel, Mode = BindingMode.OneWay }
        );
    }
}
