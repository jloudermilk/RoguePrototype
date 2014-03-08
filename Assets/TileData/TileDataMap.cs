
public class TileDataMap  {
	TileData[] tiles;
	int m_iWidth,m_iHeight;

	public TileDataMap(int a_iWidth,int a_iHeight)
	{
		m_iWidth= a_iWidth;
		m_iHeight = a_iHeight;

		tiles = new TileData[m_iWidth * m_iHeight];
	}
	public TileData GetTile(int a_iX,int a_iY)
	{
		if(a_iX < 0 || a_iX >= m_iWidth || a_iY<0 || a_iY >= m_iHeight)
		{
			return null;
		}
		return tiles[a_iY * m_iWidth + a_iX];
	}

}
