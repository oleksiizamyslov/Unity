namespace Prototype;

public class AgedImage
{
    public AgedImage(string imageUrl, Age showUntil)
    {
        ImageUrl = imageUrl;
        ShowUntil = showUntil;
    }

    public string ImageUrl { get; }
    public Age ShowUntil { get; }
}