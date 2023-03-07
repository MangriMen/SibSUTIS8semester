using System.Text.RegularExpressions;

namespace Types;

public class Complex : Number
{
    private const double DEFAULT_IMAGE_REAL = 0;

    private PNumber _real = new();
    private PNumber _img = new();

    public PNumber Real
    {
        get => _real;
        set => _real = value;
    }
    public PNumber Image
    {
        get => _img;
        set => _img = value;
    }
    public override int Base
    {
        get => _real.Base;
        set => _real.Base = _img.Base = value;
    }

    public Complex()
    {
        _real = DEFAULT_IMAGE_REAL;
        _img = DEFAULT_IMAGE_REAL;
    }

    public Complex(PNumber? real = default, PNumber? image = default)
    {
        _real = real ?? DEFAULT_IMAGE_REAL;
        _img = image ?? DEFAULT_IMAGE_REAL;
    }

    public Complex(string number)
    {
        FromString(number);
    }

    public static double Abs(Complex rhs)
    {
        return Math.Sqrt((double)((rhs._real * rhs._real) + (rhs._img * rhs._img)));
    }

    public static double AngleRadians(Complex complex)
    {
        if (complex._real > 0)
        {
            return Math.Atan((double)(complex._img / complex._real));
        }
        else if (complex._real < 0)
        {
            return Math.Atan((double)(complex._img / complex._real)) + Math.PI;
        }
        else if (complex._real == 0 && complex._img < 0)
        {
            return -Math.PI / 2;
        }
        else if (complex._real == 0 && complex._img > 0)
        {
            return Math.PI / 2;
        }
        else
        {
            throw new Exception("Can't take a angle");
        }
    }

    public override bool IsNull()
    {
        return _real == 0 && _img == 0;
    }

    public Complex Root(double n = 2, int i = 0)
    {
        var roots = new List<Complex>();
        double phi = Math.Pow(Abs(this), 1 / (double)n);

        for (int k = 0; k < n; k++)
        {
            double coeff = 2 * Math.PI * k;
            roots.Add(
                new Complex(phi, 0)
                    * new Complex(
                        Math.Cos((AngleRadians(this) + coeff) / n),
                        Math.Sin((AngleRadians(this) + coeff) / n)
                    )
            );
        }

        return roots[i];
    }

    public override Complex Pow(double n = 2)
    {
        double phi = Math.Atan2((double)_img, (double)_real);
        double r = Math.Sqrt((double)(_real * _real + _img * _img));

        double R = Math.Pow(r, n);
        double Phi = n * phi;

        double X = Math.Round(R * Math.Cos(Phi));
        double Y = Math.Round(R * Math.Sin(Phi));

        var complex = new Complex(X, Y);

        return complex;
    }

    public override Complex Root(double n = 2)
    {
        return Root(n, 0);
    }

    public override Number Reciprocal()
    {
        return new Complex(_real, -_img);
    }

    protected override Number Add(Number rhs)
    {
        return this + (Complex)rhs;
    }

    protected override Number Subtract(Number rhs)
    {
        return this - (Complex)rhs;
    }

    protected override Number Multiply(Number rhs)
    {
        return this * (Complex)rhs;
    }

    protected override Number Divide(Number rhs)
    {
        return this / (Complex)rhs;
    }

    protected override bool Equals(Number rhs)
    {
        return this == (Complex)rhs;
    }

    public override void FromString(string number, int @base = 10)
    {
        var parts = number.Split('+');
        _real = new(parts[0], @base);
        _img = 0.0;

        if (parts.Length > 1)
        {
            _img = new(Regex.Replace(parts[1][2..], @"\(|\)", ""), @base);
        }
    }

    public override string ToString()
    {
        string img = _img >= 0 ? Image.ToString() : "(" + Image.ToString() + ")";
        return Real.ToString() + "+i*" + img;
    }

    static public Complex operator +(Complex lhs, Complex rhs)
    {
        return new Complex(rhs._real + lhs._real, rhs._img + lhs._img);
    }

    static public Complex operator -(Complex lhs, Complex rhs)
    {
        return new Complex(lhs._real - rhs._real, lhs._img - rhs._img);
    }

    static public Complex operator *(Complex lhs, Complex rhs)
    {
        return new Complex(
            rhs._real * lhs._real - rhs._img * lhs._img,
            rhs._real * lhs._img + lhs._real * rhs._img
        );
    }

    static public Complex operator /(Complex lhs, Complex rhs)
    {
        double denomimator = (double)(rhs._real * rhs._real + rhs._img * rhs._img);

        double nominatorReal = (double)(lhs._real * rhs._real + lhs._img * rhs._img);
        double nominatorImag = (double)(rhs._real * lhs._img - lhs._real * rhs._img);

        return new Complex(nominatorReal / denomimator, nominatorImag / denomimator);
    }

    static public bool operator ==(Complex rhs, Complex lhs)
    {
        return rhs._real == lhs._real && rhs._img == lhs._img;
    }

    static public bool operator !=(Complex rhs, Complex lhs)
    {
        return rhs._real != lhs._real || rhs._img != lhs._img;
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

        return Equals((Complex)obj);
    }
}
