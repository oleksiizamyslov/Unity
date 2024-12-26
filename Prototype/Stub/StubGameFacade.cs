namespace Prototype.Stub;

public static class StubGameFacade
{
    private const int PlayerStartAge = 18;
    private const int LifeEventsCount = 10;
    
    public static IGameFacade GetFacade()
    {
        var worldData = GetWorldData();
        var ev = GetEvents();
        var eog = new EndOfGameData(StubImages.EndOfGameImage, "Game over");
        var ret = new TestGameFacade(ev, worldData, PlayerStartAge, eog, StubImages.WaitingImages);
        return ret;
    }
    
    private static IWorldData GetWorldData()
    {
        var dialogs = Enumerable.Range(0, 6).Select(GetDialog).ToArray();
        var wd = new WorldData(dialogs);
        return wd;
    }
    
    private static IDialog GetDialog(int index)
    {
        return StubDialog.Dialogs[index % StubDialog.Dialogs.Length];
    }
    
    private static LifeEvent[] GetEvents()
    {
        List<LifeEvent> ret = new();
        var age = new Age(PlayerStartAge*360);
        for (var i = 0; i < LifeEventsCount; i++)
        {
            var evt = new LifeEvent(age, GetDialog(i));
            ret.Add(evt);
            age = age.AddYears(i % 3 == 0 ? 2 : 1);
        }

        return ret.ToArray();
    }

}