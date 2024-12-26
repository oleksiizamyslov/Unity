namespace Prototype;

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