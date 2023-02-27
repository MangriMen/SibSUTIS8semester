using CommunityToolkit.Mvvm.ComponentModel;
using lab11;
using lab12;
using rgr.Controls;

namespace rgr.Models;

public class Calculator<T, U> : ObservableObject
    where U : new()
{
    private const int MAX_NUMBER_LENGTH = 16;

    private static readonly Dictionary<
        CalculatorButton.Types,
        Processor.Operation
    > _typesToOperations =
        new()
        {
            { CalculatorButton.Types.Plus, lab12.Processor.Operation.Plus },
            { CalculatorButton.Types.Minus, lab12.Processor.Operation.Minus },
            { CalculatorButton.Types.Multiply, lab12.Processor.Operation.Multiply },
            { CalculatorButton.Types.Divide, lab12.Processor.Operation.Divide },
        };

    private static readonly Dictionary<
        CalculatorButton.Types,
        Processor.Function
    > _typesToFunctions =
        new()
        {
            { CalculatorButton.Types.Module, lab12.Processor.Function.Module },
            { CalculatorButton.Types.Reciprocal, lab12.Processor.Function.Reciprocal },
            { CalculatorButton.Types.Sqr, lab12.Processor.Function.Sqr },
            { CalculatorButton.Types.Sqrt, lab12.Processor.Function.Sqrt }
        };

    private static readonly Dictionary<
        Processor.Operation,
        CalculatorButton.Types
    > _operationsToTypes =
        new()
        {
            { lab12.Processor.Operation.Plus, CalculatorButton.Types.Plus },
            { lab12.Processor.Operation.Minus, CalculatorButton.Types.Minus },
            { lab12.Processor.Operation.Multiply, CalculatorButton.Types.Multiply },
            { lab12.Processor.Operation.Divide, CalculatorButton.Types.Divide }
        };

    private static readonly Dictionary<
        Processor.Function,
        CalculatorButton.Types
    > _functionsToTypes =
        new()
        {
            { lab12.Processor.Function.Module, CalculatorButton.Types.Module },
            { lab12.Processor.Function.Reciprocal, CalculatorButton.Types.Reciprocal },
            { lab12.Processor.Function.Sqr, CalculatorButton.Types.Sqr },
            { lab12.Processor.Function.Sqrt, CalculatorButton.Types.Sqrt }
        };

    private static readonly Dictionary<CalculatorButton.Types, string> _functionFromatStrings =
        new()
        {
            { CalculatorButton.Types.Module, "{0}" },
            { CalculatorButton.Types.Reciprocal, "1/( {0} )" },
            { CalculatorButton.Types.Sqr, "sqr( {0} )" },
            { CalculatorButton.Types.Sqrt, "sqrt( {0} )" },
        };

    private readonly lab11.Memory<U> Memory = new();

    private readonly dynamic Editor =
        Activator.CreateInstance(typeof(T)) ?? throw new Exception("Unable to create editor");

    private readonly Processor<U> Processor = new();

    private bool _isNewInput = false;

    private string _buffer = string.Empty;
    public string Buffer
    {
        get => _buffer;
        set => SetProperty(ref _buffer, value);
    }

    public bool IsMemorySet => Memory.IsOn;
    public bool IsNoError => !Editor.IsError;
    public string Input => Editor.CurrentNumber;

    private void SetNumberToInput(dynamic? number)
    {
        try
        {
            Editor.CurrentNumber = number?.ToString();
        }
        catch
        {
            Editor.IsError = true;
            OnPropertyChanged(nameof(IsNoError));
        }

        OnPropertyChanged(nameof(Input));
    }

    private dynamic? GetNumberFromInput()
    {
        return Activator.CreateInstance(typeof(U), Input);
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
            Editor.IsError = true;
            OnPropertyChanged(nameof(IsNoError));
        }

        return (string.Empty, string.Empty);
    }

    public void AppendSymbolToInput(CalculatorButton.Types type)
    {
        if (Input.Length > MAX_NUMBER_LENGTH)
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
        Editor.PopNumber();
        OnPropertyChanged(nameof(Input));
    }

    public void ClearInput()
    {
        Editor.IsError = false;
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
        var operation = CalculatorButton.ActionSymbols[_operationsToTypes[Processor.LastOperation]];
        var equalSign = CalculatorButton.ActionSymbols[CalculatorButton.Types.Equal];

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
        Buffer = $"{Input} {CalculatorButton.ActionSymbols[type]} ";
        Processor.LeftOperand = GetNumberFromInput();
        Processor.LastOperation = _typesToOperations[type];
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
        if (Editor.IsError)
        {
            ClearAll();
            return;
        }

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

    public void ProcessMemoryButton(Memory.Actions action)
    {
        switch (action)
        {
            case lab11.Memory.Actions.Store:
                Memory.Store(GetNumberFromInput());
                break;
            case lab11.Memory.Actions.Read:
                SetNumberToInput(Memory.Read());
                break;
            case lab11.Memory.Actions.Add:
                Memory.Add(GetNumberFromInput());
                break;
            case lab11.Memory.Actions.Subtract:
                Memory.Subtract(GetNumberFromInput());
                break;
            case lab11.Memory.Actions.Clear:
                Memory.Clear();
                break;
        }
        OnPropertyChanged(nameof(IsMemorySet));
    }
}
