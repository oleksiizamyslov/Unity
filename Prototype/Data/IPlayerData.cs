namespace Prototype;

public interface IPlayerData
{
    Age Age { get; }
    int Health { get; }
    int Money { get; }
    int Spirit { get; }
    string State { get; }
}