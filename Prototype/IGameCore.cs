namespace Prototype;

public class ScreenRenderer : IScreenRenderer
{
    public ScreenRenderer(IPlayerData playerData, 
        IWaitingScreen? waitingScreen = null, 
        IDialogScreen? dialogScreen = null, 
        IWorldScreen? worldScreen = null, 
        IEndOfGameScreen? endOfGameScreen = null)
    {
        PlayerData = playerData;
        WaitingScreen = waitingScreen;
        DialogScreen = dialogScreen;
        WorldScreen = worldScreen;
        EndOfGameScreen = endOfGameScreen;
    }

    public IPlayerData PlayerData { get; }
    public IWaitingScreen? WaitingScreen { get; }
    public IDialogScreen? DialogScreen { get; }
    public IWorldScreen? WorldScreen { get; }
    public IEndOfGameScreen? EndOfGameScreen { get; }
}

public interface IScreenRenderer
{
    IPlayerData PlayerData { get; }

    IWaitingScreen? WaitingScreen { get; }
    IDialogScreen? DialogScreen { get; }
    IWorldScreen? WorldScreen { get; }
    IEndOfGameScreen? EndOfGameScreen { get; }
}

public interface IEndOfGameScreen
{
    IEndOfGameData Data { get; }
}

public class EndOfGameScreen : IEndOfGameScreen
{
    public EndOfGameScreen(IEndOfGameData data)
    {
        Data = data;
    }

    public IEndOfGameData Data { get; }
}

public interface IWaitingScreen
{
    IWaitingData WaitingData { get; }
    IScreenRenderer WaitUntil(Age age);
}

public interface IDialogScreen
{
    IDialog PendingDialog { get; }
    IScreenRenderer DialogComplete(IDialogResult dialogResult);
}

public interface IWorldScreen
{
    IPlayerData PlayerData { get; }
    IWorldData WorldData { get; }
    IScreenRenderer RecordWorldDialogResult(IDialogResult dialogResult);
}