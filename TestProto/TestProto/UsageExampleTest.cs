using System.Text;
using Prototype;
using Prototype.Stub;

namespace TestProto;

public class UsageExampleTest
{
    [Fact]
    public void Main_Game_Cycle()
    {
        var renderer = StubGameFacade.GetRenderer();
        var mockPresenter = new UiMockPresenter();
        var log = new StringBuilder();

        while (true)
        {
            if (renderer.DialogScreen != null)
            {
                var res = mockPresenter.ShowDialog(renderer.DialogScreen.PendingDialog!);
                renderer = renderer.DialogScreen.DialogComplete(res);
                log.AppendLine($"{renderer.PlayerData.Age}: Dialog result: {res}");
            }
            else if (renderer.WaitingScreen != null)
            {
                var res = mockPresenter.Wait(renderer.PlayerData.Age, renderer.WaitingScreen.WaitingData!);
                renderer = renderer.WaitingScreen.WaitUntil(res);
            }
            else if (renderer.WorldScreen != null)
            {
                var result = mockPresenter.ShowWorldDialog(renderer.WorldScreen.WorldData, renderer.WorldScreen.PlayerData);
                log.AppendLine($"{renderer.PlayerData.Age}: World result {result}");
                renderer = renderer.WorldScreen.RecordWorldDialogResult(result);
            }
            else if (renderer.EndOfGameScreen != null)
            {
                mockPresenter.ShowEndOfGame(renderer.EndOfGameScreen.Data!);
                log.AppendLine($"{renderer.PlayerData.Age}: Game over");
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