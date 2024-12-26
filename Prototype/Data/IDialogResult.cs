namespace Prototype;

public interface IDialogResult
{
    IDialogStep[] Steps { get; }
    string[] Choices { get; }

    IDialogResult Continue(IDialogStep step, string choice);
}