using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using lab5;
using lab6;
using lab9;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
        if (symbol == "," || symbol == "i")
        {
            if (Editor.CurrentNumber.Contains(symbol))
            {
                return;
            }
            if (symbol == "i")
            {
                Editor.AppendNumber("+i*");
            }
            else
            {
                Editor.CurrentNumber += symbol;
            }
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

            Editor.AppendNumber(symbol);
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
            firstOperand = _firstOperand?.ToString() ?? "";
            secondOperand = _secondOperand?.ToString() ?? "";
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
            Editor.CurrentNumber = _firstOperand?.ToString();
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

        var methodPow = typeof(U).GetMethod("Pow");

        System.Reflection.MethodInfo? methodSqrt = null;
        object[]? sqrtArguments = null;

        dynamic? result = _firstOperand;

        if (!_isOperationDone)
        {
            result = _secondOperand;
        }

        if (result == null)
        {
            return;
        }

        dynamic? oneHundred = null;

        if (typeof(U) == typeof(Fraction))
        {
            sqrtArguments = new object[] { result, 0.5 };
            methodSqrt = typeof(U).GetMethod("Pow");
            oneHundred = Activator.CreateInstance(typeof(U), "100/1");
        }
        else if (typeof(U) == typeof(Complex))
        {
            sqrtArguments = new object[] { result, 2, 0 };
            methodSqrt = typeof(U).GetMethod("Root");
            oneHundred = Activator.CreateInstance(typeof(U), "100");
        }

        switch (action)
        {
            case CalculatorButton.Actions.Module:
                result /= oneHundred;
                break;
            case CalculatorButton.Actions.Reciprocal:
                result = Fraction.Reverse(result);
                break;
            case CalculatorButton.Actions.Sqr:
                result = methodPow?.Invoke(null, new object?[] { result, 2 });
                break;
            case CalculatorButton.Actions.Sqrt:
                result = methodSqrt?.Invoke(null, sqrtArguments);
                break;
        }

        try
        {
            if (_isOperationDone)
            {
                _firstOperand = result;
                Editor.CurrentNumber = _firstOperand?.ToString();
            }
            else
            {
                _secondOperand = result;
                Editor.CurrentNumber = _secondOperand?.ToString();
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
        try
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
                case CalculatorButton.Actions.ComplexI:
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
        catch
        {
            try
            {
                _ = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Неизвестная ошибка",
                    CloseButtonText = "Ок",
                    XamlRoot = App.MainWindow.Content.XamlRoot,
                }.ShowAsync();
            }
            catch
            {

            }
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
                    Editor.CurrentNumber = ((dynamic?)Memory.Read())?.ToString();
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
