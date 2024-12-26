namespace Prototype;

public class WorldData : IWorldData
{
    public WorldData(IDialog[] dialogsAvailable)
    {
        DialogsAvailable = dialogsAvailable;
    }

    public IDialog[] DialogsAvailable { get; }
}