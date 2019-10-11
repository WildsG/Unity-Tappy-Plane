using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // how many degrees per second will the object rotate
    public float rotationSpeed = 90f;
    public float flyForce = 30f;
    public float minRotation = -45;
    public float maxRotation = 45;
    public float maxPlaneYVelocity = 5f;

    private Vector3 rotationEuler = Vector3.zero;
    private Rigidbody2D rb2d;
    private float lastFrameVelocity;
    private GameManager GM;
    private Transform gfx;
	// Use this for initialization
	void Start () {
        GM = FindObjectOfType<GameManager>();
        rb2d = GetComponent<Rigidbody2D>();
        gfx = transform.GetChild(0);
        gfx.eulerAngles = rotationEuler;
	}
	
	// Update is called once per frame
	void Update () {

        if (!isGamePlayable())
            return;
        
        float currentFrameVelocity = rb2d.velocity.y;
		if(Input.GetMouseButton(0))
            rb2d.AddForce(Vector2.up * flyForce,ForceMode2D.Force);

        if(currentFrameVelocity-lastFrameVelocity>0)
            rotationEuler.z += rotationSpeed * Time.deltaTime*1.1f;
        else if(currentFrameVelocity<-0.75f)
            rotationEuler.z -= rotationSpeed * 0.8f *  Time.deltaTime;

        rb2d.velocity = new Vector2(0, Mathf.Clamp(rb2d.velocity.y, -maxPlaneYVelocity,maxPlaneYVelocity));
        rotationEuler.z = Mathf.Clamp(rotationEuler.z, minRotation, maxRotation);
        gfx.eulerAngles = rotationEuler;
    
        lastFrameVelocity = rb2d.velocity.y;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("ScoreTrigger"))
            GM.IncreaseScore(1);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        bool collidedWithObstacle = false;
        if (other.gameObject.CompareTag("Obstacle"))
            collidedWithObstacle = true;
        if(other.transform.childCount > 0 )
            if(other.transform.GetChild(0).CompareTag("Obstacle"))
                collidedWithObstacle = true;

        if (collidedWithObstacle)
        {
            GameManager.currentGameState = GameManager.GameState.End;
            StartCoroutine(slowAnimation());
        }
        
    }

    private IEnumerator slowAnimation()
    {
        Animator gfxAnimator = gfx.GetComponent<Animator>();
        float currentPlaybackSpeed = gfxAnimator.speed;
        while(currentPlaybackSpeed>0.1f)
        {   
            currentPlaybackSpeed -= Time.deltaTime/5;
            gfxAnimator.speed = currentPlaybackSpeed;
            yield return null;
        }
        gfxAnimator.speed = 0;
        yield return null;
    }

    private bool isGamePlayable()
    {   
        if (GameManager.currentGameState == GameManager.GameState.Play)
        {
            if (!rb2d.simulated)
                rb2d.simulated = true;
            return true;
        }
        else if (GameManager.currentGameState == GameManager.GameState.Start)
        {
            rb2d.simulated = false;
            return false;
        }
        return false;
    }
}
