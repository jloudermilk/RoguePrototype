using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	public GameObject Player;
	public GameObject FireE;
	public float Timer = 2f;
	public float Size = 1;
	public Animator animator;
	float elapsedTime  = 0;
	public float clipTime;
	public AnimationEvent ReloadEvent;
	public AnimationEvent DieEvent;
	public bool exploded = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		ReloadEvent = new AnimationEvent();
		ReloadEvent.functionName = "AddBomb";
		DieEvent = new AnimationEvent();
		DieEvent.functionName = "Die";
	}

	// Update is called once per frame
	void Update () {
		clipTime = animator.GetCurrentAnimatorStateInfo(0).length;
		elapsedTime +=  Time.deltaTime;
		if(elapsedTime > Timer && !exploded)
		{
			exploded = true;
			animator.Play("Explode");
			int layerMask = LayerMask.NameToLayer("Floor");
			RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.right, 1f,layerMask);
			if(hit.collider.isTrigger)
			{
				GameObject SetFireEnd = Instantiate(FireE,transform.position + new Vector3(1,0,0),Quaternion.identity) as GameObject;
				SetFireEnd.GetComponent<Fire>().Player = Player;
			}
		}
	}
	public void AddBomb()
	{
		Player.GetComponent<DarkPlayerController>().BombNum++;
		Die();
	}
	public void Die()
	{

		GameObject.DestroyObject(gameObject);
	}
}
