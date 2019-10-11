using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarMover : MonoBehaviour {
	public float speed;
	private Rigidbody2D rb2d;
	void Start() {
		rb2d = GetComponent<Rigidbody2D>();
	}
	// Update is called once per frame
	void Update () {
        switch (GameManager.currentGameState)
        {
            case GameManager.GameState.End:
            case GameManager.GameState.Start:
                rb2d.velocity = Vector2.zero;
                break;
            case GameManager.GameState.Play:
                rb2d.velocity = Vector2.left * speed;
                break;
        }
	}
}
