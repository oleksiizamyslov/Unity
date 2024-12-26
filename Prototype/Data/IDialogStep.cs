namespace Prototype;

public interface IDialogStep
{
    int Index { get; }
    string ImageUrl { get; }
    IDialogItem[] Items { get; }
    bool IsFinal { get; }
    IDialogResult Result { get; }
}