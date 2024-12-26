namespace Prototype;

public class LifeEvent
{
    public LifeEvent(Age age, IDialog dialog)
    {
        Age = age;
        Dialog = dialog;
    }

    public Age Age { get; }
    public IDialog Dialog { get; }
}

public interface IGameFacade
{
    State State { get; }
    IWaitingData? WaitingData { get; }
    IDialog? PendingDialog { get; }
    IWorldData GetWorldData(Age atAge);
    IPlayerData PlayerData { get; }
    IEndOfGameData? EndOfGameData { get; }
    void SendWaitedUntil(Age age);
    void SendDialogResult(IDialogResult dialogResult);
    void SendWorldDialogResult(IDialogResult dialogResult, Age age);
}

public interface IEndOfGameData
{
    string ImageUrl { get; }
    string Text { get; }
}

public class EndOfGameData : IEndOfGameData
{
    public EndOfGameData(string imageUrl, string text)
    {
        ImageUrl = imageUrl;
        Text = text;
    }

    public string ImageUrl { get; }
    public string Text { get; }
}

public interface IDialog
{
    IDialogStep GetFirstStep();
    IDialogStep Proceed(IDialogStep step, ButtonDialogItem clickedItem);
}

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

public interface IDialogStep
{
    int Index { get; }
    string ImageUrl { get; }
    IDialogItem[] Items { get; }
    bool IsFinal { get; }
    IDialogResult Result { get; }
}

public interface IDialogItem
{
}

public class ChoiceDialogItem : IDialogItem
{
    public string SelectedImageUrl { get; }
    public string Text { get; }
}

public class TextDialogItem : IDialogItem
{
    public TextDialogItem(string text)
    {
        Text = text;
    }

    public string Text { get; }
}

public class NumericDialogItem : IDialogItem
{
    public NumericDialogItem(string text, int minValue, int maxValue, int value)
    {
        Text = text;
        MinValue = minValue;
        MaxValue = maxValue;
        Value = value;
    }

    public string Text { get; }
    public int MinValue { get; }
    public int MaxValue { get; }
    public int Value { get; set; }
}

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

public interface IPlayerData
{
    Age Age { get; }
    int Health { get; }
    int Money { get; }
    int Spirit { get; }
    string State { get; }
}

public class PlayerData : IPlayerData
{
    public PlayerData(Age age, int health, int money, int spirit, string state)
    {
        Age = age;
        Health = health;
        Money = money;
        Spirit = spirit;
        State = state;
    }

    public Age Age { get; set; }
    public int Health { get; }
    public int Money { get; }
    public int Spirit { get; }
    public string State { get; }
}

public interface IWorldData
{
    IDialog[] DialogsAvailable { get; }
}

public class WorldData : IWorldData
{
    public WorldData(IDialog[] dialogsAvailable)
    {
        DialogsAvailable = dialogsAvailable;
    }

    public IDialog[] DialogsAvailable { get; }
}

public class DialogResult : IDialogResult
{
    public DialogResult()
    {
        Steps = [];
        Choices = [];
    }

    protected DialogResult(IDialogStep[] steps, string[] choices)
    {
        Steps = steps;
        Choices = choices;
    }

    public IDialogStep[] Steps { get; }
    public string[] Choices { get; }
    public IDialogResult Continue(IDialogStep step, string choice)
    {
        var s2 = Steps.ToList();
        s2.Add(step);
        var c2 = Choices.ToList();
        c2.Add(choice);
        return new DialogResult(s2.ToArray(), c2.ToArray());
    }
}

public interface IDialogResult
{
    IDialogStep[] Steps { get; }
    string[] Choices { get; }

    IDialogResult Continue(IDialogStep step, string choice);
}

public interface IWaitingData
{
    Age WaitingUntil { get; }
    AgedImage[] Images { get; }
}

public class WaitingData : IWaitingData
{
    public WaitingData(Age waitingUntil, AgedImage[] images)
    {
        WaitingUntil = waitingUntil;
        Images = images;
    }

    public Age WaitingUntil { get; }
    public AgedImage[] Images { get; }
}

public class AgedImage
{
    public AgedImage(string imageUrl, Age showUntil)
    {
        ImageUrl = imageUrl;
        ShowUntil = showUntil;
    }

    public string ImageUrl { get; }
    public Age ShowUntil { get; }
}

public class Age
{
    public Age(int days)
    {
        Days = days;
    }

    public int Days { get; }
    public int Years => Days / 360;

    public Age AddYears(int years)
    {
        return new Age(this.Days + years * 360);
    }

    public override string ToString()
    {
        return $"{Years}Y";
    }
}

public enum State
{
    InWaiting,
    InDialog,
    GameOver
}