using SRNG;
using System.Collections.Generic;

public class TileMapData
{

	//this is a workaround because of an error in monodevelope 
	private class RoomList : List<RoomData>
	{
	}


	TYPE[,] mapData;
	public int mapWidth, mapHeight;
	RoomList roomList;


	//default constructor
	public TileMapData (): this(20,20)
	{}

	//constructor generally used
	public TileMapData (int a_iWidth, int a_iHeight)
	{
		SimpleRNG.SetSeed(1337);
		mapWidth = a_iWidth;
		mapHeight = a_iHeight;

		mapData = new TYPE[mapWidth, mapHeight];


		//set the entiremap to "stone"
		for (int x = 0; x < mapWidth; x++)
			for (int y = 0; y < mapHeight; y++) {
				mapData [x, y] = TYPE.STONE;
			}	


		roomList = new RoomList ();
				
		int failCount = 10;
				
		while (roomList.Count < 10) {
			RoomData room = new RoomData ();
			int rsx = (int)SimpleRNG.GetNormal() * 4+ 10;
			int rsy = (int)SimpleRNG.GetNormal() * 4+ 8;

			room.m_iLeft = (int)SimpleRNG.GetNormal()  *(mapWidth - rsx);
			room.m_iTop = (int)SimpleRNG.GetNormal()  *(mapWidth - rsy);
			room.m_iWidth = rsx;
			room.m_iHeight = rsy;
						
			if (roomList.Count == 0) {

				roomList.Add (room);
			} else if (!RoomCollides (room)) {
				roomList.Add (room);

			} else {
				failCount--;
				if (failCount <= 0)
					break;
			}
							
					
		}
		for(int i = 0; i <roomList.Count; i++) {
			MakeRoom (roomList[i]);
		}

		for(int i = 0; i <roomList.Count; i++) {
			if (!roomList[i].Connected) {
				int j = 1 + ((int)SimpleRNG.GetNormal()  * roomList.Count);

				MakeCorridor (roomList [i], roomList [(i+j) % roomList.Count]);
		
			}
		}

		MakeWalls();

	}

	public TYPE GetTile (int a_iX, int a_iY)
	{
		if (a_iX < 0 || a_iX >= mapWidth || a_iY < 0 || a_iY >= mapHeight) {
			return TYPE.UNKNOWN;
		}
		return mapData [a_iX, a_iY];
	}

	void MakeRoom (RoomData room)
	{
		for (int x = 0; x < room.m_iWidth; x++)
			for (int y = 0; y < room.m_iHeight; y++) {
				if (x == 0 || x == room.m_iWidth - 1 || y == 0 || y == room.m_iHeight - 1)
					mapData [room.m_iLeft + x, room.m_iTop + y] = TYPE.WALL;
				else
					mapData [room.m_iLeft + x, room.m_iTop + y] = TYPE.FLOOR;

			}

	}

	void MakeCorridor (RoomData r1, RoomData r2)
	{
		int x = r1.m_iCenterX;
		int y = r1.m_iCenterY;
		while (x != r2.m_iCenterX) {
		
			mapData [x, y] = TYPE.FLOOR;
			x += x < r2.m_iCenterX ? 1 : -1;
				
		}
		while (y != r2.m_iCenterY) {
			
			mapData [x, y] = TYPE.FLOOR;
			y += y < r2.m_iCenterY ? 1 : -1;
			
		}
		r1.Connected = true;
		r2.Connected = true;

	}
	void MakeWalls()
	{
		for(int x = 0; x < mapWidth ; x++) {
			for(int y = 0; y < mapHeight; y++) {
				if(mapData[x,y] == TYPE.STONE && HasAdjacentFloor(x,y))
					mapData[x,y] = TYPE.WALL;
			}
		}

	}
	bool HasAdjacentFloor(int x, int y)
	{	
		//n,s,e,w
		if(x > 0 && mapData[x-1,y] == TYPE.FLOOR)
			return true;
		if(x < mapWidth-1 && mapData[x+1,y] == TYPE.FLOOR)
			return true;
		if(y > 0 && mapData[x,y-1] == TYPE.FLOOR)
			return true;
		if(y < mapHeight-1 && mapData[x,y+1] == TYPE.FLOOR)
			return true;

		//nw,ne,sw,se
		if(x > 0 && y > 0 && mapData[x-1,y-1] == TYPE.FLOOR)
			return true;
		if(x < mapWidth-1 && y > 0 && mapData[x+1,y-1] == TYPE.FLOOR)
			return true;
		if(x > 0 && y < mapHeight-1 && mapData[x-1,y+1] == TYPE.FLOOR)
			return true;
		if(x < mapWidth-1 && y < mapHeight-1 && mapData[x+1,y+1] == TYPE.FLOOR)
			return true;


		return false;
	}
	
	bool RoomCollides (RoomData r)
	{
		RoomData other = new RoomData ();
		for (int i =0; i < roomList.Count; i++) {
			other = roomList [i];
			if (r.CollidesWith (roomList [i])) {
				return true;
			}
		}
		return false;
	}
} 
