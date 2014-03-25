using UnityEngine;
using System.Collections;

public class DarkPlayerController : MonoBehaviour {
	public Vector2 targetVelocity;
	private Animator animator;
	public GameObject Bomb;
	public int BombNum = 1;
	public int BombSize = 1;
	public Vector2 test;
	public Vector2 bombSpot; 
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

		TileTest();
	
		if(Input.GetButtonDown("Fire1") && BombNum >0)
		{
			BombNum--;
			PlaceBomb();

		}



	}
	void TileTest()
	{
		Vector2 pos = transform.position;
		float fX, fY;
		int iX, iY;
		iX = (int) pos.x;
		fX = pos.x;
		iY = (int) pos.y * -1;
		fY = pos.y * -1;


		if(fX -iX >= .5)
		{
			test.x = iX+1;
		}
		else
		{
			test.x = iX;
		}
		if(fY -iY >= .5)
		{
			test.y = (iY+1) * -1;
		}
		else
		{
			test.y = (iY) * -1;
		}
	}


	void LateUpdate(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}



	public void PlaceBomb()
	{

		GameObject SetBomb = Instantiate(Bomb,test,Quaternion.identity) as GameObject;
	
		SetBomb.GetComponent<Bomb>().Player = gameObject;
		SetBomb.GetComponent<Bomb>().Size =BombSize;
	}
}
