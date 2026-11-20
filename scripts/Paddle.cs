using Godot;
using System;

public partial class Paddle : Node3D{

	[Export] public bool Active;
	
	[Export] private Node3D _left;
	[Export] private Node3D _right;
	[Export] private float _speed;

	private float _pos; // 0 - 1
	private bool _wasMoving;

	[Export] private CharacterBody3D _paddle;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		_pos = .5f;
		UpdatePaddlePosition();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta){
		
		if (!Active || !IsMultiplayerAuthority())
			return; // the other player's paddle, position comes from the synchronizer

		
		float dir = Input.GetAxis("player_left", "player_right");
		bool moving = !Mathf.IsZeroApprox(dir);
		if (moving)
			_pos = Mathf.Clamp(_pos + dir * _speed * (float)delta, 0, 1);
		UpdatePaddlePosition();
		
		
		if (Multiplayer.GetPeers().Length == 0)
			return;
		
		if (moving) {
			Rpc(MethodName.SyncPos, _pos);                 // unreliable, every tick while moving
		}

	}

	private void UpdatePaddlePosition(){
		_paddle.GlobalPosition = _left.GlobalPosition.Lerp(_right.GlobalPosition, _pos);
	}
	
	[Rpc(MultiplayerApi.RpcMode.Authority, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	private void SyncPos(float pos)
	{
		_pos = pos;
		UpdatePaddlePosition();
	}
	
}
