using Assets.Scenes.Scripts.Server;

public interface IServerFactory
{
    public IGameCreator MakeGameCreator();
    public IGameJoiner MakeGameJoiner();
    public IGamesReader MakeGamesReader();
    public INetworkGameManager MakeNetworkGameManager();
    public IServerListener MakeServerListener();
}