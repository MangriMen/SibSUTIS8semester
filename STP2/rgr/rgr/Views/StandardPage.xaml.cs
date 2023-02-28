using Microsoft.UI.Xaml.Controls;

using rgr.ViewModels;

namespace rgr.Views;

public sealed partial class StandardPage : Page
{
    public StandardViewModel ViewModel { get; }

    public StandardPage()
    {
        ViewModel = App.GetService<StandardViewModel>();
        InitializeComponent();
    }
}
