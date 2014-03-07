using UnityEngine;
using System.Collections;

public class TileMouseOver : MonoBehaviour {
	public Color highlightColor;
	Color normalColor;
	// Use this for initialization
	void Start () {
		normalColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if(collider.Raycast(ray,out hitInfo,100))
		   {
			Debug.Log(transform.worldToLocalMatrix *hitInfo.point);
		}
		else
		{
			//renderer.material.color = normalColor;
		}


			
	}
}
