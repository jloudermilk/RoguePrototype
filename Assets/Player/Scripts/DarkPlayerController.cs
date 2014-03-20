using UnityEngine;
using System.Collections;

public class DarkPlayerController : MonoBehaviour {
	public Vector2 targetVelocity;
	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	void FixedUpdate () {
		targetVelocity = new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		rigidbody2D.velocity=targetVelocity * 2.5f;
		animator.SetFloat("SpeedX",targetVelocity.x);
		animator.SetFloat("SpeedY",targetVelocity.y);
	}
	void LateUpdate(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}
}
