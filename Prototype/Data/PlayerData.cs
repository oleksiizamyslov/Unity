namespace Prototype;

public class PlayerData : IPlayerData
{
    public PlayerData(Age age, int health, int money, int spirit, string state)
    {
        Age = age;
        Health = health;
        Money = money;
        Spirit = spirit;
        State = state;
    }

    public Age Age { get; set; }
    public int Health { get; }
    public int Money { get; }
    public int Spirit { get; }
    public string State { get; }
}