using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab7
{
    public class PNumber
    {
        double num = 0.0;
        double numBase = 0.0;
        double accuracy = 0.0;
        public PNumber(double a_ = 0, double b_ = 2, double c_ = 0)
        {
            if (b_ < 2 || b_ > 16)
            {
                throw new Exception("Base must be in range [2..16]");
            }

            num = a_;
            numBase = b_;
            accuracy = c_;
        }

        public PNumber(string str_)
        {
            var delimeterPos = str_.Split(",");

            double num_ = double.Parse(delimeterPos[0]);
            int base_ = int.Parse(delimeterPos[1]);
            int accuracy_ = int.Parse(delimeterPos[2]);

            if (base_ < 2 || base_ > 16)
            {
                throw new Exception("Base must be in range [2..16]");
            }

            num = num_;
            numBase = base_;
            accuracy = accuracy_;
        }

        public static PNumber operator +(PNumber lhs, PNumber rhs)
        {
            if (lhs.numBase != rhs.numBase && lhs.accuracy != rhs.accuracy)
            {
                throw new Exception("Base and accuracy must be equals");
            }

            return new PNumber(lhs.num + rhs.num, lhs.numBase, lhs.accuracy);
        }

        public static PNumber operator -(PNumber lhs, PNumber rhs)
        {
            if (lhs.numBase != rhs.numBase && lhs.accuracy != rhs.accuracy)
            {
                throw new Exception("Base and accuracy must be equals");
            }

            return new PNumber(lhs.num - rhs.num, lhs.numBase, lhs.accuracy);
        }

        public static PNumber operator *(PNumber lhs, PNumber rhs)
        {
            if (lhs.numBase != rhs.numBase && lhs.accuracy != rhs.accuracy)
            {
                throw new Exception("Base and accuracy must be equals");
            }

            return new PNumber(lhs.num * rhs.num, lhs.numBase, lhs.accuracy);
        }

        public static PNumber operator /(PNumber lhs, PNumber rhs)
        {
            if (lhs.numBase != rhs.numBase && lhs.accuracy != rhs.accuracy)
            {
                throw new Exception("Base and accuracy must be equals");
            }

            return new PNumber(lhs.num / rhs.num, lhs.numBase, lhs.accuracy);
        }

        public static bool operator ==(PNumber lhs, PNumber rhs)
        {
            return lhs.num == rhs.num && lhs.numBase == rhs.numBase && lhs.accuracy == rhs.accuracy;
        }

        public static bool operator !=(PNumber lhs, PNumber rhs)
        {
            return lhs.num != rhs.num || lhs.numBase != rhs.numBase || lhs.accuracy != rhs.accuracy;
        }

        public static PNumber Revers(PNumber lhs)
        {
            return new PNumber(1 / lhs.num, lhs.numBase, lhs.accuracy);
        }

        public static PNumber Pow(PNumber lhs, int degree = 2)
        {
            return new PNumber(Math.Pow(lhs.num, degree), lhs.numBase, lhs.accuracy);
        }

        public static double GetNum(PNumber lhs)
        {
            return lhs.num;
        }

        public static string GetString(PNumber lhs)
        {
            return $"{lhs.num}, {lhs.numBase}, {lhs.accuracy}";
        }

        public static double GetBase(PNumber lhs)
        {
            return lhs.numBase;
        }

        public static string GetBaseString(PNumber lhs)
        {
            return $"{lhs.numBase}";
        }

        public static double GetAccuracy(PNumber lhs)
        {
            return lhs.accuracy;
        }

        public static string GetAccuracyString(PNumber lhs)
        {
            return $"{lhs.accuracy}";
        }

        public void SetBase(double newBase)
        {
            if (newBase < 2 || newBase > 16)
            {
                throw new Exception("Base must be in range [2..16]");
            }

            //if (newBase < numBase)
            //{
            //    throw new Exception("Base must be bigger");
            //}

            numBase = newBase;
        }

        public void SetBase(string newBase_)
        {
            int newBase = int.Parse(newBase_);

            if (newBase < 2 || newBase > 16)
            {
                throw new Exception("Base must be in range [2..16]");
            }

            //if (newBase < numBase)
            //{
            //    throw new Exception("Base must be bigger");
            //}

            numBase = newBase;
        }

        public void SetAccuracy(double newAccuracy)
        {
            if (newAccuracy < 0)
            {
                throw new Exception("Base must be higher than zero");
            }

            accuracy = newAccuracy;
        }

        public void setAccuracy(string accuracy_)
        {
            int newAccuracy = int.Parse(accuracy_);

            if (newAccuracy < 0)
            {
                throw new Exception("Base must be higher than zero");
            }

            accuracy = newAccuracy;
        }

        public void Show()
        {
            Console.WriteLine($"Number: {num}");
            Console.WriteLine($"Number: {numBase}");
            Console.WriteLine($"Number: {accuracy}");
        }

        public string GetNumber()
        {
            return Convert.ToString((int)num, (int)numBase);
        }
    }
}
