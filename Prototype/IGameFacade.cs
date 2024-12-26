namespace Prototype;

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