namespace Prototype;

public interface IWaitingData
{
    Age WaitingUntil { get; }
    AgedImage[] Images { get; }
}