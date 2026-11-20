using Godot;

public partial class GameManager : Node3D
{
    
    #region Singleton

    public static GameManager Instance {private set; get;}
    public override void _EnterTree() => Instance = this;

    #endregion
    
    [Export] public Paddle Paddle2;
    [Export] public Ball Ball;
    
    private const int Port = 7777;

    public override void _Ready() {
        Multiplayer.PeerConnected += OnPeerConnected;           // fires on the host
        Multiplayer.ConnectedToServer += OnConnectedToServer;   // fires on the client
    }

    private void OnPeerConnected(long id) {
        if (!Multiplayer.IsServer()) return;

        Paddle2.SetMultiplayerAuthority((int)id);
        Paddle2.Active = true;
        GD.Print($"HOST: Paddle2 authority is now {Paddle2.GetMultiplayerAuthority()}, expected {id}");
        Ball.Start();
    }

    private void OnConnectedToServer() {
        Paddle2.SetMultiplayerAuthority(Multiplayer.GetUniqueId());
        Paddle2.Active = true;
        GD.Print($"CLIENT: Paddle2 authority is now {Paddle2.GetMultiplayerAuthority()}, expected {Multiplayer.GetUniqueId()}");
    }


    public void Host(){
        var peer = new ENetMultiplayerPeer();
        peer.CreateServer(Port, maxClients: 1);
        Multiplayer.MultiplayerPeer = peer;
        Logger.Info("Host server started");
    }

    public void Join(string address) {
        var peer = new ENetMultiplayerPeer();
        peer.CreateClient(address, Port);
        Multiplayer.MultiplayerPeer = peer;
        Logger.Info("Joined server " + address);
    }
    
}