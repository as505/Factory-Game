using Godot;
using System;



public partial class Animations : AnimationPlayer
{
	[Signal]
	public delegate void SpriteNextFrameEventHandler();
	private double timeCounter = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timeCounter += delta;
		if (timeCounter > 1/6)
		{
			GD.Print(timeCounter);
			EmitSignal(nameof(SpriteNextFrame));
			timeCounter = timeCounter%(1/6);
		}
	}
	private void foo ()
	{
		return;
	}
}
