using System.Text;
using Prototype;
using Prototype.Stub;

namespace TestProto;

public class UsageExampleTest
{
    [Fact]
    public void Main_Game_Cycle()
    {
        var gf = StubGameFacade.GetRenderer();
        var mockPresenter = new UiMockPresenter();
        var log = new StringBuilder();

        while (true)
        {
            if (gf.DialogScreen != null)
            {
                var res = mockPresenter.ShowDialog(gf.DialogScreen.PendingDialog!);
                gf = gf.DialogScreen.DialogComplete(res);
                log.AppendLine($"{gf.PlayerData.Age}: Dialog result: {res}");
            }
            else if (gf.WaitingScreen != null)
            {
                var res = mockPresenter.Wait(gf.PlayerData.Age, gf.WaitingScreen.WaitingData!);
                gf = gf.WaitingScreen.WaitUntil(res);
            }
            else if (gf.WorldScreen != null)
            {
                var result = mockPresenter.ShowWorldDialog(gf.WorldScreen.WorldData, gf.WorldScreen.PlayerData);
                log.AppendLine($"{gf.PlayerData.Age}: World result {result}");
                gf = gf.WorldScreen.RecordWorldDialogResult(result);
            }
            else if (gf.EndOfGameScreen != null)
            {
                mockPresenter.ShowEndOfGame(gf.EndOfGameScreen.Data!);
                log.AppendLine($"{gf.PlayerData.Age}: Game over");
                break;
            }
            else
            {
                throw new InvalidDataException();
            }
        }

        Console.WriteLine(log.ToString());
    }
    
}