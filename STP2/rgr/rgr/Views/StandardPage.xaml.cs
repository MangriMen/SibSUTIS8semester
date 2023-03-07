using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using rgr.Behaviors;
using rgr.ViewModels;

namespace rgr.Views;

public sealed partial class StandardPage : Page
{
    public StandardViewModel ViewModel { get; }

    public StandardPage()
    {
        ViewModel = App.GetService<StandardViewModel>();
        InitializeComponent();
        SetBinding(
            NavigationViewHeaderBehavior.HeaderContextProperty,
            new Binding { Source = ViewModel, Mode = BindingMode.OneWay }
        );
    }
}
