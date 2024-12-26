namespace Prototype;

public class NumericDialogItem : IDialogItem
{
    public NumericDialogItem(string text, int minValue, int maxValue, int value)
    {
        Text = text;
        MinValue = minValue;
        MaxValue = maxValue;
        Value = value;
    }

    public string Text { get; }
    public int MinValue { get; }
    public int MaxValue { get; }
    public int Value { get; set; }
}