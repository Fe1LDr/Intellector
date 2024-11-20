public class GameInfo
{
    private static GameInfo _instance;

    public uint ID { get; set; }
    public string Name { get; set; }
    public TimeContol TimeContol { get; set; }
    public ColorChoice Color { get; set; }
    public bool Team { get; set; }

    public void Save()
    {
        _instance = this;
    }

    public static GameInfo Load()
    {
        return _instance ?? new();
    }
}
