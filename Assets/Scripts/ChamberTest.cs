using UnityEngine;
using System.Collections;



public class ChamberTest: MonoBehaviour
{
	public int Left, Top;
	public int Width, Height;
	public int Range = 8, Offset = 3;
	public bool Overlapping = false;
	public int moveX;
	public int moveY;
	public ChamberList Neighbors;
	
	public void Copy (Chamber other)
	{
		Left = other.Left;
		Top = other.Top;
		Width = other.Width;
		Height = other.Height;
		Range = other.Range; 
		Offset = other.Offset;
		Overlapping = other.Overlapping;
		moveX = other.moveX;
		moveY = other.moveY;
		Neighbors = other.Neighbors;
		
		
	}
	
}