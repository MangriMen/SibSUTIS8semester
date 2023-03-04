namespace Editors;

public class FractionEditor : Editor
{
    public const string ZERO = "0";
    public const char SIGN = '-';
    public const char SEPARATOR = '/';

    private readonly PNumberEditor _nominator = new();
    private readonly PNumberEditor _denominator = new();

    public override string Zero
    {
        get => ZERO;
    }
    public override string Separator { get; set; } = SEPARATOR.ToString();

    protected override string AddDigitLS(int digit)
    {
        Number =
            $"{_nominator.AddDigit(digit)}{(HaveSeparator ? SEPARATOR : string.Empty)}{_denominator.Number}";
        return Number;
    }

    protected override string AddDigitRS(int digit)
    {
        Number =
            $"{_nominator.Number}{(HaveSeparator ? SEPARATOR : string.Empty)}{_denominator.AddDigit(digit)}";
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
            Number = ZERO;
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
        if (Number.Length > 0)
        {
            Number = Number[..^1];
        }
        else
        {
            Number = ZERO;
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
        _nominator.Clear();
        _denominator.Clear();
        return base.Clear();
    }
}
