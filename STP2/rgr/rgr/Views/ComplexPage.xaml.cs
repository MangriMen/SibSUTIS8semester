using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using rgr.Behaviors;
using rgr.ViewModels;

namespace rgr.Views;

public sealed partial class ComplexPage : Page
{
    public ComplexViewModel ViewModel { get; }

    public ComplexPage()
    {
        ViewModel = App.GetService<ComplexViewModel>();
        InitializeComponent();
        SetBinding(
            NavigationViewHeaderBehavior.HeaderContextProperty,
            new Binding { Source = ViewModel, Mode = BindingMode.OneWay }
        );
    }
}
