using CommunityToolkit.Mvvm.ComponentModel;
using lab5;
using lab9;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using rgr.Controls;

namespace rgr.ViewModels;

public class StandardViewModel : ObservableRecipient
{
    public readonly FractionEditor FractionEditor = new();

    private bool _isNewInput = false;

    public string MainInput => FractionEditor.CurrentNumber;

    public bool IsNoError
    {
        get => !FractionEditor.IsError;
        set
        {
            FractionEditor.IsError = !value;
            OnPropertyChanged(nameof(IsNoError));
        }
    }

    private string _buffer = string.Empty;
    public string Buffer
    {
        get => _buffer;
        set => SetProperty(ref _buffer, value);
    }

    public SimpleFraction _firstOperand = new(1, 1);
    public SimpleFraction _secondOperand = new(1, 1);
    public string _lastOperation = string.Empty;

    public TextBlock? _mainInputObject;
    public TextBlock? MainInputObject
    {
        get => _mainInputObject;
        set => SetProperty(ref _mainInputObject, value);
    }

    public StandardViewModel()
    {
    }

    public void AppendSymbolToInput(string symbol)
    {
        if (symbol == ",")
        {
            if (FractionEditor.CurrentNumber.Contains(symbol))
            {
                return;
            }
            FractionEditor.CurrentNumber += symbol;
        }
        else
        {
            if (FractionEditor.IsError)
            {
                ClearAll();
                IsNoError = !false;
            }

            if (_isNewInput)
            {
                FractionEditor.Clear();
                _isNewInput = false;
            }

            if (MainInput.Length > 16)
            {
                return;
            }

            FractionEditor.AppendNumber(int.Parse(symbol));
        }

        OnPropertyChanged(nameof(MainInput));
        CalculateFontSize();
    }

    public void EraseNumberFromInput()
    {
        if (FractionEditor.IsError)
        {
            IsNoError = !false;
            ClearAll();
            OnPropertyChanged(nameof(MainInput));
            return;
        }

        FractionEditor.PopNumber();
        OnPropertyChanged(nameof(MainInput));
        CalculateFontSize();
    }

    public void ClearInput()
    {
        IsNoError = !false;

        FractionEditor.Clear();
        OnPropertyChanged(nameof(MainInput));
    }

    private void ClearBuffer()
    {
        Buffer = "";
    }

    public void ClearAll()
    {
        ClearBuffer();
        ClearInput();
        CalculateFontSize();
    }

    public void ToggleNegative()
    {
        FractionEditor.ToggleNegative();
        OnPropertyChanged(nameof(MainInput));
    }

    public void EqualPerformed(string operation)
    {
        if (FractionEditor.IsError)
        {
            IsNoError = !false;
            ClearAll();
            OnPropertyChanged(nameof(MainInput));
        }

        Buffer += $"{MainInput} {operation}";

        _secondOperand = new(MainInput);

        var result = new SimpleFraction(1, 1);
        switch (_lastOperation)
        {
            case "+":
                result = _firstOperand + _secondOperand;
                break;
            case "-":
                result = _firstOperand - _secondOperand;
                break;
            case "×":
                result = _firstOperand * _secondOperand;
                break;
            case "÷":
                result = _firstOperand / _secondOperand;
                break;
        }

        _firstOperand = result;

        try
        {
            FractionEditor.CurrentNumber = result.ToFloatString();
        }
        catch
        {
            IsNoError = !true;
        }

        OnPropertyChanged(nameof(MainInput));
        _isNewInput = true;
    }

    public void OperationPerformed(string operation)
    {
        Buffer = $"{MainInput} {operation} ";

        _lastOperation = operation;

        _firstOperand = new(MainInput);

        OnPropertyChanged(nameof(MainInput));
        _isNewInput = true;
    }

    private readonly Dictionary<string, string> _visualOperation = new()
    {
        {"1/x", "1/( {0} )" },
        {"x²", "sqr( {0} )" },
        {"²√x", "sqrt( {0} )" },
    };

    public void OneOperandOperationPerformed(string operation)
    {
        Buffer = string.Format(_visualOperation[operation], MainInput);

        _firstOperand = new(MainInput);
        var result = new SimpleFraction(1, 1);
        switch (operation)
        {
            case "1/x":
                result = SimpleFraction.Revers(_firstOperand);
                break;
            case "x²":
                result = SimpleFraction.Pow(_firstOperand, 2);
                break;
            case "²√x":
                result = SimpleFraction.Pow(_firstOperand, 0.5);
                break;
        }

        _firstOperand = result;
        FractionEditor.CurrentNumber = result.ToFloatString();

        OnPropertyChanged(nameof(MainInput));
        _isNewInput = true;
    }

    public void ProcessButton(CalculatorButton.Actions action, string content)
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
                AppendSymbolToInput(content);
                break;
            case CalculatorButton.Actions.Plus:
            case CalculatorButton.Actions.Minus:
            case CalculatorButton.Actions.Multiply:
            case CalculatorButton.Actions.Divide:
            case CalculatorButton.Actions.Module:
                OperationPerformed(content);
                break;
            case CalculatorButton.Actions.Reciprocal:
            case CalculatorButton.Actions.Sqr:
            case CalculatorButton.Actions.Sqrt:
                OneOperandOperationPerformed(content);
                break;
            case CalculatorButton.Actions.Equal:
                EqualPerformed(content);
                break;
            case CalculatorButton.Actions.ChangeSign:
                ToggleNegative();
                break;
            case CalculatorButton.Actions.Backspace:
                EraseNumberFromInput();
                break;
            case CalculatorButton.Actions.Clear:
                ClearInput();
                break;
            case CalculatorButton.Actions.ClearEntry:
                ClearAll();
                break;
            case CalculatorButton.Actions.Delimiter:
                AppendSymbolToInput(content);
                break;
        }
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

    public void CalcualtorButtonClick(object sender, RoutedEventArgs e)
    {
        var button = (CalculatorButton)sender;
        if (button == null || button.Action == CalculatorButton.Actions.None)
        {
            return;
        }

        ProcessButton(button.Action, button.Content);
    }

    public void MainInput_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        MainInputObject = (TextBlock)sender;
    }
}
