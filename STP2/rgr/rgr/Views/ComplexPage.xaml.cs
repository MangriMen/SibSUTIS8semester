using Microsoft.UI.Xaml.Controls;

using rgr.ViewModels;

namespace rgr.Views;

public sealed partial class ComplexPage : Page
{
    public ComplexViewModel ViewModel
    {
        get;
    }

    public ComplexPage()
    {
        ViewModel = App.GetService<ComplexViewModel>();
        InitializeComponent();
    }
}
