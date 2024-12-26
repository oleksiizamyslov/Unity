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