using System.Numerics;

namespace lab5;

public class Fraction
{
    private BigInteger _nominator = 0;
    private BigInteger _denominator = 0;

    private void Reduce()
    {
        var GCD = Gcd(_nominator, _denominator);

        if (GCD != 1)
        {
            _nominator /= GCD;
            _denominator /= GCD;
        }
    }

    private static BigInteger Gcd(BigInteger a, BigInteger b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static BigInteger Lcm(BigInteger a, BigInteger b)
    {
        return a / Gcd(a, b) * b;
    }

    public Fraction()
    {
        _nominator = 0;
        _denominator = 1;

        Reduce();
    }

    public Fraction(BigInteger numerator, BigInteger denominator)
    {
        _nominator = numerator;
        _denominator = denominator;

        Reduce();
    }

    public Fraction(string fractionString)
    {
        int delimeterPosition = fractionString.IndexOf('/');

        if (delimeterPosition < 0)
        {
            delimeterPosition = fractionString.IndexOf(',');

            if (delimeterPosition < 0)
            {
                if (!BigInteger.TryParse(fractionString, out BigInteger integer))
                {
                    throw new Exception($"Invalid string");
                }

                _nominator = integer;
                _denominator = 1;

                Reduce();
                return;
            }

            var integerPart = fractionString[..delimeterPosition];
            var fractionalPart = fractionString[(delimeterPosition + 1)..];

            var trimmedFractional = fractionalPart.TrimEnd('0');

            _denominator = BigInteger.Parse($"1{new string('0', trimmedFractional.Length)}");
            _nominator =
                BigInteger.Parse(integerPart) * _denominator
                + (BigInteger)double.Parse(fractionalPart);

            Reduce();
            return;
        }

        _nominator = BigInteger.Parse(fractionString[..delimeterPosition]);
        _denominator = BigInteger.Parse(fractionString[(delimeterPosition + 1)..]);

        Reduce();
    }

    public Fraction(Fraction x)
    {
        _nominator = x._nominator;
        _denominator = x._denominator;
    }

    public static Fraction operator +(Fraction a, Fraction b)
    {
        var unionDenominator = Lcm(a._denominator, b._denominator);

        var firstNumber = a._nominator * unionDenominator / a._denominator;
        var secondNumber = b._nominator * unionDenominator / b._denominator;

        var result = new Fraction(firstNumber + secondNumber, unionDenominator);
        result.Reduce();

        return result;
    }

    public static Fraction operator -(Fraction a, Fraction b)
    {
        var unionDenominator = Lcm(a._denominator, b._denominator);

        var firstNumber = a._nominator * unionDenominator / a._denominator;
        var secondNumber = b._nominator * unionDenominator / b._denominator;

        var result = new Fraction(firstNumber - secondNumber, unionDenominator);
        result.Reduce();

        return result;
    }

    public static Fraction operator *(Fraction a, Fraction b)
    {
        var result = new Fraction(a._nominator * b._nominator, a._denominator * b._denominator);
        result.Reduce();

        return result;
    }

    public static Fraction operator /(Fraction a, Fraction b)
    {
        var nominator = a._nominator * b._denominator;
        var denominator = a._denominator * b._nominator;

        if (denominator < 0)
        {
            nominator *= -1;
            denominator *= -1;
        }

        var result = new Fraction(nominator, denominator);
        result.Reduce();

        return result;
    }

    public static Fraction Pow(Fraction a, double n = 2)
    {
        var nominator = Math.Pow((double)a._nominator, n);
        var denominator = Math.Pow((double)a._denominator, n);

        if (n < 1)
        {
            return new((nominator / denominator).ToString());
        }

        var result = new Fraction((BigInteger)nominator, (BigInteger)denominator);
        result.Reduce();

        return result;
    }

    public static Fraction Root(Fraction a, double n = 2)
    {
        return Pow(a, 1 / n);
    }

    public static Fraction Reverse(Fraction a)
    {
        return new(a._denominator, a._nominator);
    }

    public static Fraction Minus(Fraction a)
    {
        Fraction z = new(0, 1);
        return new(z - a);
    }

    public static bool operator ==(Fraction a, Fraction b)
    {
        return a._nominator == b._nominator && a._denominator == b._denominator;
    }

    public static bool operator !=(Fraction a, Fraction b)
    {
        return a._nominator != b._nominator || a._denominator != b._denominator;
    }

    public BigInteger GetNominator()
    {
        return _nominator;
    }

    public BigInteger GetDenominator()
    {
        return _denominator;
    }

    public string GetNominatorString()
    {
        return _nominator.ToString();
    }

    public string GetDenominatorString()
    {
        return _denominator.ToString();
    }

    public string ToFractionString()
    {
        return $"{GetNominatorString()}/{GetDenominatorString()}";
    }

    new public string ToString()
    {
        var floatNumber = (double)GetNominator() / (double)GetDenominator();

        var floatStr = floatNumber.ToString();

        if (floatStr.Length > 18)
        {
            return floatNumber.ToString("e10");
        }

        floatStr = floatNumber.ToString($"0.{new string('#', 16)}");

        if (floatStr.Length > 18)
        {
            return floatNumber.ToString("e10");
        }

        return floatStr;
    }

    public override int GetHashCode()
    {
        return string.GetHashCode(ToString());
    }

    public override bool Equals(object? obj)
    {
        return GetHashCode() == obj?.GetHashCode();
    }
}
