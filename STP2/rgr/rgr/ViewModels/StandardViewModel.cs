using CommunityToolkit.Mvvm.ComponentModel;
using lab5;
using lab9;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using rgr.Controls;
using rgr.Models;

namespace rgr.ViewModels;

public class StandardViewModel : ObservableRecipient
{
    public readonly Calculator<FractionEditor, Fraction> Calculator = new();

    public TextBlock? _mainInputObject;
    public TextBlock? MainInputObject
    {
        get => _mainInputObject;
        set => SetProperty(ref _mainInputObject, value);
    }

    public StandardViewModel()
    {
    }

    public void CalculatorButtonClick(object sender, RoutedEventArgs e)
    {
        var button = (CalculatorButton)sender;
        if (button == null || button.Action == CalculatorButton.Actions.None)
        {
            return;
        }

        Calculator.ProcessCalculatorButton(button.Action, button.Content);
        CalculateFontSize();
    }

    public void MemoryButtonClick(object sender, RoutedEventArgs e)
    {
        var button = (MemoryButton)sender;
        if (button == null || button.Action == MemoryButton.Actions.None)
        {
            return;
        }

        Calculator.ProcessMemoryButton(button.Action, button.Content);
        CalculateFontSize();
    }

    public void CalculateFontSize()
    {
        if (MainInputObject == null)
        {
            return;
        }

        var desiredHeight = 60;

        var fontsizeMultiplier = Math.Sqrt(desiredHeight / MainInputObject.ActualHeight);

        MainInputObject.FontSize = Math.Floor(MainInputObject.FontSize * fontsizeMultiplier);
    }

    public void MainInput_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        MainInputObject = (TextBlock)sender;
    }
}
