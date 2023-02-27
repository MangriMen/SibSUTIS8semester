using System.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace rgr.Controls;

public sealed partial class CalculatorButton : UserControl
{
    public enum Types
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
        Ten,
        Eleven,
        Twelve,
        Thirteen,
        Fourteen,
        Fifteen,
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
        ComplexI,
    }

    public static readonly Dictionary<Types, string> ActionSymbols =
        new()
        {
            { Types.None, "" },
            { Types.Zero, "0" },
            { Types.One, "1" },
            { Types.Two, "2" },
            { Types.Three, "3" },
            { Types.Four, "4" },
            { Types.Five, "5" },
            { Types.Six, "6" },
            { Types.Seven, "7" },
            { Types.Eight, "8" },
            { Types.Nine, "9" },
            { Types.Ten, "A" },
            { Types.Eleven, "B" },
            { Types.Twelve, "C" },
            { Types.Thirteen, "D" },
            { Types.Fourteen, "E" },
            { Types.Fifteen, "F" },
            { Types.Plus, "+" },
            { Types.Minus, "-" },
            { Types.Multiply, "×" },
            { Types.Divide, "÷" },
            { Types.Equal, "=" },
            { Types.Module, "%" },
            { Types.Reciprocal, "1/x" },
            { Types.Sqr, "x²" },
            { Types.Sqrt, "²√x" },
            { Types.ChangeSign, "+/-" },
            { Types.Delimiter, "," },
            { Types.Backspace, "⌫" },
            { Types.Clear, "C" },
            { Types.ClearEntry, "CE" },
            { Types.ComplexI, "i" },
        };

    public Types Type
    {
        get => (Types)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    private readonly DependencyProperty TypeProperty = DependencyProperty.Register(
        nameof(Type),
        typeof(Types),
        typeof(CalculatorButton),
        new PropertyMetadata(
            default(Types),
            new PropertyChangedCallback(
                (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                {
                    var sender = d as CalculatorButton;
                    if (sender == null)
                    {
                        return;
                    }

                    if (e.NewValue == null)
                    {
                        return;
                    }

                    sender.Content = ActionSymbols[(Types)e.NewValue];
                }
            )
        )
    );

    public new string Content
    {
        get => (string)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    private readonly new DependencyProperty ContentProperty = DependencyProperty.Register(
        nameof(Content),
        typeof(string),
        typeof(CalculatorButton),
        new PropertyMetadata(default(string))
    );

    public int Notation
    {
        get => (int)GetValue(NotationProperty);
        set => SetValue(NotationProperty, value);
    }

    private readonly DependencyProperty NotationProperty = DependencyProperty.Register(
        nameof(Notation),
        typeof(int),
        typeof(CalculatorButton),
        new PropertyMetadata(
            default(int),
            new PropertyChangedCallback(
                (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                {
                    var sender = d as CalculatorButton;
                    if (sender == null)
                    {
                        return;
                    }

                    if (e.NewValue == null)
                    {
                        return;
                    }

                    sender.IsEnabled = Convert.ToInt32(sender.Content, 16) < (int)e.NewValue;
                }
            )
        )
    );

    public event RoutedEventHandler? Click;

    public CalculatorButton()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Click?.Invoke(this, e);
    }
}
