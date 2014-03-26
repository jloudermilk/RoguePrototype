using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public GameObject Player;
	public Animator animator;
	float elapsedTime  = 0;
	public float clipTime;
	public AnimationEvent DieEvent;
	
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		DieEvent = new AnimationEvent();
		DieEvent.functionName = "Die";
	}

	public void Die()
	{	
		GameObject.DestroyObject(gameObject);
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		other.gameObject.SendMessage("Bust",SendMessageOptions.DontRequireReceiver);
		other.gameObject.SendMessage("Kill",SendMessageOptions.DontRequireReceiver);
		other.gameObject.SendMessage("Explode",SendMessageOptions.DontRequireReceiver);
	}
}
