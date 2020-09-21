using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Room
{

	public Vector3 gridPos;
	public int type;
	public bool doorTop, doorBot, doorLeft, doorRight;

	public Room(Vector3 _gridPos, int _type)
	{
		gridPos = _gridPos;
		type = _type;
	}

}

public class MapManager : MonoBehaviour
{
	Vector3 worldSize = new Vector3(30, 0, 30);
	Room[,] rooms;

	List<NavMeshSurface> navMeshSurfaces;

	List<Vector3> takenPositions = new List<Vector3>();
	int gridSizeX, gridSizeZ, numberOfRooms = 10;
	public GameObject roomWhiteObj, Player;

	void Start()
	{
		if (numberOfRooms >= (worldSize.x * 2) * (worldSize.z * 2))
		{
			numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.z * 2));
		}

		gridSizeX = Mathf.RoundToInt(worldSize.x);
		gridSizeZ = Mathf.RoundToInt(worldSize.z);
		CreateRooms();
		SetRoomDoors();
		DrawMap();
	}
	void CreateRooms()
	{
		rooms = new Room[gridSizeX * 2, gridSizeZ * 2];
		rooms[gridSizeX, gridSizeZ] = new Room(Vector3.zero, 1);
		takenPositions.Insert(0, Vector3.zero);

		Vector3 checkPos = Vector3.zero;
		float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
		
		for (int i = 0; i < numberOfRooms - 1; i++)
		{
			float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
			randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

			checkPos = NewPosition();

			if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
			{
				int iterations = 0;
				do
				{
					checkPos = SelectiveNewPosition();
					iterations++;
				} while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
				
				if (iterations >= 50) print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
			}

			rooms[(int)checkPos.x + gridSizeX, (int)checkPos.z + gridSizeZ] = new Room(checkPos, 0);
			takenPositions.Insert(0, checkPos);
		}
	}
	Vector3 NewPosition()
	{
		float x = 0, z = 0;
		Vector3 checkingPos = Vector3.zero;
		do
		{
			int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
			x = (int)takenPositions[index].x;
			z = (int)takenPositions[index].z;
			bool UpDown = (Random.value < 0.5f);
			bool positive = (Random.value < 0.5f);

			if (UpDown)
			{
				z = (positive) ? z + 1 : z - 1;
			} else { 
				x = (positive) ? x + 1 : x - 1;
			}

			checkingPos = new Vector3(x, 0f, z);

		} while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || z >= gridSizeZ || z < -gridSizeZ);
		return checkingPos;
	}
	Vector3 SelectiveNewPosition()
	{
		int index = 0, inc = 0;
		float x = 0, z = 0;
		Vector3 checkingPos = Vector3.zero;
		do
		{
			inc = 0;
			do
			{
				index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				inc++;
			} while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);

			x = (int)takenPositions[index].x;
			z = (int)takenPositions[index].z;
			
			bool UpDown = (Random.value < 0.5f);
			bool positive = (Random.value < 0.5f);

			if (UpDown)
			{
				z = (positive) ? z + 1 : z - 1;
			}
			else
			{
				x = (positive) ? x + 1 : x - 1;
			}

			checkingPos = new Vector3(x, 0f, z);

		} while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || z >= gridSizeZ || z < -gridSizeZ);

		if (inc >= 100) print("Error: could not find position with only one neighbor");
		return checkingPos;
	}
	int NumberOfNeighbors(Vector3 checkingPos, List<Vector3> usedPositions)
	{
		int ret = 0;
		if (usedPositions.Contains(checkingPos + Vector3.right)) ret++;
		if (usedPositions.Contains(checkingPos + Vector3.left)) ret++;
		if (usedPositions.Contains(checkingPos + Vector3.up)) ret++;
		if (usedPositions.Contains(checkingPos + Vector3.down)) ret++;
		return ret;
	}
	void DrawMap()
	{
		int count = 0;
		foreach (Room room in rooms)
		{
			if (room == null) continue;

			Vector3 drawPos = room.gridPos;

			drawPos.x *= 10;
			drawPos.y = 0;
			drawPos.z *= 10;

			GameObject settObject = Instantiate(roomWhiteObj, drawPos, Quaternion.identity);
			RoomController obj = settObject.GetComponent<RoomController>();

			obj.doorTop = room.doorTop;
			obj.doorBot = room.doorBot;
			obj.doorRight = room.doorRight;
			obj.doorLeft = room.doorLeft;

			if (count == 1)
			{
				Instantiate(Player, obj.PlayerSpawnPosition, false);
			}
			count++;

		}
	}
	void SetRoomDoors()
	{
		for (int x = 0; x < ((gridSizeX * 2)); x++)
		{
			for (int z = 0; z < ((gridSizeZ * 2)); z++)
			{
				if (rooms[x, z] == null) continue;

				rooms[x, z].doorBot		= (x - 1 < 0)				? true : (rooms[x - 1, z] == null);
				rooms[x, z].doorTop		= (x + 1 >= gridSizeX * 2)	? true : (rooms[x + 1, z] == null);
				rooms[x, z].doorLeft	= (z - 1 < 0)				? true : (rooms[x, z - 1] == null);
				rooms[x, z].doorRight	= (z + 1 >= gridSizeZ * 2)	? true : (rooms[x, z + 1] == null);

			}
		}
	}
}
