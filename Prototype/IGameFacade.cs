namespace Prototype;

public interface IGameFacade
{
    State State { get; }
    IWaitingData? WaitingData { get; }
    IDialog? PendingDialog { get; }
    IWorldData GetWorldData(Age atAge);
    IPlayerData PlayerData { get; }
    IEndOfGameData? EndOfGameData { get; }
    void RecordWaitedUntil(Age age);
    void RecordDialogResult(IDialogResult dialogResult);
    void RecordWorldDialogResult(IDialogResult dialogResult, Age age);
}