using Godot;
using System;



public partial class Animations : AnimationPlayer
{
	[Signal]
	public delegate void SpriteNextFrameEventHandler();
	// Called when the node enters the scene tree for the first time.
	private double timeCounter;
	public override void _Ready()
	{
		timeCounter = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timeCounter = timeCounter + delta;
		//GD.Print(timeCounter);
		if (timeCounter > 1)
		{
			EmitSignal(nameof(SpriteNextFrame));
			timeCounter = timeCounter%(1);
		}
	}
}
