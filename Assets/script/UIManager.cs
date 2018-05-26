using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

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
    
	public async Task AddScore(int _delta)
	{
		//this.score += _delta;      
		await LoomTools.Instance.GetCoin();
		Debug.Log("add score");
        UpdateScoreText();
	}
    /*public void SetScore(int _score)
	{
		//this.score = _score;
        UpdateScoreText();
	}*/
    
	async Task UpdateScoreText()
	{
		var str = await LoomTools.Instance.GetCoinAmount();
		this.ScoreText.text = string.Format("Score:{0}", str);
	}
	// Use this for initialization
	void Start () {      
        UpdateScoreText();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
