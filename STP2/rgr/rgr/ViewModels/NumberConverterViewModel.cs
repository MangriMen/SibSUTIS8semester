using CommunityToolkit.Mvvm.ComponentModel;
using lab7;
using lab8;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using rgr.Controls;
using rgr.Helpers;

namespace rgr.ViewModels;

public class NumberConverterViewModel : ObservableRecipient
{
    private const int MAX_NUMBER_LENGTH = 10;

    private readonly PNumberEditor Editor = new();
    public string NumberInput => Editor.CurrentNumber;


    public TextBlock? _numberInputObject;

    public TextBlock? _destinationTextObject;


    private PNumber _number = new();

    public readonly int[] Notations = new int[15] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
    public int SourceNotation => Notations[_sourceNotationIndex];


    private int _sourceNotationIndex = 8;
    public int SourceNotationIndex
    {
        get => _sourceNotationIndex;
        set
        {
            SetProperty(ref _sourceNotationIndex, value);
            OnPropertyChanged(nameof(SourceNotation));
            ClearAll();
        }
    }

    private int _destinationNotationIndex = 8;
    public int DestinationNotationIndex
    {
        get => _destinationNotationIndex;
        set
        {
            SetProperty(ref _destinationNotationIndex, value);
            ConvertAndShowNumber();
        }
    }

    
    private string _destinationText = string.Empty;
    public string DestinationText
    {
        get => _destinationText;
        set => SetProperty(ref _destinationText, value);
    }

    public NumberConverterViewModel()
    {
        ClearAll();
    }

    private void ClearAll()
    {
        Editor.Clear();
        OnPropertyChanged(nameof(NumberInput));
        DestinationText = new PNumber().GetConvertedNumber();
    }

    private void ConvertAndShowNumber()
    {
        _number = new(PNumber.ArbitraryToDecimalSystem(Editor.CurrentNumber, SourceNotation), SourceNotation, 0);
        _number.SetBase(Notations[DestinationNotationIndex]);
        DestinationText = _number.GetConvertedNumber().ToUpper();

        XamlHelper.CalculateFontSize(_numberInputObject);
        XamlHelper.CalculateFontSize(_destinationTextObject);
    }

    public void ProcessCalculatorButton(CalculatorButton.Actions action)
    {
        switch (action)
        {
            case CalculatorButton.Actions.Zero:
            case CalculatorButton.Actions.One:
            case CalculatorButton.Actions.Two:
            case CalculatorButton.Actions.Three:
            case CalculatorButton.Actions.Four:
            case CalculatorButton.Actions.Five:
            case CalculatorButton.Actions.Six:
            case CalculatorButton.Actions.Seven:
            case CalculatorButton.Actions.Eight:
            case CalculatorButton.Actions.Nine:
            case CalculatorButton.Actions.Ten:
            case CalculatorButton.Actions.Eleven:
            case CalculatorButton.Actions.Twelve:
            case CalculatorButton.Actions.Thirteen:
            case CalculatorButton.Actions.Fourteen:
            case CalculatorButton.Actions.Fifteen:
                if (Editor.CurrentNumber.Length >= MAX_NUMBER_LENGTH)
                {
                    return;
                }

                Editor.AppendNumber(CalculatorButton.ActionSymbols[action]);
                OnPropertyChanged(nameof(NumberInput));
                ConvertAndShowNumber();
                break;
            case CalculatorButton.Actions.Delimiter:
                if (Editor.CurrentNumber.Length >= MAX_NUMBER_LENGTH)
                {
                    return;
                }

                var symbol = CalculatorButton.ActionSymbols[action];
                if (!Editor.CurrentNumber.Contains(symbol))
                {
                    Editor.CurrentNumber += symbol;
                    OnPropertyChanged(nameof(NumberInput));
                }
                break;
            case CalculatorButton.Actions.Backspace:
                Editor.PopNumber();
                OnPropertyChanged(nameof(NumberInput));
                ConvertAndShowNumber();
                break;
            case CalculatorButton.Actions.ClearEntry:
                ClearAll();
                break;
        }
    }

    public void CalculatorButtonClick(object sender, RoutedEventArgs e)
    {
        var button = (CalculatorButton)sender;
        if (button == null || button.Action == CalculatorButton.Actions.None)
        {
            return;
        }

        ProcessCalculatorButton(button.Action);
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
