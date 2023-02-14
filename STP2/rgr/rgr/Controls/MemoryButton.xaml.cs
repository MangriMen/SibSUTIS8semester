using System.Reflection.Emit;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace rgr.Controls;
public sealed partial class MemoryButton: UserControl
{
    public enum Actions
    {
        None,
        Clear,
        Read,
        Add,
        Subtract,
        Store,
    }

    public static readonly Dictionary<Actions, string> ActionSymbols = new()
    {
        { Actions.None, "" },
        { Actions.Clear, "MC"},
        { Actions.Read, "MR"},
        { Actions.Add, "M+"},
        { Actions.Subtract, "M-"},
        { Actions.Store, "MS" },
    };

    private Actions _action = Actions.None;
    public Actions Action
    {
        get => _action;
        set
        {
            _action = value;
            Content = ActionSymbols[_action];
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

    public MemoryButton()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Click?.Invoke(this, e);
    }
}
