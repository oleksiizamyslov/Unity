namespace Prototype;

internal class DialogStep : IDialogStep
{
    public DialogStep(string imageUrl, IDialogItem[] items, bool isFinal, IDialogResult result, int index)
    {
        ImageUrl = imageUrl;
        Items = items;
        IsFinal = isFinal;
        Result = result;
        Index = index;
    }

    public int Index { get; }
    public string ImageUrl { get; }
    public IDialogItem[] Items { get; }
    public bool IsFinal { get; private set; }
    public IDialogResult Result { get; private set; }
}