using CommunityToolkit.Mvvm.ComponentModel;
using rgr.Controls;
using Types;
using Editors;

namespace rgr.Models;

public class Converter : ObservableObject
{
    private const int MAX_NUMBER_LENGTH = 10;

    private readonly PNumberEditor Editor = new();

    private PNumber _number = new();

    private bool _isNewInput = false;

    private int _sourceNotationIndex = Constants.DEFAULT_NOTATION_INDEX;
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

    private int _destinationNotationIndex = Constants.DEFAULT_NOTATION_INDEX;
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

    public int[] Notations => Constants.Notations;
    public int SourceNotation => Notations[_sourceNotationIndex];
    public string NumberInput =>
        Editor.Number != string.Empty ? Editor.Number : FractionEditor.ZERO;

    public Converter()
    {
        ConvertNumber();
    }

    private void AppendSymbolToInput(CalculatorButton.Types type)
    {
        if (Editor.Number.Length >= MAX_NUMBER_LENGTH)
        {
            return;
        }

        switch (type)
        {
            case CalculatorButton.Types.Delimiter:
                Editor.AddSeparator();
                return;
        }

        if (_isNewInput)
        {
            ClearAll();
            _isNewInput = false;
        }

        Editor.AddDigit(CalculatorButton.ActionSymbols[type]);
        OnPropertyChanged(nameof(NumberInput));
    }

    private void EraseNumberFromInput()
    {
        Editor.Backspace();
        OnPropertyChanged(nameof(NumberInput));
    }

    private void ClearAll()
    {
        _isNewInput = true;
        Editor.Clear();
        OnPropertyChanged(nameof(NumberInput));
        DestinationText = new PNumber().ToString();
    }

    private void ConvertNumber()
    {
        _number = new(
            PHelper.ArbitraryToDecimalSystem(Editor.Number, SourceNotation),
            Notations[DestinationNotationIndex]
        );
        DestinationText = _number.ToString().ToUpper();
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
