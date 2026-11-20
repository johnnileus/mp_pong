using Godot;
using System;

public partial class NetworkMenu : Control{

	[Export] private Button _hostButton;
	[Export] private Button _joinButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		_hostButton.ButtonUp += OnHostButton;
		_joinButton.ButtonUp += OnJoinButton;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnHostButton(){
		GameManager.Instance.Host();
	}

	public void OnJoinButton(){
		GameManager.Instance.Join("127.0.0.1");
	}
	
}
