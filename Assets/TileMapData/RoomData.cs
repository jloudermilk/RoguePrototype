//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public class RoomData
{
	public int m_iLeft, m_iTop, m_iWidth, m_iHeight;
	public bool Connected = false;
	
	public int m_iRight {
		get{ return m_iLeft + m_iWidth - 1;}
	}
	
	public int m_iBottom {
		get{ return m_iTop + m_iHeight - 1;}
	}
	
	public int m_iCenterX {
		get{ return (m_iLeft + m_iWidth / 2);}
	}
	
	public int m_iCenterY {
		get{ return (m_iTop + m_iHeight / 2);}
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