using CommunityToolkit.Mvvm.ComponentModel;
using lab5;
using lab9;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using rgr.Controls;
using rgr.Helpers;
using rgr.Models;

namespace rgr.ViewModels;

public class StandardViewModel : ObservableRecipient
{
    public readonly Calculator<FractionEditor, Fraction> Calculator = new();

    public TextBlock? _mainInputObject;

    public StandardViewModel() { }

    public void CalculatorButtonClick(object sender, RoutedEventArgs e)
    {
        var button = (CalculatorButton)sender;
        if (button == null || button.Type == CalculatorButton.Types.None)
        {
            return;
        }

        Calculator.ProcessCalculatorButton(button.Type);
        XamlHelper.CalculateFontSize(_mainInputObject);
    }

    public void MemoryButtonClick(object sender, RoutedEventArgs e)
    {
        var button = (MemoryButton)sender;
        if (button == null)
        {
            return;
        }

        Calculator.ProcessMemoryButton(button.Action);
        XamlHelper.CalculateFontSize(_mainInputObject);
    }

    public void MainInput_Loaded(object sender, RoutedEventArgs e)
    {
        _mainInputObject = (TextBlock)sender;
    }
}
