namespace Prototype.Stub;

public class StubDialog
{
    static StubDialog()
    {
        Dialogs = Setup();
    }

    private static IDialog[] Setup()
    {
        return new[] { SimpleOkDialog(), NumericChoiceDialog(), LayeredDialog() };
    }

    private static IDialog SimpleOkDialog()
    {
        var label = new TextDialogItem("This is your message");
        var button = new ButtonDialogItem("OK");
        var step = new DialogStepPrototype(RandomDialogImage(), label, button);
        var d = new Dialog(step);
        return d;
    }

    private static IDialog NumericChoiceDialog()
    {
        var label = new TextDialogItem("This is your message");
        var choice = new NumericDialogItem("Choose your effort", 0, 10, 0);
        var button = new ButtonDialogItem("OK");
        var step = new DialogStepPrototype(RandomDialogImage(), label, choice, button);
        var d = new Dialog(step);
        return d;
    }

    private static IDialog LayeredDialog()
    {
        var accept = new TextDialogItem("This is accept message");
        var decline = new TextDialogItem("This is decline message");
        var okButton = new ButtonDialogItem("OK");
        var acceptStep = new DialogStepPrototype(RandomDialogImage(), accept, okButton);
        var declineStep = new DialogStepPrototype(RandomDialogImage(), decline, okButton);

        var label = new TextDialogItem("This is your message");
        var choice = new NumericDialogItem("Choose your effort", 0, 10, 0);
        var button1 = new ButtonDialogItem("Accept", acceptStep);
        var button2 = new ButtonDialogItem("Decline", declineStep);
        var step = new DialogStepPrototype(RandomDialogImage(), label, choice, button1, button2);
        var dialog = new Dialog(step);
        return dialog;
    }

    public static IDialog[] Dialogs { get; }

    private static string RandomDialogImage()
    {
        return StubImages.DialogImages[Stub.R.Next() % StubImages.DialogImages.Length];
    }
}