using UnityEngine;
using System.Collections;

public class SizePlus : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag.Equals("Player"))
		{
			other.gameObject.SendMessage("IncreaseSize",SendMessageOptions.DontRequireReceiver);
			Destroy();
		}
		
	}
	public void Destroy()
	{
		GameObject.DestroyObject(this.gameObject);
	}
}
