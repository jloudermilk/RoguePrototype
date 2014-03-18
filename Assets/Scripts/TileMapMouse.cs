using UnityEngine;
using System.Collections;
[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour {
	TileMapTest tileMap;
	public Vector3 currentTileCoord;
	public Transform selectionCube;
	public Color highlightColor;

	Color normalColor;
	// Use this for initialization
	void Start () {
		tileMap = GetComponent<TileMapTest>();
		normalColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if(collider.Raycast(ray,out hitInfo,Mathf.Infinity))
		{
			int x = Mathf.FloorToInt(hitInfo.point.x /tileMap.tileSize);
			int z = Mathf.FloorToInt(hitInfo.point.z /tileMap.tileSize);
			currentTileCoord = new Vector3(x,5f,z);

			selectionCube.transform.position = currentTileCoord * tileMap.tileSize;

		}
		else
		{
			//renderer.material.color = normalColor;
		}
		
		
		
	}
}

