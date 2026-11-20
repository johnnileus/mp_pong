using Godot;
using System;

public partial class Ball : CharacterBody3D
{
	[Export] private float _speed = 10.0f;

	private Vector3 _dir = new Vector3();
	private bool _running;
	
	public void Start(){
		_running = true;
		Velocity = Vector3.Forward.Rotated(Vector3.Up, GD.Randf() * Mathf.Tau) * _speed;
	}

	public override void _PhysicsProcess(double delta) {
		
		if (!Multiplayer.IsServer() || !_running)
			return;
		
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
			Velocity = Velocity.Bounce(collision.GetNormal());
		
		if (Multiplayer.GetPeers().Length > 0)
			Rpc(MethodName.SyncBall, GlobalPosition);
	}
	
	[Rpc(MultiplayerApi.RpcMode.Authority, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	private void SyncBall(Vector3 pos)
	{
		GlobalPosition = pos;
	}
	
}
