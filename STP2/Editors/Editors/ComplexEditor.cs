namespace Editors;

public class ComplexEditor : Editor
{
    public const string ZERO = $"0{SEPARATOR}0";
    public const char SIGN = '-';
    public const string SEPARATOR = "+i*";

    private readonly PNumberEditor _real = new();
    private readonly PNumberEditor _img = new();

    public override string Separator { get; set; } = SEPARATOR;
    public override string Zero => ZERO;

    public override string Number
    {
        get =>
            $"{(HaveSign ? SIGN : string.Empty)}{_real.Number}{(HaveSeparator ? SEPARATOR : string.Empty)}{_img.Number}";
        set
        {
            if (value == string.Empty)
            {
                return;
            }

            var parts = value.Split(SEPARATOR);

            HaveSign = false;
            HaveSeparator = false;

            _real.Number = parts[0];
            if (parts[0][0] == SIGN)
            {
                HaveSign = true;
                _real.Number = parts[0][1..];
            }

            _img.Number = string.Empty;
            if (parts.Length == 2)
            {
                HaveSeparator = true;
                _img.Number = parts[1];
            }
        }
    }

    protected override string AddDigitLS(string digit)
    {
        _real.AddDigit(digit);

        return Number;
    }

    protected override string AddDigitRS(string digit)
    {
        _img.AddDigit(digit);

        return Number;
    }

    public override string AddDigit(string digit)
    {
        var separatorIndex = Number.IndexOf(SEPARATOR);
        if (separatorIndex < 0)
        {
            return AddDigitLS(digit);
        }
        else
        {
            return AddDigitRS(digit);
        }
    }

    public override string AddSeparator()
    {
        if (IsNull)
        {
            _real.AddDigit(PNumberEditor.ZERO);
        }

        if (Separator != SEPARATOR)
        {
            Separator = SEPARATOR.ToString();

            if (!HaveSeparator)
            {
                _real.AddSeparator();
            }
            else
            {
                _img.AddSeparator();
            }

            return Number;
        }

        HaveSeparator = true;

        return Number;
    }

    public override string Backspace()
    {
        if (Number.Length > 3 && Number[^3..] == SEPARATOR)
        {
            HaveSeparator = false;
        }
        else if (Number.Length > 0)
        {
            var delimeterIndex = Number.IndexOf(SEPARATOR);
            if (delimeterIndex < 0)
            {
                _real.Backspace();
            }
            else
            {
                _img.Backspace();
            }
        }

        return Number;
    }

    public override string ToggleNegative()
    {
        if (!HaveSeparator)
        {
            _real.ToggleNegative();
        }
        else
        {
            _img.ToggleNegative();
        }

        return Number;
    }

    public override string Clear()
    {
        _real.Clear();
        _img.Clear();
        return base.Clear();
    }
}
