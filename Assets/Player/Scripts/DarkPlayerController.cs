using UnityEngine;
using System.Collections;

public class DarkPlayerController : MonoBehaviour {
	public Vector2 targetVelocity;
	private Animator animator;
	public GameObject Bomb;
	public int BombNum = 1;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	void FixedUpdate () {
		targetVelocity = new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		rigidbody2D.velocity=targetVelocity * 2.5f;
		animator.SetFloat("SpeedX",targetVelocity.x);
		animator.SetFloat("SpeedY",targetVelocity.y);
		if(Input.GetButtonDown("Fire1") && BombNum >0)
		{
			BombNum--;
			PlaceBomb();

		}
	}
	void LateUpdate(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}

	public void PlaceBomb()
	{
		GameObject SetBomb = Instantiate(Bomb,transform.position,Quaternion.identity) as GameObject;

		SetBomb.GetComponent<Bomb>().Player = gameObject;
	}
}
