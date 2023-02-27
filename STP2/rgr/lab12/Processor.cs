using System.Numerics;
using System.Reflection;
using lab5;

namespace lab12;
public class Processor<T> where T : new()
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
        set {
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
        dynamic? result;
        if (IsOperationDone)
        {
            result = LeftOperand;
        }
        else
        {
            result = RightOperand;
        }

        if ((T?)result == null)
        {
            return;
        }

        var sqrtArguments = new object[] { result, 2 };
        if (typeof(T) == typeof(Complex))
        {
            sqrtArguments = (object[])sqrtArguments.Append(0);
        }

        switch (function)
        {
            case Function.Module:
                result /= _oneHundred;
                break;
            case Function.Reciprocal:
                result = Fraction.Reverse(result);
                break;
            case Function.Sqr:
                result = (T?)(_methodPow?.Invoke(null, new object?[] { result, 2 })) ?? result;
                break;
            case Function.Sqrt:
                result = (T?)(_methodSqrt?.Invoke(null, sqrtArguments)) ?? result;
                break;
        }

        if (IsOperationDone)
        {
            LeftOperand = result;
        }
        else
        {
            RightOperand = result;
        }
    }

}
