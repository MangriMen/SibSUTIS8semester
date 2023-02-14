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
    public readonly PNumberEditor Editor = new();

    public string SourceInput => Editor.CurrentNumber;

    private string _destinationText = "0";
    public string DestinationText
    {
        get => _destinationText;
        set => SetProperty(ref _destinationText, value);
    }

    public readonly short[] Notations = new short[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

    public int SourceNotation => Notations[_sourceNotationIndex];

    private int _sourceNotationIndex = 8;
    public int SourceNotationIndex
    {
        get => _sourceNotationIndex;
        set
        {
            SetProperty(ref _sourceNotationIndex, value);

            Editor.Clear();
            OnPropertyChanged(nameof(SourceInput));

            OnPropertyChanged(nameof(SourceNotation));

            DestinationText = "0";
        }
    }

    private int _destinationNotationIndex = 8;
    public int DestinationNotationIndex
    {
        get => _destinationNotationIndex;
        set
        {
            Editor.Clear();
            OnPropertyChanged(nameof(SourceInput));
            SetProperty(ref _destinationNotationIndex, value);

            OnPropertyChanged(nameof(SourceNotation));

            DestinationText = "0";
        }
    }

    private PNumber _firstOperand = new();

    public NumberConverterViewModel()
    {

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
                Editor.AppendNumber(CalculatorButton.ActionSymbols[action]);
                OnPropertyChanged(nameof(SourceInput));

                _firstOperand = new(Convert.ToInt32(Editor.CurrentNumber, SourceNotation), SourceNotation, 0);
                _firstOperand.SetBase(Notations[DestinationNotationIndex]);
                DestinationText = _firstOperand.GetNumber();
                OnPropertyChanged(nameof(DestinationText));
                break;
            case CalculatorButton.Actions.Delimiter:
                var symbol = CalculatorButton.ActionSymbols[action];
                if (!Editor.CurrentNumber.Contains(symbol))
                {
                    Editor.CurrentNumber += symbol;
                    OnPropertyChanged(nameof(SourceInput));
                }
                break;
            case CalculatorButton.Actions.Backspace:
                Editor.PopNumber();
                OnPropertyChanged(nameof(SourceInput));

                _firstOperand = new(Convert.ToInt32(Editor.CurrentNumber, SourceNotation), SourceNotation, 0);
                _firstOperand.SetBase(Notations[DestinationNotationIndex]);
                DestinationText = _firstOperand.GetNumber();
                OnPropertyChanged(nameof(DestinationText));
                break;
            case CalculatorButton.Actions.ClearEntry:
                Editor.Clear();
                OnPropertyChanged(nameof(SourceInput));

                _firstOperand = new(Convert.ToInt32(Editor.CurrentNumber, SourceNotation), SourceNotation, 0);
                _firstOperand.SetBase(Notations[DestinationNotationIndex]);
                DestinationText = _firstOperand.GetNumber();
                OnPropertyChanged(nameof(DestinationText));
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
}
