namespace Prototype;

public class ButtonDialogItem : IDialogItem
{
    public ButtonDialogItem(string text, DialogStepPrototype? nextStep = null)
    {
        Text = text;
        NextStep = nextStep;
    }

    public string Text { get; }
    public DialogStepPrototype? NextStep { get; }
}