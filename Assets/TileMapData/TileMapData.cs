using UnityEngine;
using System.Collections.Generic;

public class TileMapData  {
	public enum TYPE
	{

		UNKNOWN = 0,
		FLOOR = 1,
		WALL = 2,
		STONE = 3
	}
	protected class RoomData
	{
		public int m_iLeft, m_iTop, m_iWidth, m_iHeight;
	
	}

	TYPE[,] mapData;
	public int m_iWidth,m_iHeight;

	List<RoomData> m_lRooms;


	public TileMapData()
	{
		m_iWidth= 20;
		m_iHeight = 20;
		
		mapData = new TYPE[m_iWidth,m_iHeight];
	}
	public TileMapData(int a_iWidth,int a_iHeight)
	{
		m_iWidth= a_iWidth;
		m_iHeight = a_iHeight;

		mapData = new TYPE[m_iWidth,m_iHeight];
		m_lRooms = new List<RoomData>();

		RoomData room = new RoomData();
		for(int i = 0; i < 10;i++)
		{
			int rsx = Random.Range(4,8);
			int rsy = Random.Range(4,8);

			room.m_iLeft = Random.Range(0,m_iWidth - rsx);
			room.m_iTop = Random.Range(0,m_iHeight - rsy);
			room.m_iWidth = rsx;
			room.m_iHeight = rsy;

			MakeRoom(room);
		}

	}
	public TYPE GetTile(int a_iX,int a_iY)
	{
		if(a_iX < 0 || a_iX >= m_iWidth || a_iY<0 || a_iY >= m_iHeight)
		{
			return TYPE.UNKNOWN;
		}
		return mapData[a_iX,a_iY];
	}

	 void MakeRoom( RoomData room )
	{
		for(int x = 0; x < room.m_iWidth; x++)
			for(int y = 0; y < room.m_iWidth; y++)
		{
			if(x == 0 || x == room.m_iWidth-1 || y == 0 || y == room.m_iHeight-1)
				mapData[room.m_iLeft+x,room.m_iTop+y] = TYPE.WALL;
			else
			mapData[room.m_iLeft+x, room.m_iTop+y] = TYPE.FLOOR;

		}

	}
}
