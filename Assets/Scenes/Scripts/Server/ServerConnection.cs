using System.Net.Sockets;
using static Networking;

public class ServerConnection
{
    public TcpClient Client { get; private set; }

    private static ServerConnection Instance;

    private ServerConnection(TcpClient client)
    {
        Client = client;
    }

    public void Close() => Client.Close();

    public static ServerConnection GetConnection()
    {
        if (Instance == null || !Instance.Client.Connected) Instance = new ServerConnection(ConnectToServer());
        return Instance;
    }

    private static TcpClient ConnectToServer()
    {
        Connection connection = Settings.GetConnection();
        TcpClient client = new TcpClient(connection.ServerIP, connection.Port);

        SendString(connection.Password, client.GetStream());
        CheckVersion(client.GetStream());

        return client;
    }

    private static void CheckVersion(NetworkStream stream)
    {
        SendInt(Settings.Version, stream);
        int server_version = RecvInt(stream);
        if (Settings.Version != server_version)
        {
            throw new VersionException(
             $"\"������������ ������\n" +
             $"������ ������� - {VerToStr(server_version)}\n" +
             $"������������ ������ ������� - {VerToStr(Settings.Version)}\""
             );
        }

        string VerToStr(int ver) => $"{ver / 10}.{ver % 10}";
    }
}
