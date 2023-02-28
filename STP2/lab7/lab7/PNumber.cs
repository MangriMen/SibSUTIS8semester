using System.Globalization;
using System.Text.RegularExpressions;

namespace lab7
{
    public class PNumber
    {
        const int MIN_BASE = 2;
        const int MAX_BASE = 16;

        double _number = 0.0;
        int _base = 2;
        int _accuracy = 0;

        public PNumber(double number = 0, int @base = 2, int accuracy = 0)
        {
            Init(number, @base, accuracy);
        }

        public PNumber(string str)
        {
            var args = Regex.Replace(str, @"\s+", "").Split(",");
            Init(double.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]));
        }

        private void Init(double number = 0, int @base = 2, int accuracy = 0)
        {
            CheckBaseRange(@base);

            _number = number;
            _base = @base;
            _accuracy = accuracy;
        }

        private static void CheckBaseRange(int base_)
        {
            if (base_ < MIN_BASE || base_ > MAX_BASE)
            {
                throw new Exception("Base must be in range [2..16]");
            }
        }

        private static void CheckAccuracy(int accuracy)
        {
            if (accuracy < 0)
            {
                throw new Exception("Accuracy must be higher than zero");
            }
        }

        private static void CheckBaseAccuracyEquals(PNumber lhs, PNumber rhs)
        {
            if (lhs._base != rhs._base || lhs._accuracy != rhs._accuracy)
            {
                throw new Exception("Base and accuracy must be equals");
            }
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

        public static bool operator ==(PNumber lhs, PNumber rhs)
        {
            return lhs._number == rhs._number
                && lhs._base == rhs._base
                && lhs._accuracy == rhs._accuracy;
        }

        public static bool operator !=(PNumber lhs, PNumber rhs)
        {
            return lhs._number != rhs._number
                || lhs._base != rhs._base
                || lhs._accuracy != rhs._accuracy;
        }

        public static PNumber Revers(PNumber lhs)
        {
            return new PNumber(1 / lhs._number, lhs._base, lhs._accuracy);
        }

        public static PNumber Pow(PNumber lhs, int degree = 2)
        {
            return new PNumber(Math.Pow(lhs._number, degree), lhs._base, lhs._accuracy);
        }

        public double GetNumber()
        {
            return _number;
        }

        public double GetBase()
        {
            return _base;
        }

        public string GetBaseString()
        {
            return _base.ToString();
        }

        public void SetBase(int @base)
        {
            CheckBaseRange(@base);
            _base = @base;
        }

        public void SetBase(string @base)
        {
            _base = int.Parse(@base);
            CheckBaseRange(_base);
        }

        public double GetAccuracy()
        {
            return _accuracy;
        }

        public string GetAccuracyString()
        {
            return _accuracy.ToString();
        }

        public void SetAccuracy(int accuracy)
        {
            CheckAccuracy(accuracy);
            _accuracy = accuracy;
        }

        public void SetAccuracy(string accuracy)
        {
            _accuracy = int.Parse(accuracy);
            CheckAccuracy(_accuracy);
        }

        public string GetConvertedNumber()
        {
            return DecimalToArbitrarySystem(_number, _base);
        }

        new public string ToString()
        {
            return $"{_number}, {_base}, {_accuracy}";
        }

        public override int GetHashCode()
        {
            return string.GetHashCode(ToString());
        }

        public override bool Equals(object? obj)
        {
            return GetHashCode() == obj?.GetHashCode();
        }

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

        /// <summary>
        /// Converts the given decimal number to the numeral system with the
        /// specified radix (in the range [2, 36]).
        /// </summary>
        /// <param name="decimalNumber">The number to convert.</param>
        /// <param name="radix">The radix of the destination numeral system (in the range [2, 36]).</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts the given decimal number to the numeral system with the
        /// specified radix (in the range [2, 36]).
        /// </summary>
        /// <param name="decimalNumber">The number to convert.</param>
        /// <param name="radix">The radix of the destination numeral system (in the range [2, 36]).</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts the given number from the numeral system with the specified
        /// radix (in the range [2, 36]) to decimal numeral system.
        /// </summary>
        /// <param name="number">The arbitrary numeral system number to convert.</param>
        /// <param name="radix">The radix of the numeral system the given number
        /// is in (in the range [2, 36]).</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts the given number from the numeral system with the specified
        /// radix (in the range [2, 36]) to decimal numeral system.
        /// </summary>
        /// <param name="number">The arbitrary numeral system number to convert.</param>
        /// <param name="radix">The radix of the numeral system the given number
        /// is in (in the range [2, 36]).</param>
        /// <returns></returns>
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
}
