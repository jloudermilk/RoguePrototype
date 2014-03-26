using UnityEngine;
using System.Collections;

public class BombPlus : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag.Equals("Player"))
		{
			other.gameObject.SendMessage("AddBomb",SendMessageOptions.DontRequireReceiver);	
			Destroy();
		}

	}
	public void Destroy()
	{
		GameObject.DestroyObject(this.gameObject);
	}
}
