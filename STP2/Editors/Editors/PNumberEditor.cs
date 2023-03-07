namespace Editors;

public class PNumberEditor : Editor
{
    public const string ZERO = "0";
    public const char SIGN = '-';
    public const char SEPARATOR = ',';
    public override string Separator { get; set; } = SEPARATOR.ToString();

    public override string Zero => ZERO;

    protected override string AddDigitLS(string digit)
    {
        if (digit == ZERO)
        {
            if (Number.Length == 0 || (Number.Length == 1 && Number.StartsWith(ZERO)))
            {
                return Number;
            }
        }

        var digitStr = digit.ToString();

        if (Number.Length == 2 && Number.StartsWith(ZERO))
        {
            Number = digitStr;
        }

        var separatorIndex = Number.IndexOf(SEPARATOR);
        if (separatorIndex < 0)
        {
            Number += digitStr;
        }
        else
        {
            Number = Number.Insert(separatorIndex, digitStr);
        }

        return Number;
    }

    protected override string AddDigitRS(string digit)
    {
        if (digit == ZERO)
        {
            var separatorIndex = Number.IndexOf(SEPARATOR);
            var rightPart = Number[(separatorIndex + 1)..];
            if (rightPart.Length == 1 && rightPart.StartsWith(ZERO))
            {
                return Number;
            }
        }

        Number += digit.ToString();
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
}
