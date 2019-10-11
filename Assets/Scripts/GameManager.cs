using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public enum GameState
	{
		Start,
		Play,
		End
	};
	public static GameState currentGameState;
	public Text scoreText;
	public GameObject gameOverPanel;
	public GameObject gameStartPanel;
	public Text CurrentScore;
	public Text HighScore;
	public Animator playerAnimator;	
	public Image medalImagePlaceholder;

	private int score = 0;
	private string[] animationNames = new string[]{"FlyingRed","FlyingBlue","FlyingYellow","FlyingGreen"};
	private List<Sprite> medalSprites = new List<Sprite>();
	private Vector3 swipeStartPos = Vector3.zero;
	private int animationIndex = 0;
	void Start () {
		currentGameState = GameState.Start;
		LoadMedals();	
	}
	
	// Update is called once per frame
	void Update () {
        switch (currentGameState)
        {
            case GameState.End:
                endGame();
                break;
            case GameState.Start:
                startGame();
                break;
        }
	}
	public void IncreaseScore(int scoreToAdd)
	{
		score += scoreToAdd;
		scoreText.text = score.ToString();
	}
	private void endGame()
	{	
		// enable labels
		scoreText.enabled = false;
		gameOverPanel.SetActive(true);
		// check which medal to display
		if(score>=20&&score<50)
			medalImagePlaceholder.sprite = medalSprites[1];
		else if(score>=50)
			medalImagePlaceholder.sprite = medalSprites[2];
		else
			medalImagePlaceholder.sprite = medalSprites[0];
		
		//check whether highscore needs to be updated
		if(score > PlayerPrefs.GetInt("HighScore"))
			PlayerPrefs.SetInt("HighScore",score);

		// show scores
		HighScore.text = PlayerPrefs.GetInt("HighScore").ToString();
		CurrentScore.text = score.ToString();

	}
	private void startGame()
	{	
		//swipedetection
		if(Input.GetMouseButtonDown(0))
			swipeStartPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		if(Input.GetMouseButtonUp(0))
		{	
			Vector3 swipeEndPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			int prevAnimIndex = animationIndex;

			if(swipeStartPos.x - swipeEndPos.x < -0.3f)
				animationIndex++;
			else if(swipeStartPos.x - swipeEndPos.x > 0.3f)
				animationIndex--;

			if(animationIndex>3)
				animationIndex = 0;
			else if(animationIndex<0)
				animationIndex = 3;

			if(prevAnimIndex != animationIndex)
				playerAnimator.Play(animationNames[animationIndex]);
		}
	}
	private void LoadMedals()
	{
		foreach(Sprite s in Resources.LoadAll<Sprite>("sheet"))
		{
			if(s.name.StartsWith("medal"))
			{
				medalSprites.Add(s);
			}
		}
	}
	
	public void RestartGame()
	{	
		currentGameState = GameState.Start;
		score = 0;
		SceneManager.LoadScene("Main");
	}
	public void onClickStartGame()
	{	
		gameStartPanel.SetActive(false);
		currentGameState = GameState.Play;

	}
}
