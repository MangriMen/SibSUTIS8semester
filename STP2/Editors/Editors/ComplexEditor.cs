namespace Editors;

public class ComplexEditor : Editor
{
    public const string ZERO = $"0{SEPARATOR}0";
    public const char SIGN = '-';
    public const string SEPARATOR = "+i*";

    private readonly PNumberEditor _real = new();
    private readonly PNumberEditor _img = new();

    public override string Zero
    {
        get => ZERO;
    }
    public override string Separator { get; set; } = SEPARATOR;

    protected override string AddDigitLS(int digit)
    {
        Number =
            $"{_real.AddDigit(digit)}{(HaveSeparator ? SEPARATOR : string.Empty)}{_img.Number}";
        return Number;
    }

    protected override string AddDigitRS(int digit)
    {
        Number =
            $"{_real.Number}{(HaveSeparator ? SEPARATOR : string.Empty)}{_img.AddDigit(digit)}";
        return Number;
    }

    public override string AddDigit(int digit)
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
            Number += ZERO;
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

            Number = $"{_real.Number}{(HaveSeparator ? SEPARATOR : string.Empty)}{_img.Number}";

            return Number;
        }

        if (!Number.Contains(Separator))
        {
            Number += SEPARATOR;
        }

        HaveSeparator = true;

        return Number;
    }

    public override string Backspace()
    {
        if (Number.Length > 4 && Number[^4..^1] == SEPARATOR)
        {
            Number = Number[..^4];
        }
        else if (Number.Length > 0)
        {
            Number = Number[..^1];
        }

        return Number;
    }

    public override string ToggleNegative()
    {
        Number = Number[0] switch
        {
            SIGN => Number[1..],
            _ => $"{SIGN}{Number}",
        };
        return Number;
    }

    public override string Clear()
    {
        _real.Clear();
        _img.Clear();
        return base.Clear();
    }
}
