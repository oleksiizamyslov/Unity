namespace Prototype;

public class EndOfGameData : IEndOfGameData
{
    public EndOfGameData(string imageUrl, string text)
    {
        ImageUrl = imageUrl;
        Text = text;
    }

    public string ImageUrl { get; }
    public string Text { get; }
}