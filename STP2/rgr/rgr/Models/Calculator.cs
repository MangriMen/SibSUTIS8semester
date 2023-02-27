using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using lab12;
using lab5;
using lab6;
using Microsoft.UI.Xaml.Controls;
using rgr.Controls;

namespace rgr.Models;
public class Calculator<T, U> : ObservableObject where U : new()
{
    #region Members
    public readonly lab11.Memory<U> Memory = new();

    public readonly dynamic Editor = Activator.CreateInstance(typeof(T)) ?? throw new Exception("Unable to create editor");

    public readonly Processor<U> Processor = new();
    #endregion

    #region Public Fields
    public bool IsMemorySet => Memory.IsOn;
    public bool IsNoError => !Editor.IsError;

    private string _buffer = string.Empty;
    public string Buffer
    {
        get => _buffer;
        set => SetProperty(ref _buffer, value);
    }

    public string Input => Editor.CurrentNumber;
    #endregion

    private bool _isNewInput = false;

    #region Dictionaries
    private readonly Dictionary<CalculatorButton.Types, Processor<U>.Operation> _typesToOperations = new()
    {
        {CalculatorButton.Types.Plus, Processor<U>.Operation.Plus},
        {CalculatorButton.Types.Minus, Processor<U>.Operation.Minus},
        {CalculatorButton.Types.Multiply, Processor<U>.Operation.Multiply},
        {CalculatorButton.Types.Divide, Processor<U>.Operation.Divide},
    };

    private readonly Dictionary<CalculatorButton.Types, Processor<U>.Function> _typesToFunctions = new()
    {
        {CalculatorButton.Types.Module, Processor<U>.Function.Module},
        {CalculatorButton.Types.Reciprocal, Processor<U>.Function.Reciprocal},
        {CalculatorButton.Types.Sqr, Processor<U>.Function.Sqr},
        {CalculatorButton.Types.Sqrt, Processor<U>.Function.Sqrt}
    };

    private readonly Dictionary<Processor<U>.Operation, CalculatorButton.Types> _operationsToTypes = new()
    {
        {Processor<U>.Operation.Plus, CalculatorButton.Types.Plus},
        {Processor<U>.Operation.Minus, CalculatorButton.Types.Minus},
        {Processor<U>.Operation.Multiply, CalculatorButton.Types.Multiply},
        {Processor<U>.Operation.Divide, CalculatorButton.Types.Divide}
    };

    private readonly Dictionary<Processor<U>.Function, CalculatorButton.Types> _functionsToTypes = new()
    {
        {Processor<U>.Function.Module, CalculatorButton.Types.Module},
        {Processor<U>.Function.Reciprocal, CalculatorButton.Types.Reciprocal},
        {Processor<U>.Function.Sqr, CalculatorButton.Types.Sqr},
        {Processor<U>.Function.Sqrt, CalculatorButton.Types.Sqrt}
    };

    private readonly Dictionary<string, string> _visualOperation = new()
    {
        {"1/x", "1/( {0} )" },
        {"x²", "sqr( {0} )" },
        {"²√x", "sqrt( {0} )" },
    };
    #endregion

    public void AppendSymbolToInput(CalculatorButton.Types type)
    {
        if (Input.Length > 16)
        {
            return;
        }

        var toAppend = type switch
        {
            CalculatorButton.Types.ComplexI => "+i*",
            _ => CalculatorButton.ActionSymbols[type]
        };

        switch (type)
        {
            case CalculatorButton.Types.Delimiter:
            case CalculatorButton.Types.ComplexI:
                if (Editor.CurrentNumber.Contains(toAppend))
                {
                    return;
                }
                break;
        }

        if (Editor.IsError)
        {
            ClearAll();
        }

        if (_isNewInput)
        {
            ClearInput();
            _isNewInput = false;
        }

        Editor.AppendNumber(toAppend);
        OnPropertyChanged(nameof(Input));
    }

    public void EraseNumberFromInput()
    {
        if (Editor.IsError)
        {
            ClearAll();
            return;
        }

        Editor.PopNumber();
        OnPropertyChanged(nameof(Input));
    }

    public void ClearInput()
    {
        Editor.IsError = false;
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
            ClearAll();
            return;
        }

        Processor.RightOperand = GetNumberFromInput();

        var firstOperand = string.Empty;
        var secondOperand = string.Empty;
        try
        {
            firstOperand = Processor.LeftOperand?.ToString() ?? "";
            secondOperand = Processor.RightOperand?.ToString() ?? "";
        }
        catch
        {
            Editor.IsError = true;
        }

        Buffer = $"{firstOperand} {CalculatorButton.ActionSymbols[_operationsToTypes[Processor.LastOperation]]} {secondOperand} {CalculatorButton.ActionSymbols[CalculatorButton.Types.Equal]}";

        Processor.PerformOperation();
        try
        {
            Editor.CurrentNumber = Processor.LeftOperand?.ToString();
        }
        catch
        {
            Editor.IsError = true;
        }

        _isNewInput = true;
        OnPropertyChanged(nameof(Input));
    }

    public void OperationPerformed(CalculatorButton.Types type)
    {
        Buffer = $"{Input} {CalculatorButton.ActionSymbols[type]} ";
        Processor.LeftOperand = GetNumberFromInput();
        Processor.LastOperation = _typesToOperations[type];
        _isNewInput = true;
    }

    private dynamic? GetNumberFromInput()
    {
        return Activator.CreateInstance(typeof(U), Input);
    }

    public void OneOperandOperationPerformed(CalculatorButton.Types type)
    {
        var operand = type switch
        {
            CalculatorButton.Types.Module => Input,
            _ => string.Format(_visualOperation[CalculatorButton.ActionSymbols[type]], Input),
        };

        if (Processor.IsOperationDone)
        {
            Processor.LeftOperand = GetNumberFromInput();
            Buffer = operand;
        }
        else
        {
            Processor.RightOperand = GetNumberFromInput();
            Buffer += $"{operand} =";
        }

        Processor.PerformFunction(_typesToFunctions[type]);

        try
        {
            if (Processor.IsOperationDone)
            {
                Editor.CurrentNumber = Processor.LeftOperand?.ToString();
            }
            else
            {
                Editor.CurrentNumber = Processor.RightOperand?.ToString();
            }
        }
        catch
        {
            Editor.IsError = true;
        }

        OnPropertyChanged(nameof(Input));
        _isNewInput = true;
    }

    public void ProcessCalculatorButton(CalculatorButton.Types type)
    {
        try
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
                case CalculatorButton.Types.Delimiter:
                case CalculatorButton.Types.ComplexI:
                    AppendSymbolToInput(type);
                    break;
                case CalculatorButton.Types.Plus:
                case CalculatorButton.Types.Minus:
                case CalculatorButton.Types.Multiply:
                case CalculatorButton.Types.Divide:
                    OperationPerformed(type);
                    break;
                case CalculatorButton.Types.Module:
                case CalculatorButton.Types.Reciprocal:
                case CalculatorButton.Types.Sqr:
                case CalculatorButton.Types.Sqrt:
                    OneOperandOperationPerformed(type);
                    break;
                case CalculatorButton.Types.Equal:
                    EqualPerformed();
                    break;
                case CalculatorButton.Types.ChangeSign:
                    ToggleNegative();
                    break;
                case CalculatorButton.Types.Backspace:
                    EraseNumberFromInput();
                    break;
                case CalculatorButton.Types.Clear:
                    ClearInput();
                    break;
                case CalculatorButton.Types.ClearEntry:
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

    public void ProcessMemoryButton(MemoryButton.Actions type)
    {
        switch (type)
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
