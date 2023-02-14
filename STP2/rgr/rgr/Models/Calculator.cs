using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using lab5;
using lab6;
using lab9;
using Microsoft.UI.Xaml;
using rgr.Controls;

namespace rgr.Models;
public class Calculator<T, U> : ObservableObject where U : new()
{

    public readonly lab11.Memory<U> Memory = new();

    public readonly dynamic Editor = Activator.CreateInstance(typeof(T)) ?? throw new Exception("Unable to create editor");

    public bool IsMemorySet => Memory.IsOn;

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
    private bool _isOperationDone = true;

    public string Input => Editor.CurrentNumber;

    private string _buffer = string.Empty;
    public string Buffer
    {
        get => _buffer;
        set => SetProperty(ref _buffer, value);
    }

    public CalculatorButton.Actions _lastOperation = CalculatorButton.Actions.None;

    public dynamic? _firstOperand = Activator.CreateInstance(typeof(U));
    public dynamic? _secondOperand = Activator.CreateInstance(typeof(U));

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

    public void EqualPerformed()
    {
        if (Editor.IsError)
        {
            IsNoError = !false;
            ClearAll();
            OnPropertyChanged(nameof(Input));
            return;
        }

        SaveSecondOperand();

        var firstOperand = string.Empty;
        var secondOperand = string.Empty;
        try
        {
            firstOperand = _firstOperand?.ToFloatString() ?? "";
            secondOperand = _secondOperand?.ToFloatString() ?? "";
        }
        catch
        {
            IsNoError = !true;
        }

        Buffer = $"{firstOperand} {CalculatorButton.ActionSymbols[_lastOperation]} {secondOperand} {CalculatorButton.ActionSymbols[CalculatorButton.Actions.Equal]}";

        CalculateEqual();
    }

    public void CalculateEqual()
    {
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
        _isOperationDone = false;

        OnPropertyChanged(nameof(Input));
    }

    private readonly Dictionary<string, string> _visualOperation = new()
    {
        {"1/x", "1/( {0} )" },
        {"x²", "sqr( {0} )" },
        {"²√x", "sqrt( {0} )" },
    };

    private dynamic? GetNumberFromInput()
    {
        return Activator.CreateInstance(typeof(U), Input);
    }

    private void SaveFristOperand()
    {
        _firstOperand = Activator.CreateInstance(typeof(U), Input);
    }

    private void SaveSecondOperand()
    {
        _secondOperand = Activator.CreateInstance(typeof(U), Input);
    }

    public void OneOperandOperationPerformed(CalculatorButton.Actions action)
    {
        var operand = action switch
        {
            CalculatorButton.Actions.Module => Input,
            _ => string.Format(_visualOperation[CalculatorButton.ActionSymbols[action]], Input),
        };

        if (_isOperationDone)
        {
            SaveFristOperand();
            Buffer = operand;
        }
        else
        {
            SaveSecondOperand();
            Buffer += $"{operand} =";
        }

        switch (action)
        {
            case CalculatorButton.Actions.Module:
                dynamic? oneHundred = Activator.CreateInstance(typeof(U), "100/1");
                if (!_isOperationDone)
                {
                    _secondOperand /= oneHundred;
                }
                else
                {
                    _firstOperand /= oneHundred;
                }
                break;
            case CalculatorButton.Actions.Reciprocal:
                if (!_isOperationDone)
                {
                    _secondOperand = Fraction.Revers(_secondOperand);
                }
                else
                {
                    _firstOperand = Fraction.Revers(_firstOperand);
                }
                break;
            case CalculatorButton.Actions.Sqr:
                if (!_isOperationDone)
                {
                    _secondOperand = Fraction.Pow(_secondOperand, 2);
                }
                else
                {
                    _firstOperand = Fraction.Pow(_firstOperand, 2);
                }
                break;
            case CalculatorButton.Actions.Sqrt:
                if (!_isOperationDone)
                {
                    _secondOperand = Fraction.Pow(_secondOperand, 0.5);
                }
                else
                {
                    _firstOperand = Fraction.Pow(_firstOperand, 0.5);
                }
                break;
        }

        try
        {
            if (_isOperationDone)
            {
                Editor.CurrentNumber = _firstOperand?.ToFloatString();
            }
            else
            {
                Editor.CurrentNumber = _secondOperand?.ToFloatString();
            }
        }
        catch
        {
            IsNoError = !true;
        }

        OnPropertyChanged(nameof(Input));
        _isNewInput = true;
    }

    public void ProcessCalculatorButton(CalculatorButton.Actions action, string content)
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
            case CalculatorButton.Actions.Delimiter:
                AppendSymbolToInput(content);
                break;
            case CalculatorButton.Actions.Plus:
            case CalculatorButton.Actions.Minus:
            case CalculatorButton.Actions.Multiply:
            case CalculatorButton.Actions.Divide:
                OperationPerformed(action);
                break;
            case CalculatorButton.Actions.Module:
            case CalculatorButton.Actions.Reciprocal:
            case CalculatorButton.Actions.Sqr:
            case CalculatorButton.Actions.Sqrt:
                OneOperandOperationPerformed(action);
                break;
            case CalculatorButton.Actions.Equal:
                EqualPerformed();
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
        }
    }

    public void ProcessMemoryButton(MemoryButton.Actions action, string content)
    {
        switch (action)
        {
            case MemoryButton.Actions.Clear:
                Memory.Clear();
                OnPropertyChanged(nameof(IsMemorySet));
                break;
            case MemoryButton.Actions.Read:
                try
                {
                    Editor.CurrentNumber = ((dynamic?)Memory.Read())?.ToFloatString();
                    OnPropertyChanged(nameof(Input));
                }
                catch
                {
                    return;
                }
                break;
            case MemoryButton.Actions.Add:
                Memory.Add(GetNumberFromInput());
                OnPropertyChanged(nameof(IsMemorySet));
                break;
            case MemoryButton.Actions.Subtract:
                Memory.Subtract(GetNumberFromInput());
                OnPropertyChanged(nameof(IsMemorySet));
                break;
            case MemoryButton.Actions.Store:
                Memory.Storage(GetNumberFromInput());
                OnPropertyChanged(nameof(IsMemorySet));
                break;
        }
    }
}
