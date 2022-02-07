public class StaticMapChunks
{

	private StaticMapChunks () { }

	public static MapChunk GetMapChunk1()
	{
		MapChunk mapChunk = new MapChunk();

		int[,] floors = new int[16,16]
		{
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			{1,0,0,0,0,1,1,1,1,1,1,0,0,0,0,1},
			{1,0,1,1,0,1,1,1,1,1,1,0,1,1,0,1},
			{1,0,1,1,0,1,1,1,1,1,1,0,1,1,0,1},
			{1,0,0,0,0,1,1,1,1,1,1,0,0,0,0,1},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			{1,0,0,0,0,1,1,1,1,1,1,0,0,0,0,1},
			{1,0,1,1,0,1,1,1,1,1,1,0,1,1,0,1},
			{1,0,1,1,0,1,1,1,1,1,1,0,1,1,0,1},
			{1,0,0,0,0,1,1,1,1,1,1,0,0,0,0,1},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
		};

		int[,] walls = new int[16,16]
		{
			{1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,1},
			{1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1},
			{1,0,1,1,0,1,0,0,0,0,1,0,1,1,0,1},
			{1,0,1,0,0,1,0,0,0,0,1,0,0,1,0,1},
			{1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1},
			{1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,1},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,1},
			{1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1},
			{1,0,1,0,0,1,0,0,0,0,1,0,0,1,0,1},
			{1,0,1,1,0,1,0,0,0,0,1,0,1,1,0,1},
			{1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1},
			{1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,1},
		};

		AddFloors(floors, mapChunk);
		AddWalls(walls, mapChunk);

		return mapChunk;
	}

	public static MapChunk GetMapChunk2(Random rng)
	{
		MapChunk mapChunk = new MapChunk();

		int[,] walls = new int[16,16]
		{
			{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0},
			{0,0,1,0,1,0,0,1,0,0,0,0,0,0,0,0},
			{0,0,1,0,1,0,0,1,0,0,0,0,0,0,0,0},
			{0,0,1,0,1,0,0,1,1,1,0,1,1,1,0,0},
			{0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0},
			{0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0},
			{0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0},
			{0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0},
			{0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0},
			{0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0},
			{0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
		};

		FillFloors(mapChunk);

		int c = rng.Next(0, 4);
		if (c == 0)
		{
			int[,] walls2 = new int[16,16];

			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					walls2[j,i] = walls[15-i,15-j];
				}
			}

			AddWalls(walls2, mapChunk);
		}
		else if (c == 1)
		{
			int[,] walls2 = new int[16,16];

			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					if (rng.Next(0, 100) < 80)
					{
						walls2[i,j] = walls[i,j];
					}
				}
			}

			AddWalls(walls2, mapChunk);
		}
		else if (c == 2)
		{
			int[,] walls2 = new int[16,16];

			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					walls2[i,j] = walls[15-i,15-j];
				}
			}

			AddWalls(walls2, mapChunk);
		}
		else if (c == 3)
		{
			int[,] walls2 = new int[16,16];

			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					walls2[15-i,15-j] = walls[i,j];
				}
			}

			AddWalls(walls2, mapChunk);
		}
		else
		{
			AddWalls(walls, mapChunk);
		}

		return mapChunk;
	}

	public static MapChunk GetMapChunk3()
	{
		MapChunk mapChunk = new MapChunk();

		int[,] walls = new int[16,16]
		{
			{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,1,1,1,1,0,0,0,0,1,1,1,1,0,0},
			{0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0},
			{0,0,1,0,0,0,0,1,1,0,0,0,0,1,0,0},
			{0,0,1,0,0,0,1,1,1,1,0,0,0,1,0,0},
			{0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0},
			{0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0},
			{0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0},
			{0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0},
			{0,0,1,0,0,0,1,1,1,1,0,0,0,1,0,0},
			{0,0,1,0,0,0,0,1,1,0,0,0,0,1,0,0},
			{0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0},
			{0,0,1,1,1,1,0,0,0,0,1,1,1,1,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
		};

		FillFloors(mapChunk);
		AddWalls(walls, mapChunk);

		return mapChunk;
	}

	public static MapChunk GetMapChunk4(Random rng)
	{
		MapChunk mapChunk = new MapChunk();

		FillFloors(mapChunk);

		for(int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				if (rng.Next(0, 100) < 5)
				{
					Wall wall = new Wall(j, i);
					mapChunk.AddWall(wall, wall.Position);
				}
			}
		}

		return mapChunk;
	}

	public static MapChunk GetMapChunk5(Random rng)
	{
		MapChunk mapChunk = new MapChunk();

		FillFloors(mapChunk);

		return mapChunk;
	}

	private static int[,] GetBlankInts()
	{
		int[,] ints = new int[16,16]
		{
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		};

		return ints;
	}

	private static void AddFloors(int[,] floors, MapChunk mapChunk)
	{
		for(int i = 0; i < floors.GetLength(0); i++)
		{
			for (int j = 0; j < floors.GetLength(1); j++)
			{
				if (floors[i,j] == 1)
				{
					Floor floor = new Floor(j, i);
					mapChunk.AddFloor(floor, floor.Position);
				}
			}
		}
	}

	private static void AddWalls(int[,] walls, MapChunk mapChunk)
	{
		for(int i = 0; i < walls.GetLength(0); i++)
		{
			for (int j = 0; j < walls.GetLength(1); j++)
			{
				if (walls[i,j] == 1)
				{
					Wall wall = new Wall(j, i);
					mapChunk.AddWall(wall, wall.Position);
				}
			}
		}
	}

	private static void FillFloors(MapChunk mapChunk)
	{
		for(int i = 0; i < mapChunk.Height; i++)
		{
			for(int j = 0; j < mapChunk.Width; j++)
			{
				Floor floor = new Floor(j, i);
				mapChunk.AddFloor(floor, floor.Position);
			}
		}
	}

	private static void FillWalls(MapChunk mapChunk)
	{
		for(int i = 0; i < mapChunk.Height; i++)
		{
			for(int j = 0; j < mapChunk.Width; j++)
			{
				Wall wall = new Wall(j, i);
				mapChunk.AddWall(wall, wall.Position);
			}
		}
	}

}

