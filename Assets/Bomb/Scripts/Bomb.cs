using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	public GameObject Player;
	public float Timer = 2f;
	public float Size = 1;
	Animator animator;
	float elapsedTime  = 0;
	public AnimationInfo[] aniInfo;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime +=  Time.deltaTime;
		if(elapsedTime > Timer && elapsedTime < Timer +.4)
		{
			 aniInfo = animator.GetCurrentAnimationClipState(0);
			aniInfo.
			animator.Play("Explode");
		}
	}
}
