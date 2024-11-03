using Godot;
using System;
using System.Collections.Generic;



public partial class TileMapSelect : TileMap
{
	// Called when the node enters the scene tree for the first time.
	int gridSize = 10;

	// For tiles on the map
	Dictionary<Vector2I, int> MapDict = new Dictionary<Vector2I, int>();
	
	// For placeable buildings
	Dictionary<Vector2I, string> BuildableDict = new Dictionary<Vector2I, string>();


	private PackedScene _beltScene = (PackedScene)GD.Load("res://Objects/conv_belt.tscn");
	private string _beltATexture = "";
	private PackedScene _converter = (PackedScene)GD.Load("res://Objects/machines.tscn");
	private string _converterTexture = "";

	private Node currentEquiped;
	private PackedScene currentEquipedScene;

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
		
		// Change current held buildable
		if (Input.IsActionJustPressed("Hotbar_1"))
		{
			SwapHeldBuildable(_beltScene);
		}

		if (Input.IsActionJustPressed("Hotbar_2"))
		{	
			SwapHeldBuildable(_converter);
		}

		// Place equiped buildable
		if (Input.IsActionPressed("Left_Click"))
		{
			if (currentEquipedScene != null)
			{
				MakeInstancedObject(currentEquipedScene, "Area2D/AnimatedSprite2D");
			}
		}

		// Remove hovered over buildable
		if (Input.IsActionPressed("Right_Click"))
		{
			RemoveInstancedObject();
		}

		// Move current equiped buildable sprite to cursor
		if (currentEquiped != null)
		{
			currentEquiped.GetNode<AnimatedSprite2D>("Area2D/AnimatedSprite2D").Position = GetGlobalMousePosition();
			
		}

		// Outline grid when hovered over
		var tile = LocalToMap(GetGlobalMousePosition());

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
		/*
		// Place down equiped buildable
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			if (currentEquipedScene != null)
			{
				MakeInstancedObject(currentEquipedScene, "Area2D/AnimatedSprite2D");
			}
    	}	
		*/

	}

	// Places an instanced copy of a buildable object on tilemap, if possible
	private void MakeInstancedObject(PackedScene Instance, string TexturePath)
	{
		// Check if buildable can be placed
		// TODO Need to figure out how to handle larger buildings later
		var tilePosition = LocalToMap(GetGlobalMousePosition());
		if (BuildableDict.ContainsKey(tilePosition)){
			return;
		}

		// Create and place new instance
		Node Item = Instance.Instantiate();
		AddChild(Item);
		Item.GetNode<AnimatedSprite2D>(TexturePath).Play();
		Item.GetNode<AnimatedSprite2D>(TexturePath).Position = tilePosition * 32;
		BuildableDict.Add(tilePosition, Item.Name);
	}
 
	// Destroy placed object
	private void RemoveInstancedObject()
	{
		var tilePosition = LocalToMap(GetGlobalMousePosition());
		if (BuildableDict.ContainsKey(tilePosition)){
			string nodeName = "";
			BuildableDict.TryGetValue(tilePosition, out nodeName);

			// Destroy buildable
			Node Buildable = GetNodeOrNull(nodeName);
			RemoveChild(Buildable);
			Buildable.QueueFree();
			// Clear Dict entry to mark buildable as removed
			BuildableDict.Remove(tilePosition);
		}
	}

	// Changes currently equiped buildable, removing the previous item held
	private void SwapHeldBuildable(PackedScene NewEquipedScene)
	{
		// Check if an item is already equiped, and destroy it
		Node old = GetNodeOrNull("Equiped");
			if(old != null) 
			{
				RemoveChild(old);
				old.QueueFree();
			}
			currentEquiped = NewEquipedScene.Instantiate();
			currentEquipedScene = NewEquipedScene;
			AddChild(currentEquiped);
			currentEquiped.Name = "Equiped";
	}
}


