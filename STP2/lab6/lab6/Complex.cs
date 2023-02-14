namespace lab6;

public class Complex
{
    private double _real;
    private double _imag;

    public Complex(double a = 0, double b = 0)
    {
        _real = a;
        _imag = b;
    }

    public Complex(string str)
    {
        var splitter = str.Split(' ');

        _real = double.Parse(splitter[0]);
        _imag = double.Parse(splitter[1]);
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
        return lhs._real == rhs._real && lhs._imag == rhs._imag;
    }

    static public bool operator !=(Complex lhs, Complex rhs)
    {
        return lhs._real != rhs._real || lhs._imag != rhs._imag;
    }

    public Complex Pow(int n = 2)
    {
        double phi = Math.Atan2(_imag, _real);
        double r = Math.Sqrt(_real * _real + _imag * _imag);

        double R = Math.Pow(r, n);
        double Phi = n * phi;

        double X = Math.Round(R * Math.Cos(Phi));
        double Y = Math.Round(R * Math.Sin(Phi));

        Complex complex = new Complex(X, Y);

        return complex;
    }

    public double Abs()
    {
        return Math.Sqrt(_real * _real + _imag * _imag);
    }

    public double AngleRadians()
    {
        if (_real > 0)
        {
            return Math.Atan(_imag / _real);
        }
        else if (_real < 0)
        {
            return Math.Atan(_imag / _real) + Math.PI;
        }
        else if (_real == 0 && _imag < 0)
        {
            return -Math.PI / 2;
        }
        else if (_real == 0 && _imag > 0)
        {
            return Math.PI / 2;
        }
        else
        {
            throw new Exception("Can't take a angle");
        }
    }

    public Complex Root(int n, int i)
    {
        List<Complex> roots = new List<Complex>();
        double phi = Math.Pow(Abs(), 1 / (double)n);
        for (int k = 0; k < n; k++)
        {
            double coeff = 2 * Math.PI * k;
            //roots[k] = new Complex(phi, 0) * new Complex(Math.Cos((AngleRadians() + coeff) / n), Math.Sin((AngleRadians() + coeff) / n));
            roots.Add(new Complex(phi, 0) * new Complex(Math.Cos((AngleRadians() + coeff) / n), Math.Sin((AngleRadians() + coeff) / n)));
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

    public string ToString()
    {
        string img = _imag >= 0 ? ImagString() : "(" + ImagString() + ")";
        return RealString() + "+i" + img;
    }

    public void Show()
    {
        Console.WriteLine($"real: {_real}, imag: {_imag}");
    }
}
