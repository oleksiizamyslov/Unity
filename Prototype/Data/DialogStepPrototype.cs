namespace Prototype;

public class DialogStepPrototype
{
    public DialogStepPrototype(string imageUrl, params IDialogItem[] items)
    {
        ImageUrl = imageUrl;
        Items = items;
    }

    public string ImageUrl { get; }
    public IDialogItem[] Items { get; }

    public IDialogStep Init(IDialogResult result, bool isFinal, int index)
    {
        return new DialogStep(ImageUrl, Items, isFinal, result, index);
    }
}