namespace Prototype;

public class TextDialogItem : IDialogItem
{
    public TextDialogItem(string text)
    {
        Text = text;
    }

    public string Text { get; }
}