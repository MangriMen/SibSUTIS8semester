namespace Editors;

public class FractionEditor : Editor
{
    public const string ZERO = "0";
    public const char SIGN = '-';
    public const char SEPARATOR = '/';

    private readonly PNumberEditor _nominator = new();
    private readonly PNumberEditor _denominator = new();

    public override string Separator { get; set; } = SEPARATOR.ToString();
    public override string Zero => ZERO;

    public override string Number
    {
        get =>
            $"{(HaveSign ? SIGN : string.Empty)}{_nominator.Number}{(HaveSeparator ? SEPARATOR : string.Empty)}{_denominator.Number}";
        set
        {
            if (value == string.Empty)
            {
                return;
            }

            var parts = value.Split(SEPARATOR);

            HaveSign = false;
            HaveSeparator = false;

            _nominator.Number = parts[0];
            if (parts[0][0] == SIGN)
            {
                HaveSign = true;
                _nominator.Number = parts[0][1..];
            }

            _denominator.Number = string.Empty;
            if (parts.Length == 2)
            {
                HaveSeparator = true;
                _denominator.Number = parts[1];
            }
        }
    }

    protected override string AddDigitLS(string digit)
    {
        _nominator.AddDigit(digit);
        return Number;
    }

    protected override string AddDigitRS(string digit)
    {
        _denominator.AddDigit(digit);
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
            _nominator.AddDigit(ZERO);
        }

        HaveSeparator = true;

        return Number;
    }

    public override string Backspace()
    {
        if (Number.Length > 1 && Number[^1] == SEPARATOR)
        {
            HaveSeparator = false;
        }
        else if (Number.Length > 0)
        {
            var delimeterIndex = Number.IndexOf(SEPARATOR);
            if (delimeterIndex < 0)
            {
                _nominator.Backspace();
            }
            else
            {
                _denominator.Backspace();
            }
        }

        return Number;
    }

    public override string ToggleNegative()
    {
        HaveSign = !HaveSign;

        return Number;
    }

    public override string Clear()
    {
        _nominator.Clear();
        _denominator.Clear();
        return base.Clear();
    }
}
