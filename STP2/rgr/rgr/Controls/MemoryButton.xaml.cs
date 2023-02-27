using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using lab11;

namespace rgr.Controls;

public sealed partial class MemoryButton : UserControl
{
    private static readonly Dictionary<Memory.Actions, string> ActionsSymbols =
        new()
        {
            { Memory.Actions.Clear, "MC" },
            { Memory.Actions.Read, "MR" },
            { Memory.Actions.Add, "M+" },
            { Memory.Actions.Subtract, "M-" },
            { Memory.Actions.Store, "MS" },
        };

    public Memory.Actions Action
    {
        get => (Memory.Actions)GetValue(ActionProperty);
        set => SetValue(ActionProperty, value);
    }

    private readonly DependencyProperty ActionProperty = DependencyProperty.Register(
        nameof(Action),
        typeof(Memory.Actions),
        typeof(MemoryButton),
        new PropertyMetadata(
            default(Memory.Actions),
            new PropertyChangedCallback(
                (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                {
                    var sender = d as MemoryButton;
                    if (sender == null)
                    {
                        return;
                    }

                    if (e.NewValue == null)
                    {
                        return;
                    }

                    sender.Content = ActionsSymbols[(Memory.Actions)e.NewValue];
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
        typeof(MemoryButton),
        new PropertyMetadata(default(string))
    );

    public event RoutedEventHandler? Click;

    public MemoryButton()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Click?.Invoke(this, e);
    }
}
