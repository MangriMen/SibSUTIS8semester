namespace Types;

public abstract class Number
{
    public abstract int Base { get; set; }
    public abstract bool IsNull();
    public abstract Number Pow(double n = 2);
    public abstract Number Root(double n = 2);
    public abstract Number Reciprocal();
    protected abstract Number Add(Number rhs);
    protected abstract Number Subtract(Number rhs);
    protected abstract Number Multiply(Number rhs);
    protected abstract Number Divide(Number rhs);
    protected abstract bool Equals(Number rhs);

    public abstract void FromString(string number, int @base = 10);

    public abstract override string ToString();

    public static Number operator +(Number lhs, Number rhs)
    {
        return lhs.Add(rhs);
    }

    public static Number operator -(Number lhs, Number rhs)
    {
        return lhs.Subtract(rhs);
    }

    public static Number operator *(Number lhs, Number rhs)
    {
        return lhs.Multiply(rhs);
    }

    public static Number operator /(Number lhs, Number rhs)
    {
        return lhs.Divide(rhs);
    }
}
