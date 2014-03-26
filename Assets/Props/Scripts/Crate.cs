using UnityEngine;
using System.Collections;


public class Crate : MonoBehaviour {

	public GameObject[] Items;


	public void Bust()
	{
		if(Items.Length > 0)
		{
			int i = Random.Range(0, Items.Length);
			if(i <Items.Length)
			GameObject.Instantiate(Items[i],transform.position, Quaternion.identity);
		}
		GameObject.DestroyObject(this.gameObject);
	}
}
