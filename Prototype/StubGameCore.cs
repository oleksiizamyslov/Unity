namespace Prototype;

public class WaitingScreen : IWaitingScreen
{
    private StubGameCore _core;

    public WaitingScreen(StubGameCore core, IWaitingData waitingData)
    {
        _core = core;
        WaitingData = waitingData;
    }

    public IWaitingData WaitingData { get; }

    public IScreenRenderer WaitUntil(Age age)
    {
        if (age.Days > WaitingData.WaitingUntil.Days)
        {
            throw new InvalidOperationException();
        }
        _core.SetAge(age);
        return _core.ResolveRenderer();
    }
}

public class DialogScreen : IDialogScreen
{
    private StubGameCore _gameCore;

    public DialogScreen(IDialog pendingDialog, StubGameCore gameCore)
    {
        PendingDialog = pendingDialog;
        _gameCore = gameCore;
    }

    public IDialog PendingDialog { get; }

    public IScreenRenderer DialogComplete(IDialogResult dialogResult)
    {
        _gameCore.MarkEventComplete();
        return _gameCore.ResolveRenderer();
    }
}

public class StubGameCore
{
    private readonly IWorldData _worldData;
    private readonly IEndOfGameData _endOfGameData;
    private readonly string[] _agingImages;
    private Queue<LifeEvent> _events;

    public StubGameCore(LifeEvent[] events, IWorldData worldData, int startingAge,
        IEndOfGameData endOfGameData, string[] agingImages)
    {
        _worldData = worldData;
        _endOfGameData = endOfGameData;
        _agingImages = agingImages;
        _events = new Queue<LifeEvent>(events);
        _playerData = new PlayerData(new Age(startingAge * 360), 100, 1000, 10, "ABCD");
    }

    private PlayerData _playerData;

    public IWaitingData? WaitingData { get; private set; }
    public IDialog? PendingDialog { get; private set; }
    public IEndOfGameData? EndOfGameData { get; private set; }

    public IWorldData GetWorldData(Age atAge)
    {
        return _worldData;
    }

    public IPlayerData PlayerData => _playerData;
    
    public void MarkEventComplete()
    {
        _events.Dequeue();
    }

    public IScreenRenderer ResolveRenderer()
    {
        if (_events.Count == 0)
        {
            return new ScreenRenderer(_playerData, endOfGameScreen: new EndOfGameScreen(_endOfGameData));
        }
        
        var evt = _events.Peek();
        if (evt.Age.Days > _playerData.Age.Days)
        {
            var waitingData = GetWaitingData(evt.Age);
            return new ScreenRenderer(_playerData, waitingScreen: new WaitingScreen(this, waitingData));
        }
        else
        {
            var ds = new DialogScreen(evt.Dialog, this);
            var renderer = new ScreenRenderer(_playerData, dialogScreen: ds);
            return renderer;
        }
    }

    private IWaitingData GetWaitingData(Age waitUntil)
    {
        List<AgedImage> images = new();
        int imageChangeYears = 2;
        int index = 0;
        for (int i = _playerData.Age.Days; i < waitUntil.Days; i += imageChangeYears * 360)
        {
            var age = new Age(i);
            images.Add(new AgedImage(_agingImages[index++ % _agingImages.Length], age));
        }

        return new WaitingData(waitUntil, images.ToArray());
    }
    //
    // public IScreenRenderer NextState(IDialog? pendingDialog, IEndOfGameData? eog, IWaitingData? waitingData)
    // {
    //     if (state == State.GameOver)
    //     {
    //         PendingDialog = null;
    //         WaitingData = null;
    //         EndOfGameData = eog;
    //     }
    //     else if (state == State.InWaiting)
    //     {
    //         WaitingData = waitingData;
    //         PendingDialog = pendingDialog;
    //         EndOfGameData = eog;
    //     }
    //     else
    //     {
    //         PendingDialog = pendingDialog;
    //         WaitingData = null;
    //         EndOfGameData = null;
    //     }
    //
    //     State = state;
    // }

    public void SetAge(Age age)
    {
        _playerData.Age = age;
    }
}