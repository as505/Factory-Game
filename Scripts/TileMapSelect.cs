using Godot;
using System;
using System.Collections.Generic;



public partial class TileMapSelect : TileMap
{
	// Called when the node enters the scene tree for the first time.
	int gridSize = 10;

	Dictionary<Vector2I, int> MapDict = new Dictionary<Vector2I, int>();
	private PackedScene _beltScene = (PackedScene)GD.Load("res://Objects/conv_belt.tscn");
	private string _beltATexture = "";
	private PackedScene _converter = (PackedScene)GD.Load("res://Objects/machines.tscn");
	private string _converterTexture = "";

	private PackedScene currentEquiped;

	public override void _Ready()
	{
		/*
		Node belt = _beltScene.Instantiate();
		AddChild(belt);
		belt.GetNode<AnimatedSprite2D>("Area2D/AnimatedSprite2D").Play();
		*/
		//var button = GetNode<TextureButton>("Control/TextureButton");
		//button.Pressed += OnButtonPressed;
		
		for (int x = 0; x < gridSize; x++)
		{
			for (int y = 0; y < gridSize; y++)
			{ 
				var coords = new Vector2I(x, y);
				var atlas_coords = new Vector2I(0,0);
				SetCell(0, coords, 0, atlas_coords, 0);
				MapDict.Add(coords, 000);
			}
		}
		
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Keypresses
		if (Input.IsActionJustPressed("Hotbar_1"))
		{
			GD.Print("1");
			currentEquiped = _beltScene;
		}

		if (Input.IsActionJustPressed("Hotbar_2"))
		{
			GD.Print("2");
			currentEquiped = _converter;
		}
		//var map = GetNode("res://map.tscn");
		var tile = LocalToMap(GetGlobalMousePosition());

		Node belt = GetNodeOrNull("uniqueName");
		if (belt == null){
			belt = _beltScene.Instantiate();
			belt.Name = "uniqueName";
			AddChild(belt);
			belt.GetNode<AnimatedSprite2D>("Area2D/AnimatedSprite2D").Position = GetGlobalMousePosition();
		} else {
			belt.GetNode<AnimatedSprite2D>("Area2D/AnimatedSprite2D").Position = GetGlobalMousePosition();
		}

		
		

		for (int x = 0; x < gridSize; x++)
		{
			for (int y = 0; y < gridSize; y++)
			{
				EraseCell(1, new Vector2I(x, y));
			}
		}

		if (MapDict.ContainsKey(tile))
		{
			SetCell(1, tile, 1, new Vector2I(0,0), 0);

			
		}
	}

    public override void _Input(InputEvent @event)
    {	
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			/*
			belt.GetNode<AnimatedSprite2D>("Area2D/AnimatedSprite2D").Play();
			*/
			if (currentEquiped != null)
			{
				MakeInstancedObject(currentEquiped, "Area2D/AnimatedSprite2D");
			}
    	}	

	}

	private void MakeInstancedObject(PackedScene Item, string TexturePath)
	{
		Node item_instance = Item.Instantiate();
		AddChild(item_instance);
		item_instance.GetNode<AnimatedSprite2D>(TexturePath).Play();
		var tilePosition = LocalToMap(GetGlobalMousePosition());
		item_instance.GetNode<AnimatedSprite2D>(TexturePath).Position = tilePosition * 32;
	}
}
