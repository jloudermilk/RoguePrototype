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
	
	// Update is called once per frame
	void Update () {

	}
	public void Die()
	{
		
		GameObject.DestroyObject(gameObject);
	}
}
