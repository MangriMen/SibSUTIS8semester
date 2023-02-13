using System.Reflection.Emit;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace rgr.Controls;
public sealed partial class CalculatorButton : UserControl
{
    public enum Actions
    {
        None,
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Plus,
        Minus,
        Multiply,
        Divide,
        Equal,
        Module,
        Reciprocal,
        Sqr,
        Sqrt,
        ChangeSign,
        Delimiter,
        Backspace,
        Clear,
        ClearEntry,
    }

    private readonly Dictionary<Actions, string> _contents = new()
    {
        { Actions.None, "" },
        { Actions.Zero, "0"},
        { Actions.One, "1"},
        { Actions.Two, "2"},
        { Actions.Three, "3"},
        { Actions.Four, "4"},
        { Actions.Five, "5"},
        { Actions.Six, "6"},
        { Actions.Seven, "7"},
        { Actions.Eight, "8"},
        { Actions.Nine, "9"},
        { Actions.Plus, "+"},
        { Actions.Minus, "-"},
        { Actions.Multiply, "×"},
        { Actions.Divide, "÷"},
        { Actions.Equal, "="},
        { Actions.Module, "%"},
        { Actions.Reciprocal, "1/x"},
        { Actions.Sqr, "x²"},
        { Actions.Sqrt, "²√x"},
        { Actions.ChangeSign, "+/-"},
        { Actions.Delimiter, ","},
        { Actions.Backspace, "⌫"},
        { Actions.Clear, "C"},
        { Actions.ClearEntry, "CE"},
    };

    private Actions _action = Actions.None;
    public Actions Action
    {
        get => _action;
        set
        {
            _action = value;
            Content = _contents[_action];
        }
    }

    public event RoutedEventHandler? Click;

    public new string Content
    {
        get => (string)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    private readonly new DependencyProperty ContentProperty = DependencyProperty.Register(
    nameof(Label),
    typeof(string),
    typeof(CalculatorButton),
    new PropertyMetadata(null));

    public CalculatorButton()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Click?.Invoke(this, e);
    }
}
