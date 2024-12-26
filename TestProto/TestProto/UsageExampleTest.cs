using System.Security.Cryptography;
using System.Text;
using Prototype;

namespace TestProto;

public class UsageExampleTest
{
    private const int PlayerStartAge = 18;
    private const int LifeEventsCount = 10;

    [Fact]
    public void Test()
    {
        var gf = GetFacade();
        var mockPresenter = new UiMockPresenter();
        var log = new StringBuilder();

        while (true)
        {
            if (gf.State == State.InDialog)
            {
                var res = mockPresenter.ShowDialog(gf.PendingDialog!);
                gf.SendDialogResult(res);
                log.AppendLine($"{gf.PlayerData.Age}: Dialog result: {res}");
            }
            else if (gf.State == State.InWaiting)
            {
                var res = mockPresenter.Wait(gf.PlayerData.Age, gf.WaitingData!, gf.GetWorldData);
                if (res.worldDialogResult != null)
                {
                    gf.SendWorldDialogResult(res.worldDialogResult, res.age);
                    log.AppendLine($"{gf.PlayerData.Age}: World result at {res.age}: {res.worldDialogResult}");
                }
                else
                {
                    gf.SendWaitedUntil(res.age);
                    log.AppendLine($"{gf.PlayerData.Age}: Wait until {res.age}");
                }
            }
            else
            {
                mockPresenter.ShowEndOfGame(gf.EndOfGameData!);
                log.AppendLine($"{gf.PlayerData.Age}: Game over");
                break;
            }
        }

        Console.WriteLine(log.ToString());
    }

    private IGameFacade GetFacade()
    {
        var worldData = GetWorldData();
        var ev = Events();
        var eog = new EndOfGameData(MockImages.EndOfGameImage, "Game over");
        var ret = new TestGameFacade(ev, worldData, PlayerStartAge, eog, MockImages.WaitingImages);
        return ret;
    }

    private LifeEvent[] Events()
    {
        List<LifeEvent> ret = new();
        var age = new Age(PlayerStartAge);
        for (var i = 0; i < LifeEventsCount; i++)
        {
            var evt = new LifeEvent(age, GetDialog(i));
            ret.Add(evt);
            age = age.AddYears(i%3 == 0 ? 2 : 1);
        }

        return ret.ToArray();
    }

    private IWorldData GetWorldData()
    {
        var dialogs = Enumerable.Range(0, 6).Select(GetDialog).ToArray();
        var wd = new WorldData(dialogs);
        return wd;
    }

    private IDialog GetDialog(int index)
    {
        return MockDialog.Dialogs[index % MockDialog.Dialogs.Length];
    }
}

class MockDialog
{
    static MockDialog()
    {
        Dialogs = Setup();
    }

    private static IDialog[] Setup()
    {
        return new[] { SimpleOkDialog(), NumericChoiceDialog(), LayeredDialog() };
    }

    private static IDialog SimpleOkDialog()
    {
        var label = new TextDialogItem("This is your message");
        var button = new ButtonDialogItem("OK");
        var step = new DialogStepPrototype(RandomDialogImage(), label, button);
        var d = new Dialog(step);
        return d;
    }

    private static IDialog NumericChoiceDialog()
    {
        var label = new TextDialogItem("This is your message");
        var choice = new NumericDialogItem("Choose your effort", 0, 10, 0);
        var button = new ButtonDialogItem("OK");
        var step = new DialogStepPrototype(RandomDialogImage(), label, choice, button);
        var d = new Dialog(step);
        return d;
    }

    private static IDialog LayeredDialog()
    {
        var accept = new TextDialogItem("This is accept message");
        var decline = new TextDialogItem("This is decline message");
        var okButton = new ButtonDialogItem("OK");
        var acceptStep = new DialogStepPrototype(RandomDialogImage(), accept, okButton);
        var declineStep = new DialogStepPrototype(RandomDialogImage(), decline, okButton);
        
        var label = new TextDialogItem("This is your message");
        var choice = new NumericDialogItem("Choose your effort", 0, 10, 0);
        var button1 = new ButtonDialogItem("Accept", acceptStep);
        var button2 = new ButtonDialogItem("Decline", declineStep);
        var step = new DialogStepPrototype(RandomDialogImage(), label, choice, button1, button2);
        var dialog = new Dialog(step);
        return dialog;        
    }
    
    public static IDialog[] Dialogs { get; }

    private static string RandomDialogImage()
    {
        return MockImages.DialogImages[UiMockPresenter.Random.Next() % MockImages.DialogImages.Length];
    }
}