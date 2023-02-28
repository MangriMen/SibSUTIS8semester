using System.Reflection;
using lab5;
using static lab12.Processor;

namespace lab12;

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
    where T : new()
{
    private dynamic? _leftOperand;
    private dynamic? _rightOperand;

    private bool _isOperationDone = true;
    public bool IsOperationDone => _isOperationDone;

    private Operation _operation;

    private readonly MethodInfo? _methodPow = typeof(T).GetMethod("Pow");
    private readonly MethodInfo? _methodSqrt = typeof(T).GetMethod("Root");

    private readonly dynamic? _oneHundred = (T?)Activator.CreateInstance(typeof(T), "100");

    public dynamic? LeftOperand
    {
        get => _leftOperand;
        set => _leftOperand = value;
    }

    public dynamic? RightOperand
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
        _leftOperand = Activator.CreateInstance(typeof(T));
        _rightOperand = Activator.CreateInstance(typeof(T));
        ResetOperation();
    }

    public void ResetOperation()
    {
        _operation = Operation.None;
    }

    public void PerformOperation()
    {
        dynamic? rightOperand = RightOperand;

        switch (_operation)
        {
            case Operation.Plus:
                _leftOperand += rightOperand;
                break;
            case Operation.Minus:
                _leftOperand -= rightOperand;
                break;
            case Operation.Divide:
                _leftOperand /= rightOperand;
                break;
            case Operation.Multiply:
                _leftOperand *= rightOperand;
                break;
        }

        _isOperationDone = true;
    }

    public void PerformFunction(Function function)
    {
        dynamic? result = IsOperationDone switch
        {
            true => LeftOperand,
            false => RightOperand
        };

        if ((T?)result == null)
        {
            return;
        }

        var sqrParameters = new List<object?> { result };
        sqrParameters.AddRange(GetDefaultParametersForMethod(_methodPow));

        var sqrtParameters = new List<object?> { result };
        sqrtParameters.AddRange(GetDefaultParametersForMethod(_methodSqrt));

        result = function switch
        {
            Function.Module => result / _oneHundred,
            Function.Reciprocal => Fraction.Reverse(result),
            Function.Sqr => (T?)_methodPow?.Invoke(null, sqrParameters.ToArray()),
            Function.Sqrt => (T?)_methodSqrt?.Invoke(null, sqrtParameters.ToArray()),
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
