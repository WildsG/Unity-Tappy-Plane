using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
	public GameObject pillarPredfab;
	public float distanceBetweenPillars = 5f;
	private List<Transform> pillars = new List<Transform>();
	// Use this for initialization
	void Start () {
		for(int i = 1;i <= 5;i ++)
		{
			GameObject pillar = Instantiate(pillarPredfab, new Vector3((distanceBetweenPillars * i) + 5,Random.Range(-3.85f,0.2f),0),Quaternion.identity);
			pillars.Add(pillar.transform);
		}	
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.currentGameState == GameManager.GameState.End)
			return;

		if(pillars[0].position.x <= -5f)
			RepostionPillar();
	}
	void RepostionPillar()
	{
		pillars[0].position = new Vector3((pillars.Count -1) * distanceBetweenPillars ,Random.Range(-3.55f,0.2f),0);
		Transform firstPillar = pillars[0];
		pillars.RemoveAt(0);
		pillars.Add(firstPillar);

	}
}
