using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using rgr.Models;

namespace rgr.Controls;

public sealed partial class NotationSelector : UserControl
{
    private int[] Notations => Constants.Notations;

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    private readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
        nameof(SelectedIndex),
        typeof(int),
        typeof(NotationSelector),
        new PropertyMetadata(Constants.DEFAULT_NOTATION_INDEX, null)
    );

    public event SelectionChangedEventHandler? SelectionChanged;

    public NotationSelector()
    {
        InitializeComponent();
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectionChanged?.Invoke(this, e);
    }
}
