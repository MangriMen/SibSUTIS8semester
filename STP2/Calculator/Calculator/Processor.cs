using System.Reflection;
using Types;
using static Calculator.Processor;

namespace Calculator;

public static class Processor
{
    public enum Operation
    {
        None,
        Equal,
        Divide,
        Minus,
        Multiply,
        Plus,
    }

    public enum Function
    {
        Module,
        Reciprocal,
        Sqr,
        Sqrt
    }
}

public class Processor<T>
    where T : Number, new()
{
    private T _leftOperand = new();
    private T _rightOperand = new();

    private Operation _operation;

    private bool _isOperationDone = true;
    public bool IsOperationDone => _isOperationDone;

    public T LeftOperand
    {
        get => _leftOperand;
        set => _leftOperand = value;
    }

    public T RightOperand
    {
        get => _rightOperand;
        set => _rightOperand = value;
    }

    public Operation LastOperation
    {
        get => _operation;
        set
        {
            _operation = value;
            _isOperationDone = false;
        }
    }

    public Processor()
    {
        Reset();
    }

    public void Reset()
    {
        _leftOperand = new();
        _rightOperand = new();
        ResetOperation();
    }

    public void ResetOperation()
    {
        _operation = Operation.None;
    }

    public void PerformOperation()
    {
        switch (_operation)
        {
            case Operation.Plus:
                _leftOperand = (T)(_leftOperand + _rightOperand);
                break;
            case Operation.Minus:
                _leftOperand = (T)(_leftOperand - _rightOperand);
                break;
            case Operation.Divide:
                _leftOperand = (T)(_leftOperand / _rightOperand);
                break;
            case Operation.Multiply:
                _leftOperand = (T)(_leftOperand * _rightOperand);
                break;
        }

        _isOperationDone = true;
    }

    public void PerformFunction(Function function)
    {
        T result = IsOperationDone switch
        {
            true => _leftOperand,
            false => _rightOperand
        };

        var _oneHundred = new T();
        _oneHundred.FromString("100", result.Base);

        result = function switch
        {
            Function.Module => (T)(result / _oneHundred),
            Function.Reciprocal => (T)result.Reciprocal(),
            Function.Sqr => (T)result.Pow(2),
            Function.Sqrt => (T)result.Root(2),
            _ => throw new ArgumentException("Invalid enum value for function", nameof(function))
        };

        (LeftOperand, RightOperand) = IsOperationDone switch
        {
            true => (result, RightOperand),
            false => (LeftOperand, result)
        };
    }

    private static object?[] GetDefaultParametersForMethod(MethodInfo? method)
    {
        return method
                ?.GetParameters()
                .Where(param => param.HasDefaultValue)
                .Select(param => param.DefaultValue)
                .ToArray() ?? Array.Empty<object>();
    }
}
