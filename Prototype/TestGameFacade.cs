namespace Prototype;

public class TestGameFacade : IGameFacade
{
    private readonly IWorldData _worldData;
    private readonly IEndOfGameData _endOfGameData;
    private readonly string[] _agingImages;
    private Queue<LifeEvent> _events;

    public TestGameFacade(LifeEvent[] events, IWorldData worldData, int startingAge,
        IEndOfGameData endOfGameData, string[] agingImages)
    {
        _worldData = worldData;
        _endOfGameData = endOfGameData;
        _agingImages = agingImages;
        _events = new Queue<LifeEvent>(events);
        _playerData = new PlayerData(new Age(startingAge * 360), 100, 1000, 10, "ABCD");
        PutNextEvent();
    }
    
    private PlayerData _playerData;
    
    public State State { get; private set; }
    public IWaitingData? WaitingData { get; private set; }
    public IDialog? PendingDialog { get; private set; }
    public IEndOfGameData? EndOfGameData { get; private set; }

    public IWorldData GetWorldData(Age atAge)
    {
        return _worldData;
    }

    public IPlayerData PlayerData => _playerData;

    public void RecordWaitedUntil(Age age)
    {
        SetAge(age);
        ScrollForward();
    }

    public void RecordDialogResult(IDialogResult dialogResult)
    {
        PendingDialog = null;
        ScrollForward();
    }

    public void RecordWorldDialogResult(IDialogResult dialogResult, Age age)
    {
        SetAge(age);
        ScrollForward();
    }
    
    private void ScrollForward()
    {
        if (State == State.InWaiting)
        {
            if (WaitingData!.WaitingUntil.Days == _playerData.Age.Days)
            {
                PutNextEvent();
            }
            else
            {
                // keep waiting
            }
        }
        else if (State == State.InDialog)
        {
            PutNextEvent();
        }
        else
        {
            // do nothing
        }
    }

    private void PutNextEvent()
    {
        if (_events.Count == 0)
        {
            SetState(State.GameOver, null, _endOfGameData, null);
        }
        else
        {
            var evt = _events.Peek();
            if (evt.Age.Days > _playerData.Age.Days)
            {
                var waitingData = GetWaitingData(evt.Age);
                SetState(State.InWaiting, null, null, waitingData);
            }
            else
            {
                _events.Dequeue();
                SetState(State.InDialog, evt.Dialog, null, null);
            }
        }
    }

    private IWaitingData? GetWaitingData(Age waitUntil)
    {
        List<AgedImage> images = new();
        int imageChangeYears = 2;
        int index = 0;
        for (int i = _playerData.Age.Days; i < waitUntil.Days; i += imageChangeYears * 360)
        {
            var age = new Age(i);
            images.Add(new AgedImage(_agingImages[index++%_agingImages.Length], age));
        }

        return new WaitingData(waitUntil, images.ToArray());
    }


    private void SetState(State state, IDialog? pendingDialog, IEndOfGameData? eog, IWaitingData? waitingData)
    {
        if (state == State.GameOver)
        {
            PendingDialog = null;
            WaitingData = null;
            EndOfGameData = eog;
        }
        else if (state == State.InWaiting)
        {
            WaitingData = waitingData;
            PendingDialog = pendingDialog;
            EndOfGameData = eog;
        }
        else
        {
            PendingDialog = pendingDialog;
            WaitingData = null;
            EndOfGameData = null;
        }

        State = state;
    }

    private void SetAge(Age age)
    {
        _playerData.Age = age;
    }
}