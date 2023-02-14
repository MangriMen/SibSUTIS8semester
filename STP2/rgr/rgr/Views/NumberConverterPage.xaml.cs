using Microsoft.UI.Xaml.Controls;

using rgr.ViewModels;

namespace rgr.Views;

public sealed partial class NumberConverterPage : Page
{
    public NumberConverterViewModel ViewModel
    {
        get;
    }

    public NumberConverterPage()
    {
        ViewModel = App.GetService<NumberConverterViewModel>();
        InitializeComponent();
    }
}
