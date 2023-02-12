using CommunityToolkit.Mvvm.ComponentModel;
using lab5;
using lab9;
using Microsoft.UI.Xaml.Controls;

namespace rgr.ViewModels;

public class StandardViewModel : ObservableRecipient
{
    public readonly FractionEditor FractionEditor = new();

    private bool _isNewInput = false;

    public string MainInput => FractionEditor.CurrentNumber;

    private string _buffer = string.Empty;
    public string Buffer
    {
        get => _buffer;
        set => SetProperty(ref _buffer, value);
    }

    public string _firstOperand = string.Empty;
    public string _secondOperand = string.Empty;
    public string _lastOperation = string.Empty;

    public TextBlock? _mainInputObject;
    public TextBlock? MainInputObject
    {
        get => _mainInputObject;
        set => SetProperty(ref _mainInputObject, value);
    }

    public StandardViewModel()
    {
    }

    public void AppendSymbolToInput(string symbol)
    {
        if (symbol == ",")
        {
            var delimeterIndex = FractionEditor.CurrentNumber.IndexOf(',');
            if (delimeterIndex == -1)
            {
               FractionEditor.CurrentNumber += ',';
                OnPropertyChanged(nameof(MainInput));
                CalculateFontSize();
            }
            return;
        }

        if (_isNewInput)
        {
            FractionEditor.Clear();
        }

        if (MainInput.Length > 16)
        {
            return;
        }

        FractionEditor.AppendNumber(int.Parse(symbol));
        OnPropertyChanged(nameof(MainInput));
        CalculateFontSize();

        _isNewInput = false;
    }

    public void EraseNumberFromInput()
    {
        FractionEditor.PopNumber();
        OnPropertyChanged(nameof(MainInput));
        CalculateFontSize();
    }

    public void ClearInput()
    {
        FractionEditor.Clear();
        OnPropertyChanged(nameof(MainInput));
    }

    private void ClearBuffer()
    {
        Buffer = "";
    }

    public void ClearAll()
    {
        ClearBuffer();
        ClearInput();
        CalculateFontSize();
    }

    public void ToggleNegative()
    {
        FractionEditor.ToggleNegative();
        OnPropertyChanged(nameof(MainInput));
    }

    public void OperationPerformed(string operation)
    {
        switch (operation)
        {
            case "=":
                _secondOperand = MainInput;
                Buffer += $"{MainInput} {operation}";
                var a = new SimpleFraction(_firstOperand);
                var b = new SimpleFraction(_secondOperand);

                switch (_lastOperation)
                {
                    case "+":
                        FractionEditor.CurrentNumber = (a + b).ToFloatString();
                        break;
                    case "-":
                        FractionEditor.CurrentNumber = (a - b).ToFloatString();
                        break;
                    case "×":
                        FractionEditor.CurrentNumber = (a * b).ToFloatString();
                        break;
                    case "÷":
                        FractionEditor.CurrentNumber = (a / b).ToFloatString();
                        break;
                }
                break;
            default:
                _firstOperand = MainInput;
                _lastOperation = operation;
                Buffer = $"{MainInput} {operation} ";
                break;
        }

        OnPropertyChanged(nameof(MainInput));
        _isNewInput = true;
    }

    public void OneOperandOperationPerformed(string operation)
    {
        switch (operation)
        {
            case "1/x":
                Buffer = $"1/( {MainInput} )";
                break;
            case "x²":
                Buffer = $"sqr( {MainInput} )";
                break;
            case "²√x":
                Buffer = $"sqrt( {MainInput} )";
                break;
        }
        _isNewInput = true;
    }

    public void CalculateFontSize()
    {
        if (MainInputObject == null)
        {
            return;
        }

        var desiredHeight = 80;

        var fontsizeMultiplier = Math.Sqrt(desiredHeight / MainInputObject.ActualHeight);

        MainInputObject.FontSize = Math.Floor(MainInputObject.FontSize * fontsizeMultiplier);
    }

    public void NumberButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var button = (Button)sender;
        var buttonContent = button?.Content?.ToString() ?? string.Empty;

        if (buttonContent == string.Empty)
        {
            return;
        }

        AppendSymbolToInput(buttonContent);
    }

    public void OperationButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var button = (Button)sender;
        var buttonContent = button?.Content?.ToString() ?? string.Empty;

        if (buttonContent == string.Empty)
        {
            return;
        }

        OperationPerformed(buttonContent);
    }

    public void OneOperandOperationButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var button = (Button)sender;
        var buttonContent = button?.Content?.ToString() ?? string.Empty;

        if (buttonContent == string.Empty)
        {
            return;
        }

        OneOperandOperationPerformed(buttonContent);
    }

    public void DelimeterButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var button = (Button)sender;
        var buttonContent = button?.Content?.ToString() ?? string.Empty;

        if (buttonContent == string.Empty)
        {
            return;
        }

        AppendSymbolToInput(buttonContent);
    }

    public void MainInput_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        MainInputObject = (TextBlock)sender;
    }
}
