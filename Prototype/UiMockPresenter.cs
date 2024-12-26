namespace Prototype;

public class UiMockPresenter
{
    public void RefreshPlayerData(IPlayerData playerData)
    {
        // refresh 4 stats in the top and age in the bottom.
    }

    public IDialogResult ShowDialog(IDialog dialog)
    {
        // show dialog, let user scroll through it and return the result
        var step = dialog.GetFirstStep();
        while (!step.IsFinal)
        {
            ShowStep(step);
            var button = (ButtonDialogItem)step.Items.First(p => p is ButtonDialogItem);
            step = dialog.Proceed(step, button);
        }

        return step.Result!;
    }

    public Age Wait(Age currentAge, IWaitingData waitingData)
    {
        // We are waiting from current age to waitingData.WaitingUntil
        // Time passes at X seconds per Year (let's start with X = 4)
        // As the time passes, you show images from the waitingData
        // User can stop playing at any moment to see world dialog, choose something and affect the game.
        var waitTillEnd = (Stub.Stub.R.Next(0, 2) % 2) == 0;
        if (waitTillEnd)
        {
            return waitingData.WaitingUntil;
        }
        else
        {
            var stoppedAtAge = RandomTimeBetween(currentAge, waitingData.WaitingUntil);
            return stoppedAtAge;
        }
    }

    private Age RandomTimeBetween(Age currentAge, Age waitingDataWaitingUntil)
    {
        var daysDiff = waitingDataWaitingUntil.Days - currentAge.Days;
        var r = Stub.Stub.R.Next(daysDiff);
        var ret = new Age(currentAge.Days + r);
        return ret;
    }

    public void ShowEndOfGame(IEndOfGameData eog)
    {
        // Just show the image and the text below it.
    }

    private void ShowStep(IDialogStep step)
    {
        // show image from step.ImageUrl
        // show items from step.Items vertically
        //    TextDialogItem is displayed as a text
        //    NumericDialogItem is displayed as a numeric slider.
        //    User can change the slider, you should call NumericDialogItem.Value.set
        //    ChoiceDialogItem represents a radiobutton. When certain choice is selected,
        //    the current dialog image is changed to the one taken from ChoiceDialogItem
        //    ButtonDialogItem is displayed as a button, usually at the bottom
        //    Click on the button continues the dialog to the next step.
    }

    public IDialogResult ShowWorldDialog(IWorldData worldData, IPlayerData playerData)
    {
        var chosenDialog = worldData.DialogsAvailable[Stub.Stub.R.Next(worldData.DialogsAvailable.Length)];
        var res = ShowDialog(chosenDialog);
        return res;
    }
}