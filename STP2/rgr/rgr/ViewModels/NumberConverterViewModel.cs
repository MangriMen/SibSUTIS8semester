using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using rgr.Controls;
using rgr.Helpers;
using rgr.Models;

namespace rgr.ViewModels;

public class NumberConverterViewModel : ObservableRecipient
{
    public readonly Converter Converter = new();

    private TextBlock? _numberInputObject;

    private TextBlock? _destinationTextObject;

    public NumberConverterViewModel() { }

    public void CalculatorButtonClick(object sender, RoutedEventArgs e)
    {
        var button = (CalculatorButton)sender;
        if (button == null || button.Type == CalculatorButton.Types.None)
        {
            return;
        }

        Converter.ProcessCalculatorButton(button.Type);
        XamlHelper.CalculateFontSize(_numberInputObject);
        XamlHelper.CalculateFontSize(_destinationTextObject);
    }

    public void NumberInputLoaded(object sender, RoutedEventArgs e)
    {
        _numberInputObject = (TextBlock)sender;
    }

    public void DestinationTextLoaded(object sender, RoutedEventArgs e)
    {
        _destinationTextObject = (TextBlock)sender;
    }
}
