using System.Numerics;

namespace Types;

public class Fraction : Number
{
    private const int DEFAULT_NOMINATOR = 0;
    private const int DEFAULT_DENOMINATOR = 1;

    private PNumber _nominator = new();
    private PNumber _denominator = new();

    public PNumber Nominator
    {
        get => _nominator;
        set => _nominator = value;
    }
    public PNumber Denominator
    {
        get => _denominator;
        set => _denominator = value;
    }

    public Fraction()
    {
        _nominator = DEFAULT_NOMINATOR;
        _denominator = DEFAULT_DENOMINATOR;
    }

    public Fraction(PNumber? nominator = default, PNumber? denominator = default)
    {
        _nominator = nominator ?? DEFAULT_NOMINATOR;
        _denominator = denominator ?? DEFAULT_DENOMINATOR;

        Reduce();
    }

    public Fraction(string number)
    {
        FromString(number);
    }

    private static PNumber Gcd(PNumber a, PNumber b)
    {
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }
        return a;
    }

    private static PNumber Lcm(PNumber a, PNumber b)
    {
        return a / Gcd(a, b) * b;
    }

    private void Reduce()
    {
        var GCD = Gcd(_nominator, _denominator);

        if (GCD != 1)
        {
            _nominator /= GCD;
            _denominator /= GCD;
        }
    }

    public override bool IsNull()
    {
        return _nominator == 0;
    }

    public override Fraction Pow(double n = 2)
    {
        var nominator = Math.Pow((double)_nominator, n);
        var denominator = Math.Pow((double)_denominator, n);

        if (n < 1)
        {
            return new((nominator / denominator).ToString());
        }

        var result = new Fraction((PNumber)nominator, (PNumber)denominator);
        result.Reduce();

        return result;
    }

    public override Fraction Root(double n = 2)
    {
        return Pow(1 / n);
    }

    public override Fraction Reciprocal()
    {
        return new(_denominator, _nominator);
    }

    protected override Fraction Add(Number rhs)
    {
        return this + (Fraction)rhs;
    }

    protected override Fraction Subtract(Number rhs)
    {
        return this - (Fraction)rhs;
    }

    protected override Fraction Multiply(Number rhs)
    {
        return this * (Fraction)rhs;
    }

    protected override Fraction Divide(Number rhs)
    {
        return this / (Fraction)rhs;
    }

    protected override bool Equals(Number rhs)
    {
        var rhs_ = (Fraction)rhs;
        return _nominator == rhs_._nominator && _denominator == rhs_._denominator;
    }

    public override void FromString(string number)
    {
        int delimeterPosition = number.IndexOf('/');

        if (delimeterPosition < 0)
        {
            delimeterPosition = number.IndexOf(',');

            if (delimeterPosition < 0)
            {
                _nominator = new PNumber(number, 10, 16);
                _denominator = 1;

                Reduce();
                return;
            }

            var integerPart = number[..delimeterPosition];
            var fractionalPart = number[(delimeterPosition + 1)..];

            var trimmedFractional = fractionalPart.TrimEnd('0');

            _denominator = new PNumber($"1{new string('0', trimmedFractional.Length)}");
            _nominator =
                new PNumber(integerPart) * _denominator + (PNumber)double.Parse(fractionalPart);

            Reduce();
            return;
        }

        _nominator = new PNumber(number[..delimeterPosition]);
        _denominator = new PNumber(
            number.Length - 1 > delimeterPosition ? number[(delimeterPosition + 1)..] : "1"
        );

        Reduce();
    }

    public override string ToString()
    {
        return $"{Nominator}/{Denominator}";
    }

    public static Fraction operator +(Fraction lhs, Fraction rhs)
    {
        var unionDenominator = Lcm(lhs._denominator, rhs._denominator);

        var firstNumber = lhs._nominator * unionDenominator / lhs._denominator;
        var secondNumber = rhs._nominator * unionDenominator / rhs._denominator;

        var result = new Fraction(firstNumber + secondNumber, unionDenominator);
        result.Reduce();

        return result;
    }

    public static Fraction operator -(Fraction lhs, Fraction rhs)
    {
        var unionDenominator = Lcm(lhs._denominator, rhs._denominator);

        var firstNumber = lhs._nominator * unionDenominator / lhs._denominator;
        var secondNumber = rhs._nominator * unionDenominator / rhs._denominator;

        var result = new Fraction(firstNumber - secondNumber, unionDenominator);
        result.Reduce();

        return result;
    }

    public static Fraction operator *(Fraction lhs, Fraction rhs)
    {
        var result = new Fraction(
            lhs._nominator * rhs._nominator,
            lhs._denominator * rhs._denominator
        );
        result.Reduce();

        return result;
    }

    public static Fraction operator /(Fraction lhs, Fraction rhs)
    {
        var nominator = lhs._nominator * rhs._denominator;
        var denominator = lhs._denominator * rhs._nominator;

        if (denominator < 0)
        {
            nominator *= -1;
            denominator *= -1;
        }

        var result = new Fraction(nominator, denominator);
        result.Reduce();

        return result;
    }

    public static bool operator ==(Fraction lhs, Fraction rhs)
    {
        return lhs._nominator == rhs._nominator && lhs._denominator == rhs._denominator;
    }

    public static bool operator !=(Fraction lhs, Fraction rhs)
    {
        return !(lhs == rhs);
    }

    public override int GetHashCode()
    {
        return string.GetHashCode(ToString());
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

        return Equals((Fraction)obj);
    }
}
