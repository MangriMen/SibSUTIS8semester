using Microsoft.UI.Xaml.Controls;

using rgr.ViewModels;

namespace rgr.Views;

public sealed partial class PNumberPage : Page
{
    public PNumberViewModel ViewModel
    {
        get;
    }

    public PNumberPage()
    {
        ViewModel = App.GetService<PNumberViewModel>();
        InitializeComponent();
    }
}
