using System.Text.RegularExpressions;

namespace lab9;

public class FractionEditor
{
    private bool _isError = false;
    public bool IsError
    {
        get => _isError;
        set
        {
            _isError = value;
            _currentNumber = "Error";
        }
    }

    private string _currentNumber = "";
    public string CurrentNumber
    {
        get => _currentNumber;
        set
        {
            bool isValid = Regex.Match(value, "[0-9]+").Success;
            if (!isValid)
            {
                throw new Exception("Invalid number");
            }

            _currentNumber = value;
        }
    }

    public FractionEditor()
    {
        Clear();
    }

    public bool IsNull()
    {
        return _currentNumber.Length == 1 && _currentNumber[0] == '0';
    }

    public void ToggleNegative()
    {
        if (IsNull())
        {
            return;
        }

        if (_currentNumber[0] == '-')
        {
            _currentNumber = _currentNumber[1..];
        }
        else
        {
            _currentNumber = $"-{_currentNumber}";
        }
    }

    public void AppendNumber(string num)
    {
        if (IsNull())
        {
            _currentNumber = num;
            return;
        }

        _currentNumber += num;
    }

    public void PopNumber()
    {
        if (_currentNumber.Length == 1)
        {
            Clear();
            return;
        }

        _currentNumber = _currentNumber[..^1];
    }

    public void Clear()
    {
        _currentNumber = "0";
    }
}
