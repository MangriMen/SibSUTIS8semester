using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace rgr.Helpers;
public class XamlHelper
{
    public static void CalculateFontSize(TextBlock? textBlock)
    {
        if (textBlock == null)
        {
            return;
        }

        var actualTextSize = CalculateTextSize(textBlock.Text, textBlock.FontSize);

        var desiredWidth = (((FrameworkElement)textBlock.Parent)?.ActualWidth ?? 320) / 2;

        if (actualTextSize.Width > desiredWidth)
        {
            var fontsizeMultiplier = Math.Sqrt(desiredWidth / actualTextSize.Width);

            textBlock.FontSize = Math.Floor(textBlock.FontSize * fontsizeMultiplier);
        }

        if (actualTextSize.Height > textBlock.MaxHeight)
        {
            var fontsizeMultiplier = Math.Sqrt(textBlock.MaxHeight / actualTextSize.Height);

            textBlock.FontSize = Math.Floor(textBlock.FontSize * fontsizeMultiplier);
        }
        else
        {
            var fontsizeMultiplier = Math.Sqrt(textBlock.MaxHeight / actualTextSize.Height);

            textBlock.FontSize = Math.Floor(textBlock.FontSize * fontsizeMultiplier);
        }
    }

    public static Size CalculateTextSize(string text, double fontSize)
    {
        var tb = new TextBlock { Text = text, FontSize = fontSize };
        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        return tb.DesiredSize;
    }
}
