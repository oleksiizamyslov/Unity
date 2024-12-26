using System.Text;
using Prototype;
using Prototype.Stub;

namespace TestProto;

public class UsageExampleTest
{
    [Fact]
    public void Main_Game_Cycle()
    {
        var gf = StubGameFacade.GetFacade();
        var mockPresenter = new UiMockPresenter();
        var log = new StringBuilder();

        while (true)
        {
            if (gf.State == State.InDialog)
            {
                var res = mockPresenter.ShowDialog(gf.PendingDialog!);
                gf.RecordDialogResult(res);
                log.AppendLine($"{gf.PlayerData.Age}: Dialog result: {res}");
            }
            else if (gf.State == State.InWaiting)
            {
                var res = mockPresenter.Wait(gf.PlayerData.Age, gf.WaitingData!, gf.GetWorldData);
                if (res.worldDialogResult != null)
                {
                    gf.RecordWorldDialogResult(res.worldDialogResult, res.age);
                    log.AppendLine($"{gf.PlayerData.Age}: World result at {res.age}: {res.worldDialogResult}");
                }
                else
                {
                    gf.RecordWaitedUntil(res.age);
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
    
}