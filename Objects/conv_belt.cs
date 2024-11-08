using Godot;
using System;

public partial class conv_belt : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var anim = GetNode("/root/Map/Animations");
        anim.Connect(nameof(Animations.SpriteNextFrame),  new Callable(this, MethodName.Increment));
		//anim.Connect("SpriteNextFrame", this, "Increment");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void Increment(int frame)
	{
		var sprite = GetNode<AnimatedSprite2D>("Area2D/AnimatedSprite2D");
		sprite.Frame = frame;
	}
}
