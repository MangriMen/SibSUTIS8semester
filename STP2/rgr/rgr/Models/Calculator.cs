using CommunityToolkit.Mvvm.ComponentModel;
using rgr.Controls;
using Types;
using Editors;
using Calculator;
using Microsoft.UI.Xaml.Controls;

namespace rgr.Models;

public class Calculator<T, U> : ObservableObject
    where T : Editor, new()
    where U : Number, new()
{
    private const int MAX_NUMBER_LENGTH = 16;

    private static readonly Dictionary<
        CalculatorButton.Types,
        Processor.Operation
    > _typesToOperations =
        new()
        {
            { CalculatorButton.Types.Plus, Calculator.Processor.Operation.Plus },
            { CalculatorButton.Types.Minus, Calculator.Processor.Operation.Minus },
            { CalculatorButton.Types.Multiply, Calculator.Processor.Operation.Multiply },
            { CalculatorButton.Types.Divide, Calculator.Processor.Operation.Divide },
        };

    private static readonly Dictionary<
        CalculatorButton.Types,
        Processor.Function
    > _typesToFunctions =
        new()
        {
            { CalculatorButton.Types.Module, Calculator.Processor.Function.Module },
            { CalculatorButton.Types.Reciprocal, Calculator.Processor.Function.Reciprocal },
            { CalculatorButton.Types.Sqr, Calculator.Processor.Function.Sqr },
            { CalculatorButton.Types.Sqrt, Calculator.Processor.Function.Sqrt }
        };

    private static readonly Dictionary<
        Processor.Operation,
        CalculatorButton.Types
    > _operationsToTypes =
        new()
        {
            { Calculator.Processor.Operation.Plus, CalculatorButton.Types.Plus },
            { Calculator.Processor.Operation.Minus, CalculatorButton.Types.Minus },
            { Calculator.Processor.Operation.Multiply, CalculatorButton.Types.Multiply },
            { Calculator.Processor.Operation.Divide, CalculatorButton.Types.Divide }
        };

    private static readonly Dictionary<CalculatorButton.Types, string> _functionFromatStrings =
        new()
        {
            { CalculatorButton.Types.Module, "{0}" },
            { CalculatorButton.Types.Reciprocal, "1/( {0} )" },
            { CalculatorButton.Types.Sqr, "sqr( {0} )" },
            { CalculatorButton.Types.Sqrt, "sqrt( {0} )" },
        };

    private readonly T Editor = new();

    private readonly Processor<U> Processor = new();

    private readonly Calculator.Memory<U> Memory = new();

    private bool _isNewInput = false;

    private string _buffer = string.Empty;
    public string Buffer
    {
        get => _buffer;
        set => SetProperty(ref _buffer, value);
    }

    public bool IsMemorySet => Memory.IsOn;
    public bool IsNoError => !false;
    public string Input => Editor.Number != string.Empty ? Editor.Number : Editor.Zero;

    private int _selectedNotationIndex = 0;
    public int SelectedNotationIndex
    {
        get => _selectedNotationIndex;
        set
        {
            SetProperty(ref _selectedNotationIndex, value);
            ClearAll();
            OnPropertyChanged(nameof(SelectedNotation));
        }
    }

    public int SelectedNotation => Constants.Notations[SelectedNotationIndex];

    private void SetNumberToInput(U number)
    {
        number.Base = SelectedNotation;
        try
        {
            Editor.Number = number.ToString();
        }
        catch
        {
            OnPropertyChanged(nameof(IsNoError));
        }

        OnPropertyChanged(nameof(Input));
    }

    private U GetNumberFromInput()
    {
        var number = new U();
        number.FromString(Input, SelectedNotation);
        number.Base = SelectedNotation;
        return number;
    }

    private (string, string) GetOperands()
    {
        try
        {
            var leftOperandStr = Processor.LeftOperand?.ToString();
            var rightOperandStr = Processor.RightOperand?.ToString();

            if (string.IsNullOrEmpty(leftOperandStr) || string.IsNullOrEmpty(rightOperandStr))
            {
                throw new ArgumentNullException("One of operands is null");
            }

            return (
                Processor.LeftOperand?.ToString() ?? string.Empty,
                Processor.RightOperand?.ToString() ?? string.Empty
            );
        }
        catch
        {
            OnPropertyChanged(nameof(IsNoError));
        }

        return (string.Empty, string.Empty);
    }

    public void AppendSymbolToInput(CalculatorButton.Types type)
    {
        if (_isNewInput)
        {
            ClearInput();
            _isNewInput = false;
        }

        if (Input.Length > MAX_NUMBER_LENGTH)
        {
            return;
        }

        Editor.AddDigit(CalculatorButton.ActionSymbols[type]);
        OnPropertyChanged(nameof(Input));
    }

    public void EraseNumberFromInput()
    {
        Editor.Backspace();
        OnPropertyChanged(nameof(Input));
    }

    public void ClearInput()
    {
        Editor.Clear();
        OnPropertyChanged(nameof(IsNoError));
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
        Processor.RightOperand = GetNumberFromInput();
        var (firstOperand, secondOperand) = GetOperands();
        var equalSign = CalculatorButton.ActionSymbols[CalculatorButton.Types.Equal];

        if (Processor.LastOperation == Calculator.Processor.Operation.None)
        {
            Processor.LeftOperand = GetNumberFromInput();
            Buffer = string.Join(' ', Processor.LeftOperand, equalSign);
            return;
        }

        var operation = CalculatorButton.ActionSymbols[_operationsToTypes[Processor.LastOperation]];

        if (firstOperand == string.Empty || secondOperand == string.Empty)
        {
            return;
        }

        Buffer = string.Join(' ', firstOperand, operation, secondOperand, equalSign);

        Processor.PerformOperation();

        SetNumberToInput(Processor.LeftOperand);

        _isNewInput = true;
    }

    public void OperationPerformed(CalculatorButton.Types type)
    {
        if (!Processor.IsOperationDone)
        {
            EqualPerformed();
        }

        Processor.LeftOperand = GetNumberFromInput();
        Processor.LastOperation = _typesToOperations[type];

        Buffer = $"{Processor.LeftOperand} {CalculatorButton.ActionSymbols[type]} ";
        _isNewInput = true;
    }

    public void OneOperandOperationPerformed(CalculatorButton.Types type)
    {
        var operand = string.Format(_functionFromatStrings[type], Input);

        (Processor.LeftOperand, Processor.RightOperand, Buffer) = Processor.IsOperationDone switch
        {
            true => (GetNumberFromInput(), Processor.RightOperand, operand),
            false => (Processor.LeftOperand, GetNumberFromInput(), $"{Buffer}{operand}=")
        };

        Processor.PerformFunction(_typesToFunctions[type]);

        SetNumberToInput(
            Processor.IsOperationDone switch
            {
                true => Processor.LeftOperand,
                false => Processor.RightOperand,
            }
        );

        _isNewInput = true;
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
                AppendSymbolToInput(type);
                break;
            case CalculatorButton.Types.Delimiter:
                Editor.Separator = ",";
                Editor.AddSeparator();
                OnPropertyChanged(nameof(Input));
                break;
            case CalculatorButton.Types.Slash:
                Editor.AddSeparator();
                OnPropertyChanged(nameof(Input));
                break;
            case CalculatorButton.Types.ComplexI:
                Editor.AddSeparator();
                OnPropertyChanged(nameof(Input));
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

    public void ProcessMemoryButton(Memory.Actions action)
    {
        switch (action)
        {
            case Calculator.Memory.Actions.Store:
                Memory.Store(GetNumberFromInput());
                break;
            case Calculator.Memory.Actions.Read:
                SetNumberToInput(Memory.Read());
                break;
            case Calculator.Memory.Actions.Add:
                Memory.Add(GetNumberFromInput());
                break;
            case Calculator.Memory.Actions.Subtract:
                Memory.Subtract(GetNumberFromInput());
                break;
            case Calculator.Memory.Actions.Clear:
                Memory.Clear();
                break;
        }
        OnPropertyChanged(nameof(IsMemorySet));
    }
}
