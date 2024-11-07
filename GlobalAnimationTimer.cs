using Godot;
using System;

public partial class GlobalAnimationTimer : Node2D
{
	public double count;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		count = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		count += delta;
		count = count%6;
	}
}
