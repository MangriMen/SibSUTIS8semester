using CommunityToolkit.Mvvm.ComponentModel;
using rgr.Controls;
using Types;
using Editors;

namespace rgr.Models;

public class Converter : ObservableObject
{
    private const int MAX_NUMBER_LENGTH = 10;

    private const int DEFAULT_NOTATION_INDEX = 8;

    private static readonly int[] _notations = new int[15]
    {
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13,
        14,
        15,
        16
    };

    private readonly PNumberEditor Editor = new();

    private PNumber _number = new();

    private bool _isNewInput = false;

    private int _sourceNotationIndex = DEFAULT_NOTATION_INDEX;
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

    private int _destinationNotationIndex = DEFAULT_NOTATION_INDEX;
    public int DestinationNotationIndex
    {
        get => _destinationNotationIndex;
        set
        {
            SetProperty(ref _destinationNotationIndex, value);
            ConvertNumber();
        }
    }

    private string _destinationText = string.Empty;
    public string DestinationText
    {
        get => _destinationText;
        set => SetProperty(ref _destinationText, value);
    }

    public int[] Notations => _notations;
    public int SourceNotation => _notations[_sourceNotationIndex];
    public string NumberInput => Editor.CurrentNumber;

    private void AppendSymbolToInput(CalculatorButton.Types type)
    {
        if (Editor.CurrentNumber.Length >= MAX_NUMBER_LENGTH)
        {
            return;
        }

        var symbol = CalculatorButton.ActionSymbols[type];

        switch (type)
        {
            case CalculatorButton.Types.Delimiter:
                if (Editor.CurrentNumber.Contains(symbol))
                {
                    return;
                }
                break;
        }

        if (_isNewInput)
        {
            ClearAll();
            _isNewInput = false;
        }

        Editor.AppendNumber(symbol);
        OnPropertyChanged(nameof(NumberInput));
    }

    private void EraseNumberFromInput()
    {
        Editor.PopNumber();
        OnPropertyChanged(nameof(NumberInput));
    }

    private void ClearAll()
    {
        _isNewInput = true;
        Editor.Clear();
        OnPropertyChanged(nameof(NumberInput));
        DestinationText = new PNumber().GetConvertedNumber();
    }

    private void ConvertNumber()
    {
        _number = new(
            PNumber.ArbitraryToDecimalSystem(Editor.CurrentNumber, SourceNotation),
            SourceNotation,
            0
        );
        _number.SetBase(_notations[DestinationNotationIndex]);
        DestinationText = _number.GetConvertedNumber().ToUpper();
    }

    public void ProcessCalculatorButton(CalculatorButton.Types type)
    {
        switch (type)
        {
            case CalculatorButton.Types.Zero:
            case CalculatorButton.Types.One:
            case CalculatorButton.Types.Two:
            case CalculatorButton.Types.Three:
            case CalculatorButton.Types.Four:
            case CalculatorButton.Types.Five:
            case CalculatorButton.Types.Six:
            case CalculatorButton.Types.Seven:
            case CalculatorButton.Types.Eight:
            case CalculatorButton.Types.Nine:
            case CalculatorButton.Types.Ten:
            case CalculatorButton.Types.Eleven:
            case CalculatorButton.Types.Twelve:
            case CalculatorButton.Types.Thirteen:
            case CalculatorButton.Types.Fourteen:
            case CalculatorButton.Types.Fifteen:
            case CalculatorButton.Types.Delimiter:
                AppendSymbolToInput(type);
                break;
            case CalculatorButton.Types.Backspace:
                EraseNumberFromInput();
                break;
            case CalculatorButton.Types.ClearEntry:
                ClearAll();
                break;
        }
        ConvertNumber();
    }
}
