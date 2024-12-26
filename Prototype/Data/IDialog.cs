namespace Prototype;

public interface IDialog
{
    IDialogStep GetFirstStep();
    IDialogStep Proceed(IDialogStep step, ButtonDialogItem clickedItem);
}