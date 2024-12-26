namespace Prototype;

public class Dialog : IDialog
{
    private readonly DialogStepPrototype _firstStep;

    public Dialog(DialogStepPrototype firstStep)
    {
        _firstStep = firstStep;
    }

    public IDialogStep GetFirstStep()
    {
        var r = _firstStep.Init(new DialogResult(), false, 0);
        return r;
    }

    public IDialogStep Proceed(IDialogStep step, ButtonDialogItem clickedItem)
    {
        var res = step.Result.Continue(step, clickedItem.Text);
        if (clickedItem.NextStep == null)
        {
            return new DialogStep(null, null, true, res, step.Index + 1);
        }

        var nextStep = clickedItem.NextStep;
        var ret = nextStep.Init(res, false, step.Index + 1);
        return ret;
    }
}