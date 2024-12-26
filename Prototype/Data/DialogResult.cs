namespace Prototype;

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

    public override string ToString()
    {
        return $"{string.Join(", ", Choices)}; {string.Join(", ", Steps.Select(p => p.ToString()))}";
    }
}