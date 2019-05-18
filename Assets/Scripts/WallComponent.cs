using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallComponent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
