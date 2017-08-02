using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
	public GameObject Player;
	public GameObject FireN;
	public GameObject FireS;
	public GameObject FireE;
	public GameObject FireW;
	public GameObject FireH;
	public GameObject FireV;
	public float Timer = 2f;
	public float Size = 1;
	public Animator animator;
	float elapsedTime = 0;
	public float clipTime;
	public AnimationEvent ReloadEvent;
	public AnimationEvent DieEvent;
	public bool exploded = false;
	
	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
		ReloadEvent = new AnimationEvent ();
		ReloadEvent.functionName = "BombPlus";
	}
	void Update ()
	{
		clipTime = animator.GetCurrentAnimatorStateInfo (0).length;

		elapsedTime += Time.deltaTime;
		if (elapsedTime > Timer && !exploded) {
			Expode();
		}
	}
	// Update is called once per frame

	void OnTriggerExit2D(Collider2D other)
	{
		transform.GetComponent<Collider2D>().isTrigger = false;
	}
	public void Expode ()
	{
		
		exploded = true;
		animator.Play ("Explode");
		int layerMask = LayerMask.NameToLayer ("CollisionLayer");
		RaycastHit2D hit;
		GameObject SetFire;
		Vector3 point;
		
		for (int i = 1; i < Size; i++) {
			hit = Physics2D.Raycast (transform.position, Vector2.right, (float)i, 1 << layerMask);
			
			if (hit.collider == null) {
				point = transform.position + (new Vector3 (1, 0, 0) * i);
				SetFire = Instantiate (FireH, point, Quaternion.identity) as GameObject;
				Debug.DrawLine (transform.position, point, Color.green, 10f);
				Debug.Log (point);
				SetFire.GetComponent<Fire> ().Player = Player;
			}
			
			hit = Physics2D.Raycast (transform.position, -Vector2.right, (float)i, 1 << layerMask);
			
			if (hit.collider == null) {
				point = transform.position + (new Vector3 (-1, 0, 0) * i);
				SetFire = Instantiate (FireH, point, Quaternion.identity) as GameObject;
				Debug.DrawLine (transform.position, point, Color.green, 10f);
				Debug.Log (point);
				SetFire.GetComponent<Fire> ().Player = Player;
			}
			
			hit = Physics2D.Raycast (transform.position, Vector2.up, (float)i, 1 << layerMask);
			
			if (hit.collider == null) {
				point = transform.position + (new Vector3 (0, 1, 0) * i);
				SetFire = Instantiate (FireV, point, Quaternion.identity) as GameObject;
				Debug.DrawLine (transform.position, point, Color.green, 10f);
				Debug.Log (point);
				SetFire.GetComponent<Fire> ().Player = Player;
			}
			
			hit = Physics2D.Raycast (transform.position, -Vector2.up, (float)i, 1 << layerMask);
			
			if (hit.collider == null) {
				point = transform.position + (new Vector3 (0, -1, 0) * i);
				SetFire = Instantiate (FireV, point, Quaternion.identity) as GameObject;
				Debug.DrawLine (transform.position, point, Color.green, 10f);
				Debug.Log (point);
				SetFire.GetComponent<Fire> ().Player = Player;
			}
			
		}
		
		hit = Physics2D.Raycast (transform.position, Vector2.right, (float)Size, 1 << layerMask);
		
		if (hit.collider == null) {
			point = transform.position + (new Vector3 (1, 0, 0) * Size);
			SetFire = Instantiate (FireE, point, Quaternion.identity) as GameObject;
			Debug.DrawLine (transform.position, point, Color.green, 10f);
			Debug.Log (point);
			SetFire.GetComponent<Fire> ().Player = Player;
		}
		
		hit = Physics2D.Raycast (transform.position, -Vector2.right, (float)Size, 1 << layerMask);
		
		if (hit.collider == null) {
			point = transform.position + (new Vector3 (-1, 0, 0) * Size);
			SetFire = Instantiate (FireW, point, Quaternion.identity) as GameObject;
			Debug.DrawLine (transform.position, point, Color.green, 10f);
			Debug.Log (point);
			SetFire.GetComponent<Fire> ().Player = Player;
		}
		
		hit = Physics2D.Raycast (transform.position, Vector2.up, (float)Size, 1 << layerMask);
		
		if (hit.collider == null) {
			point = transform.position + (new Vector3 (0, 1, 0) * Size);
			SetFire = Instantiate (FireN, point, Quaternion.identity) as GameObject;
			Debug.DrawLine (transform.position, point, Color.green, 10f);
			Debug.Log (point);
			SetFire.GetComponent<Fire> ().Player = Player;
		}
		
		hit = Physics2D.Raycast (transform.position, -Vector2.up, (float)Size, 1 << layerMask);
		
		if (hit.collider == null) {
			point = transform.position + (new Vector3 (0, -1, 0) * Size);
			SetFire = Instantiate (FireS, point, Quaternion.identity) as GameObject;
			Debug.DrawLine (transform.position, point, Color.green, 10f);
			Debug.Log (point);
			SetFire.GetComponent<Fire> ().Player = Player;
		}
		
	}
	
	public void BombPlus ()
	{
		Player.SendMessage("AddBomb",SendMessageOptions.DontRequireReceiver);
		Die ();
	}
	
	public void Die ()
	{
		
		GameObject.DestroyObject (gameObject);
	}
}
