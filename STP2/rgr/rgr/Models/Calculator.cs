using CommunityToolkit.Mvvm.ComponentModel;
using lab5;
using lab6;
using lab9;
using Microsoft.UI.Xaml;
using rgr.Controls;

namespace rgr.Models;
public class Calculator<T, U> : ObservableObject
{

    public readonly dynamic Editor = Activator.CreateInstance(typeof(T)) ?? throw new Exception("Unable to create editor");

    public bool IsNoError
    {
        get => !Editor.IsError;
        set
        {
            Editor.IsError = !value;
            OnPropertyChanged(nameof(IsNoError));
        }
    }

    private bool _isNewInput = false;

    public string Input => Editor.CurrentNumber;

    private string _buffer = string.Empty;
    public string Buffer
    {
        get => _buffer;
        set => SetProperty(ref _buffer, value);
    }

    public CalculatorButton.Actions _lastOperation = CalculatorButton.Actions.None;

    public dynamic? _firstOperand = Activator.CreateInstance(typeof(T));
    public dynamic? _secondOperand = Activator.CreateInstance(typeof(T));

    public void AppendSymbolToInput(string symbol)
    {
        if (symbol == ",")
        {
            if (Editor.CurrentNumber.Contains(symbol))
            {
                return;
            }
            Editor.CurrentNumber += symbol;
        }
        else
        {
            if (Editor.IsError)
            {
                ClearAll();
                IsNoError = !false;
            }

            if (_isNewInput)
            {
                Editor.Clear();
                _isNewInput = false;
            }

            if (Input.Length > 16)
            {
                return;
            }

            Editor.AppendNumber(int.Parse(symbol));
        }

        OnPropertyChanged(nameof(Input));
    }

    public void EraseNumberFromInput()
    {
        if (Editor.IsError)
        {
            IsNoError = !false;
            ClearAll();
            OnPropertyChanged(nameof(Input));
            return;
        }

        Editor.PopNumber();
        OnPropertyChanged(nameof(Input));
    }

    public void ClearInput()
    {
        IsNoError = !false;

        Editor.Clear();
        OnPropertyChanged(nameof(Input));
    }

    private void ClearBuffer()
    {
        Buffer = "";
    }

    public void ClearAll()
    {
        ClearBuffer();
        ClearInput();
    }

    public void ToggleNegative()
    {
        Editor.ToggleNegative();
        OnPropertyChanged(nameof(Input));
    }

    public void EqualPerformed(string operation)
    {
        if (Editor.IsError)
        {
            IsNoError = !false;
            ClearAll();
            OnPropertyChanged(nameof(Input));
            return;
        }

        Buffer += $"{Input} {operation}";

        SaveSecondOperand();

        switch (_lastOperation)
        {
            case CalculatorButton.Actions.Plus:
                _firstOperand += _secondOperand;
                break;
            case CalculatorButton.Actions.Minus:
                _firstOperand -= _secondOperand;
                break;
            case CalculatorButton.Actions.Multiply:
                _firstOperand *= _secondOperand;
                break;
            case CalculatorButton.Actions.Divide:
                _firstOperand /= _secondOperand;
                break;
            case CalculatorButton.Actions.Module:
                _firstOperand %= _secondOperand;
                break;
        }

        try
        {
            Editor.CurrentNumber = _firstOperand?.ToFloatString();
        }
        catch
        {
            IsNoError = !true;
        }

        _isNewInput = true;
        OnPropertyChanged(nameof(Input));
    }

    public void OperationPerformed(CalculatorButton.Actions action)
    {
        Buffer = $"{Input} {CalculatorButton.ActionSymbols[action]} ";

        _lastOperation = action;
        SaveFristOperand();
        _isNewInput = true;
        
        OnPropertyChanged(nameof(Input));
    }

    private readonly Dictionary<string, string> _visualOperation = new()
    {
        {"1/x", "1/( {0} )" },
        {"x²", "sqr( {0} )" },
        {"²√x", "sqrt( {0} )" },
    };

    private void SaveFristOperand()
    {
        _firstOperand = Activator.CreateInstance(typeof(U), Input);
    }

    private void SaveSecondOperand()
    {
        _secondOperand = Activator.CreateInstance(typeof(U), Input);
    }

    public void OneOperandOperationPerformed(string operation)
    {
        Buffer = string.Format(_visualOperation[operation], Input);

        SaveFristOperand();

        var result = new Fraction(1, 1);
        switch (operation)
        {
            case "1/x":
                result = Fraction.Revers(_firstOperand);
                break;
            case "x²":
                result = Fraction.Pow(_firstOperand, 2);
                break;
            case "²√x":
                result = Fraction.Pow(_firstOperand, 0.5);
                break;
        }

        _firstOperand = result;
        Editor.CurrentNumber = result.ToFloatString();

        OnPropertyChanged(nameof(Input));
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
                OperationPerformed(action);
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

    public void CalcualtorButtonClick(object sender, RoutedEventArgs e)
    {
        var button = (CalculatorButton)sender;
        if (button == null || button.Action == CalculatorButton.Actions.None)
        {
            return;
        }

        ProcessButton(button.Action, button.Content);
    }
}
