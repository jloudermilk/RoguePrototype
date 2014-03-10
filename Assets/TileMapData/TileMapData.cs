using UnityEngine;
using System.Collections.Generic;

public class TileMapData
{
		public enum TYPE
		{

				UNKNOWN = 0,
				FLOOR = 1,
				WALL = 2,
				STONE = 3
		}

		public class RoomData
		{
				public int m_iLeft, m_iTop, m_iWidth, m_iHeight;

				public int m_iRight {
						get{ return m_iLeft + m_iWidth - 1;}
				}

				public int m_iBottom {
						get{ return m_iTop + m_iHeight - 1;}
				}

		public int m_iCenterX
		{
			get{ return (m_iLeft+ m_iWidth/2);}
		}
		public int m_iCenterY
		{
			get{return (m_iTop + m_iHeight/2);}
		}

				public bool CollidesWith (RoomData other)
				{
						if (m_iLeft > other.m_iRight - 1)
								return false;
						if (m_iTop > other.m_iBottom - 1)
								return false;
						if (m_iRight < other.m_iLeft + 1)
								return false;
						if (m_iBottom < other.m_iTop + 1)
								return false;
						return true;
				}
		}

		private class RoomList : List<RoomData>
		{
		}
		TYPE[,] mapData;
		public int m_iWidth, m_iHeight;
		RoomList m_lRooms;

		public TileMapData ()
		{
				m_iWidth = 20;
				m_iHeight = 20;
		
				mapData = new TYPE[m_iWidth, m_iHeight];
		}

		bool RoomCollides (RoomData r)
		{
				RoomData other = new RoomData ();
				for (int i =0; i < m_lRooms.Count; i++) {
						other = m_lRooms [i];
						if (r.CollidesWith (m_lRooms [i])) {
								return true;
						}
				}
				return false;
		}

		public TileMapData (int a_iWidth, int a_iHeight)
		{

				m_iWidth = a_iWidth;
				m_iHeight = a_iHeight;

				mapData = new TYPE[m_iWidth, m_iHeight];
				for (int x = 0; x < m_iWidth; x++)
						for (int y = 0; y < m_iHeight; y++) {
								mapData [x, y] = TYPE.STONE;
						}	


				m_lRooms = new RoomList ();

				
				for (int i = 0; i < 20; i++) {
						RoomData room = new RoomData ();
						int rsx = Random.Range (4, 8);
						int rsy = Random.Range (4, 8);

						room.m_iLeft = Random.Range (0, m_iWidth - rsx);
						room.m_iTop = Random.Range (0, m_iHeight - rsy);
						room.m_iWidth = rsx;
						room.m_iHeight = rsy;
						
						if (m_lRooms.Count == 0) {

								m_lRooms.Add (room);
						} else if (!RoomCollides (room)) {
								m_lRooms.Add (room);
								MakeRoom (room);
						}
				}

		MakeCorridor(m_lRooms[0],m_lRooms[1]);
		}

		public TYPE GetTile (int a_iX, int a_iY)
		{
				if (a_iX < 0 || a_iX >= m_iWidth || a_iY < 0 || a_iY >= m_iHeight) {
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

	void MakeCorridor(RoomData r1,RoomData r2 )
	{
		int x = r1.m_iCenterX;
		int y = r1.m_iCenterY;
		while(x != r2.m_iCenterX )
		{
		
				mapData[x,y] = TYPE.FLOOR;
			x += x < r2.m_iCenterX ? 1 : -1;
				
			}
		while(y != r2.m_iCenterY )
		{
			
			mapData[x,y] = TYPE.FLOOR;
			y += y < r2.m_iCenterY ? 1 : -1;
			
		}
			





	}
} 
