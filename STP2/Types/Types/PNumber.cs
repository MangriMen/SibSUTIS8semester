using System.Globalization;

namespace Types;

public class PNumber : Number
{
    private const string DEFAULT_NUMBER_STR = "0";
    private const double DEFAULT_NUMBER = 0;
    private const int DEFAULT_BASE = 10;
    private const int DEFAULT_ACCURACY = 16;

    private const int MIN_BASE = 2;
    private const int MAX_BASE = 16;

    private double _number = DEFAULT_NUMBER;
    private int _base = DEFAULT_BASE;
    private int _accuracy = DEFAULT_ACCURACY;

    public double Number
    {
        get => _number;
        set => _number = value;
    }
    public int Base
    {
        get => _base;
        set
        {
            ValidateBase(value);
            _base = value;
        }
    }
    public int Accuracy
    {
        get => _accuracy;
        set
        {
            ValidateAccuracy(value);
            _accuracy = value;
        }
    }

    public PNumber()
    {
        _number = DEFAULT_NUMBER;
        _base = DEFAULT_BASE;
        _accuracy = DEFAULT_ACCURACY;
    }

    public PNumber(
        double number = DEFAULT_NUMBER,
        int @base = DEFAULT_BASE,
        int accuracy = DEFAULT_ACCURACY
    )
    {
        ValidateBase(@base);
        ValidateAccuracy(accuracy);

        _number = number;
        _base = @base;
        _accuracy = accuracy;
    }

    public PNumber(
        string number = DEFAULT_NUMBER_STR,
        int @base = DEFAULT_BASE,
        int accuracy = DEFAULT_ACCURACY
    )
    {
        ValidateBase(@base);
        ValidateAccuracy(accuracy);

        _base = @base;
        _accuracy = accuracy;

        _number = PHelper.ArbitraryToDecimalSystem(number, @base);
    }

    private static void ValidateBase(int @base)
    {
        if (@base < MIN_BASE || @base > MAX_BASE)
        {
            throw new Exception("Base must be in range [2..16]");
        }
    }

    private static void ValidateAccuracy(int accuracy)
    {
        if (accuracy < 0 || accuracy > 16)
        {
            throw new Exception("Accuracy must be in range [2..16]");
        }
    }

    private static void CheckBaseAccuracyEquals(PNumber lhs, PNumber rhs)
    {
        if (lhs._base != rhs._base || lhs._accuracy != rhs._accuracy)
        {
            throw new Exception("Base and accuracy must be equals");
        }
    }

    public bool IsNegative()
    {
        return _number < 0;
    }

    public override bool IsNull()
    {
        return _number == 0;
    }

    public override PNumber Pow(double n = 2)
    {
        return new PNumber(Math.Pow(_number, n), _base, _accuracy);
    }

    public override PNumber Root(double n = 2)
    {
        return Pow(1 / n);
    }

    public override PNumber Reciprocal()
    {
        return new PNumber(1 / _number, _base, _accuracy);
    }

    protected override Number Add(Number rhs)
    {
        return this + (PNumber)rhs;
    }

    protected override Number Subtract(Number rhs)
    {
        return this - (PNumber)rhs;
    }

    protected override Number Multiply(Number rhs)
    {
        return this * (PNumber)rhs;
    }

    protected override Number Divide(Number rhs)
    {
        return this / (PNumber)rhs;
    }

    protected override bool Equals(Number rhs)
    {
        return this == (PNumber)rhs;
    }

    public override void FromString(string number)
    {
        _number = PHelper.ArbitraryToDecimalSystem(number, _base);
    }

    public override string ToString()
    {
        return PHelper.DecimalToArbitrarySystem(_number, _base);
    }

    public static PNumber operator +(PNumber lhs, PNumber rhs)
    {
        CheckBaseAccuracyEquals(lhs, rhs);
        return new PNumber(lhs._number + rhs._number, lhs._base, lhs._accuracy);
    }

    public static PNumber operator -(PNumber lhs, PNumber rhs)
    {
        CheckBaseAccuracyEquals(lhs, rhs);
        return new PNumber(lhs._number - rhs._number, lhs._base, lhs._accuracy);
    }

    public static PNumber operator -(PNumber rhs)
    {
        return new PNumber(-rhs._number, rhs._base, rhs._accuracy);
    }

    public static PNumber operator *(PNumber lhs, PNumber rhs)
    {
        CheckBaseAccuracyEquals(lhs, rhs);
        return new PNumber(lhs._number * rhs._number, lhs._base, lhs._accuracy);
    }

    public static PNumber operator /(PNumber lhs, PNumber rhs)
    {
        CheckBaseAccuracyEquals(lhs, rhs);
        return new PNumber(lhs._number / rhs._number, lhs._base, lhs._accuracy);
    }

    public static PNumber operator %(PNumber lhs, PNumber rhs)
    {
        CheckBaseAccuracyEquals(lhs, rhs);
        return new PNumber(lhs._number % rhs._number, lhs._base, lhs._accuracy);
    }

    public static bool operator ==(PNumber lhs, PNumber rhs)
    {
        return lhs._number == rhs._number
            && lhs._base == rhs._base
            && lhs._accuracy == rhs._accuracy;
    }

    public static bool operator !=(PNumber lhs, PNumber rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator >(PNumber lhs, PNumber rhs)
    {
        return lhs._number > rhs._number;
    }

    public static bool operator <(PNumber lhs, PNumber rhs)
    {
        return lhs._number < rhs._number;
    }

    public static bool operator >=(PNumber lhs, PNumber rhs)
    {
        return lhs._number >= rhs._number;
    }

    public static bool operator <=(PNumber lhs, PNumber rhs)
    {
        return lhs._number <= rhs._number;
    }

    public static implicit operator PNumber(double v)
    {
        return new PNumber(v);
    }

    public static explicit operator double(PNumber v)
    {
        return v._number;
    }

    public override int GetHashCode()
    {
        return string.GetHashCode($"{_number},{_base},{_accuracy}");
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        return Equals((PNumber)obj);
    }
}

public static class PHelper
{
    const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string DecimalToArbitrarySystem(double decimalNumber, int radix)
    {
        if (radix < 2 || radix > Digits.Length)
            throw new ArgumentException(
                "The radix must be >= 2 and <= " + Digits.Length.ToString()
            );

        if (decimalNumber == 0)
            return "0";

        var integerPart = (long)decimalNumber;
        var fractionalPart = decimalNumber - integerPart;

        var integerStr = DecimalToArbitrarySystemInt(integerPart, radix);
        var floatStr = DecimalToArbitrarySystemDouble(fractionalPart, radix);

        var delimiter = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

        if (floatStr == "0")
        {
            return integerStr;
        }

        return $"{integerStr}{delimiter}{floatStr}";
    }

    public static string DecimalToArbitrarySystemInt(long decimalNumber, int radix)
    {
        if (decimalNumber == 0)
            return "0";

        const int BitsInLong = 64;

        int index = BitsInLong - 1;
        var currentNumber = Math.Abs(decimalNumber);
        char[] charArray = new char[BitsInLong];

        while (currentNumber != 0)
        {
            int remainder = (int)(currentNumber % radix);
            charArray[index--] = Digits[remainder];
            currentNumber = currentNumber / radix;
        }

        var result = new string(charArray, index + 1, BitsInLong - index - 1);
        if (decimalNumber < 0)
        {
            result = "-" + result;
        }

        return result;
    }

    public static string DecimalToArbitrarySystemDouble(double decimalNumber, int radix)
    {
        const int PrecisionBits = 16;
        const double precision = 1e-6;

        int index = 0;
        var currentNumber = Math.Abs(decimalNumber);
        char[] charArray = new char[PrecisionBits];

        do
        {
            currentNumber *= radix;
            var num = (int)currentNumber;
            charArray[index++] = Digits[num];

            currentNumber -= num;
        } while (currentNumber > precision && index < PrecisionBits);

        var result = new string(charArray, 0, index);
        return result;
    }

    public static double ArbitraryToDecimalSystem(string number, int radix)
    {
        if (radix < 2 || radix > Digits.Length)
            throw new ArgumentException(
                "The radix must be >= 2 and <= " + Digits.Length.ToString()
            );

        if (string.IsNullOrEmpty(number))
            return 0;

        var splitted = number.Split(",");

        var integerPart = splitted[0];

        var fractionalPart = string.Empty;
        if (splitted.Length > 1)
        {
            fractionalPart = splitted[1];
        }

        var integerNumber = ArbitraryToDecimalSystemInt(integerPart, radix);
        var fractionalNumber = ArbitraryToDecimalSystemDouble(fractionalPart, radix);

        return integerNumber + fractionalNumber;
    }

    public static long ArbitraryToDecimalSystemInt(string number, int radix)
    {
        // Make sure the arbitrary numeral system number is in upper case
        number = number.ToUpperInvariant();

        long result = 0;
        long multiplier = 1;
        for (int i = number.Length - 1; i >= 0; i--)
        {
            char c = number[i];
            if (i == 0 && c == '-')
            {
                // This is the negative sign symbol
                result = -result;
                break;
            }

            int digit = Digits.IndexOf(c);
            if (digit == -1)
                throw new ArgumentException(
                    "Invalid character in the arbitrary numeral system number",
                    "number"
                );

            result += digit * multiplier;
            multiplier *= radix;
        }

        return result;
    }

    public static double ArbitraryToDecimalSystemDouble(string number, int radix)
    {
        // Make sure the arbitrary numeral system number is in upper case
        number = number.ToUpperInvariant();

        var radixDouble = (double)radix;

        double result = 0;
        double multiplier = 1 / radixDouble;
        for (int i = 0; i < number.Length; i++)
        {
            char c = number[i];

            int digit = Digits.IndexOf(c);
            if (digit == -1)
                throw new ArgumentException(
                    "Invalid character in the arbitrary numeral system number",
                    nameof(number)
                );

            result += digit * multiplier;
            multiplier /= radixDouble;
        }

        return result;
    }
}
