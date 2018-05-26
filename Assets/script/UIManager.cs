using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	int score;
	public static UIManager Instance;
	public Text ScoreText;
	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("UIManager failed");
		}else
		{
			Instance = this;
		}
	}

    public void AddScore(int _delta)
	{
		this.score += _delta;      
        UpdateScoreText();
	}
    public void SetScore(int _score)
	{
		this.score = _score;
        UpdateScoreText();
	}

	void UpdateScoreText()
	{
		this.ScoreText.text = string.Format("Score:{0}", this.score);
	}
	// Use this for initialization
	void Start () {      
        UpdateScoreText();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
