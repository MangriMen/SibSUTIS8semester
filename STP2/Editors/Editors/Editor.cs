namespace Editors;

public abstract class Editor
{
    public abstract string Separator { get; set; }
    public abstract string Zero { get; }

    public virtual string Number { get; set; } = string.Empty;
    public virtual bool HaveSeparator { get; protected set; }
    public virtual bool HaveSign { get; protected set; }
    public virtual bool IsNull => Number == string.Empty;

    protected abstract string AddDigitLS(string digit);
    protected abstract string AddDigitRS(string digit);

    public virtual string Clear()
    {
        HaveSeparator = false;
        Number = string.Empty;
        return Number;
    }

    public abstract string AddDigit(string digit);
    public abstract string AddSeparator();
    public abstract string ToggleNegative();
    public abstract string Backspace();
}
