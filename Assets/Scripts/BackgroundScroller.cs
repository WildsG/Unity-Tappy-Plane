using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {
	public float speed;
	public float resetPos;
	Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(GameManager.currentGameState == GameManager.GameState.End)
			return;
			
		transform.Translate(Vector3.left * speed * Time.deltaTime);
		if(transform.position.x < resetPos)
			transform.position = startPos;
	}
}
