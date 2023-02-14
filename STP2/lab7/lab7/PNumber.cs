using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            return lhs._number == rhs._number && lhs._base == rhs._base && lhs._accuracy == rhs._accuracy;
        }

        public static bool operator !=(PNumber lhs, PNumber rhs)
        {
            return lhs._number != rhs._number || lhs._base != rhs._base || lhs._accuracy != rhs._accuracy;
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

            //if (newBase < numBase)
            //{
            //    throw new Exception("Base must be bigger");
            //}
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
            return Convert.ToString((int)_number, _base);
        }

        public static string ToString(PNumber lhs)
        {
            return $"{lhs._number}, {lhs._base}, {lhs._accuracy}";
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
