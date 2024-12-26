namespace Prototype;

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