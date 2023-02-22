using System.Diagnostics;
using System.Text.RegularExpressions;

namespace lab6;

public class Complex
{
    private double _real;
    private double _imag;

    public Complex()
    {
        Init();
    }

    public Complex(double a = 0, double b = 0)
    {
        Init(a, b);
    }

    public Complex(string str)
    {
        var splitter = str.Split('+');
        var a = double.Parse(splitter[0]);
        var b = 0.0;
        
        if (splitter.Length > 2)
        {
            b = double.Parse(Regex.Replace(splitter[1][2..], @"\(|\)", ""));
        }

        Init(a, b);
    }

    private void Init(double a = 0, double b = 0) {
        _real = a;
        _imag = b;
    }

    static public Complex operator +(Complex rhs, Complex lhs)
    {
        return new Complex(lhs._real + rhs._real, lhs._imag + rhs._imag);
    }

    static public Complex operator -(Complex rhs, Complex lhs)
    {
        return new Complex(lhs._real - rhs._real, lhs._imag - rhs._imag);
    }

    static public Complex operator *(Complex rhs, Complex lhs)
    {
        return new Complex(lhs._real * rhs._real - lhs._imag * rhs._imag, lhs._real * rhs._imag + rhs._real * lhs._imag); ;
    }
    static public Complex operator /(Complex rhs, Complex lhs)
    {
        double denomimator = rhs._real * rhs._real + rhs._imag * rhs._imag;
        double nominatorLeft = lhs._real * rhs._real + lhs._imag * rhs._imag;
        double nominatorRight = rhs._real * lhs._real - lhs._imag * rhs._imag;

        return new Complex(nominatorLeft / denomimator, nominatorRight / denomimator);
    }

    static public bool operator ==(Complex lhs, Complex rhs)
    {
        return lhs?._real == rhs?._real && lhs?._imag == rhs?._imag;
    }

    static public bool operator !=(Complex lhs, Complex rhs)
    {
        return lhs._real != rhs._real || lhs._imag != rhs._imag;
    }

    static public Complex Pow(Complex number, double n = 2)
    {
        double phi = Math.Atan2(number._imag, number._real);
        double r = Math.Sqrt(number._real * number._real + number._imag * number._imag);

        double R = Math.Pow(r, n);
        double Phi = n * phi;

        double X = Math.Round(R * Math.Cos(Phi));
        double Y = Math.Round(R * Math.Sin(Phi));

        var complex = new Complex(X, Y);

        return complex;
    }

    public static double Abs(Complex complex)
    {
        return Math.Sqrt(complex._real * complex._real + complex._imag * complex._imag);
    }

    public static double AngleRadians(Complex complex)
    {
        if (complex._real > 0)
        {
            return Math.Atan(complex._imag / complex._real);
        }
        else if (complex._real < 0)
        {
            return Math.Atan(complex._imag / complex._real) + Math.PI;
        }
        else if (complex._real == 0 && complex._imag < 0)
        {
            return -Math.PI / 2;
        }
        else if (complex._real == 0 && complex._imag > 0)
        {
            return Math.PI / 2;
        }
        else
        {
            throw new Exception("Can't take a angle");
        }
    }

    public static Complex Root(Complex complex, int n, int i)
    {
        var roots = new List<Complex>();
        double phi = Math.Pow(Abs(complex), 1 / (double)n);

        Debug.WriteLine(Abs(complex));

        for (int k = 0; k < n; k++)
        {
            double coeff = 2 * Math.PI * k;
            roots.Add(new Complex(phi, 0) * new Complex(Math.Cos((AngleRadians(complex) + coeff) / n), Math.Sin((AngleRadians(complex) + coeff) / n)));
        }

        return roots[i];
    }

    public double GetReal()
    {
        return _real;
    }

    public double GetImag()
    {
        return _imag;
    }

    public string RealString()
    {
        return _real.ToString();
    }

    public string ImagString()
    {
        return _imag.ToString();
    }

    new public string ToString()
    {
        string img = _imag >= 0 ? ImagString() : "(" + ImagString() + ")";
        return RealString() + "+i*" + img;
    }

    public void Show()
    {
        Console.WriteLine($"real: {_real}, imag: {_imag}");
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
