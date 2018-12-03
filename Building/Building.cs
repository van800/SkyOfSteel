using Godot;


public class Building : Node
{
	private static BuildPositions BuildPositionsInstance;
	private static BuildRotations BuildRotationsInstance;

	private static Dictionary<Items.TYPE, PackedScene> Scenes = new Dictionary<Items.TYPE, PackedScene>();

	private static Building Self;

	Building()
	{
		Self = this;

		Directory StructureDir = new Directory();
		StructureDir.Open("res://Building/Scenes/");
		StructureDir.ListDirBegin(true, true);
		string FileName = StructureDir.GetNext();
		while(true)
		{
			if(FileName == "")
			{
				break;
			}
			PackedScene Scene = GD.Load("res://Building/Scenes/"+FileName) as PackedScene;
			if((Scene.Instance() as Structure) == null)
			{
				throw new System.Exception("Structure scene '" + FileName + "' does not inherit Structure");
			}

			FileName = StructureDir.GetNext();
		}

		foreach(Items.TYPE Type in System.Enum.GetValues(typeof(Items.TYPE)))
		{
			File ToLoad = new File();
			if(ToLoad.FileExists("res://Building/Scenes/" + Type.ToString() + ".tscn"))
			{
				Scenes.Add(Type, GD.Load("res://Building/Scenes/" + Type.ToString() + ".tscn") as PackedScene);
			}
			else
			{
				Scenes.Add(Type, GD.Load("res://Building/Scenes/ERROR.tscn") as PackedScene);
			}
		}

		BuildPositionsInstance = new BuildPositions();
		BuildRotationsInstance = new BuildRotations();
	}


	public static void Request(Structure Base, Items.TYPE BranchType, int OwnerId)
	{
		System.Nullable<Vector3> Position = BuildPositionsInstance.Calculate(Base, BranchType);
		Vector3 Rotation = BuildRotationsInstance.Calculate(Base, BranchType);

		if(Position != null)
		{
			Perform.PlaceRequest(Events.INVOKER.CLIENT, OwnerId, BranchType, (Vector3)Position, Rotation);
		}
	}


	public static void Place(Items.TYPE BranchType, Vector3 Position, Vector3 Rotation, int OwnerId, string Name = null)
	{
		Structure Branch = Scenes[BranchType].Instance() as Structure;
		Branch.Type = BranchType;
		Branch.OwnerId = OwnerId;
		Branch.Translation = Position;
		Branch.RotationDegrees = Rotation;

		if(Name == null)
		{
			Name = System.Guid.NewGuid().ToString();
		}

		Branch.SetName(Name); //name can be used to reference a structure over network
		Game.StructureRoot.AddChild(Branch);

		if(Self.GetTree().GetNetworkPeer() != null && Self.GetTree().GetNetworkUniqueId() == 1)
		{
			Message.NetPlaceSync(BranchType, Position, Rotation, OwnerId, Name);
		}
	}


	//Name is the string GUID name of the structure to be removed
	public static void Remove(string Name)
	{
		Structure Branch = Game.StructureRoot.GetNode(Name) as Structure;
		Branch.QueueFree();
	}
}