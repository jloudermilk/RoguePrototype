using UnityEngine;
using System.Collections;

public class DarkPlayerController : MonoBehaviour {
	public Vector2 targetVelocity;
	private Animator animator;
	public GameObject Bomb;
	public int BombNum = 1;
	public int BombSize = 1;
	public Vector2 test;
	RaycastHit hit;


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

	void OnTriggerEnter2D(Collider2D other)
	{
	

		if(!other.gameObject.layer.ToString().Equals("8")){

			test = other.transform.position;
		}
	}

	public void PlaceBomb()
	{

		GameObject SetBomb = Instantiate(Bomb,test,Quaternion.identity) as GameObject;
	
		SetBomb.GetComponent<Bomb>().Player = gameObject;
	}
}
