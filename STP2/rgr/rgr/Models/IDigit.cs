namespace rgr.Models;
public interface IDigit<T> where T : IDigit<T>
{
    public static IDigit<T> operator +(IDigit<T> left, IDigit<T> right)
    {
        return left + right;
    }
}
