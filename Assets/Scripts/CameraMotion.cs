using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {
	[Range(0,50)] 
	public float circlingSpeed = 20;
	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");		
	}
	
	void Update () {		
		Vector3 pos = player.transform.position;
		transform.RotateAround (pos, Vector3.up, circlingSpeed * Time.deltaTime);
		transform.LookAt(player.transform);
	}
}
