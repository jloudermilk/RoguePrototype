using UnityEngine;
using System.Collections;

public class TheForce : MonoBehaviour {

	// Use this for initialization
	void Start () {
GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up *100);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
