using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace lab5
{
    public class SimpleFraction
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

        public SimpleFraction(BigInteger numerator, BigInteger denominator)
        {
            _nominator = numerator;
            _denominator = denominator;

            Reduce();
        }

        public SimpleFraction(string fractionString)
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

                    return;
                }

                var integerPart = fractionString[..delimeterPosition];
                var fractionalPart = fractionString[(delimeterPosition + 1)..];

                var trimmedFractional = fractionalPart.TrimEnd('0');

                _denominator = BigInteger.Parse($"1{new string('0', trimmedFractional.Length)}");
                _nominator = BigInteger.Parse(integerPart) * _denominator + (BigInteger)double.Parse(fractionalPart);

                return;
            }

            _nominator = BigInteger.Parse(fractionString[..delimeterPosition]);
            _denominator = BigInteger.Parse(fractionString[(delimeterPosition + 1)..]);
        }

        public SimpleFraction(SimpleFraction x)
        {
            _nominator = x._nominator;
            _denominator = x._denominator;
        }

        public void Show()
        {
            Console.WriteLine($"Nominator {_nominator}");
            Console.WriteLine($"Denominator {_denominator}");
        }

        public static SimpleFraction operator +(SimpleFraction a, SimpleFraction b)
        {
            var unionDenominator = Lcm(a._denominator, b._denominator);

            var firstNumber = a._nominator * unionDenominator / a._denominator;
            var secondNumber = b._nominator * unionDenominator / b._denominator;

            var result = new SimpleFraction(firstNumber + secondNumber, unionDenominator);
            result.Reduce();

            return result;
        }

        public static SimpleFraction operator -(SimpleFraction a, SimpleFraction b)
        {
            var unionDenominator = Lcm(a._denominator, b._denominator);

            var firstNumber = a._nominator * unionDenominator / a._denominator;
            var secondNumber = b._nominator * unionDenominator / b._denominator;

            var result = new SimpleFraction(firstNumber - secondNumber, unionDenominator);
            result.Reduce();

            return result;
        }

        public static SimpleFraction operator *(SimpleFraction a, SimpleFraction b)
        {
            var result = new SimpleFraction(a._nominator * b._nominator, a._denominator * b._denominator);
            result.Reduce();

            return result;
        }

        public static SimpleFraction operator /(SimpleFraction a, SimpleFraction b)
        {
            var nominator = a._nominator * b._denominator;
            var denominator = a._denominator * b._nominator;

            if (denominator < 0)
            {
                nominator *= -1;
                denominator *= -1;
            }

            var result = new SimpleFraction(nominator, denominator);
            result.Reduce();

            return result;
        }

        public static SimpleFraction Pow(SimpleFraction a, double n = 2)
        {
            var nominator = Math.Pow((double)a._nominator, n);
            var denominator = Math.Pow((double)a._denominator, n);
            if (n < 1)
            {
                var res = (nominator / denominator).ToString();
                return new(res);
            }

            var result = new SimpleFraction((BigInteger)nominator, (BigInteger)denominator);
            result.Reduce();

            return result;
        }

        public static SimpleFraction Revers(SimpleFraction a)
        {
            return new(a._denominator, a._nominator);
        }

        public static SimpleFraction Minus(SimpleFraction a)
        {
            SimpleFraction z = new(0, 1);
            return new(z - a);
        }

        public static bool operator ==(SimpleFraction a, SimpleFraction b)
        {
            return a._nominator == b._nominator && a._denominator == b._denominator;
        }

        public static bool operator !=(SimpleFraction a, SimpleFraction b)
        {
            return a._nominator != b._nominator && a._denominator != b._denominator;
        }

        public BigInteger GetNominatorInt()
        {
            return _nominator;
        }

        public BigInteger GetDenominatorInt()
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

        new public string ToString()
        {
            var nominator = GetNominatorString();
            var denominator = GetDenominatorString();

            string sout = nominator + "/" + denominator;

            return sout;
        }

        public string ToFloatString()
        {
            var floatNumber = (double)GetNominatorInt() / (double)GetDenominatorInt();

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
}
