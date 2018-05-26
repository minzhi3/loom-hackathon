using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class UIManager : MonoBehaviour {

	int score;
	public GameObject pacman;
	float TimeInterval = 2;
	float currentTime;
	string ranking;
	public static UIManager Instance;
	public Text ScoreText;
	public List<int> SkinState;
	public ToggleGroup SkinGroup;
	public Text RankingText;
	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("UIManager failed");
		}else
		{
			Instance = this;
			SkinState = new List<int>();
		}
	}
	public int Score{ get { return score; }}
    
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
		int result;
		bool success = int.TryParse(str, out result);
		if (success)
			this.score = result;
	}
	async Task updateRanking()
	{
		var strs = await LoomTools.Instance.GetRanking();
		//Debug.Log("Ranking " + str);
		ranking = "";
        foreach (var str in strs)
		{
			ranking += str;
			ranking += '\n';         
		}
		this.RankingText.text = ranking;
		Debug.Log(this.ranking);
	}
    public void ChangeSkin(int skinIndex)
	{
		if (Random.value > 0.5f)
		{

			pacman.GetComponentInChildren<SpriteRenderer>().color = Color.green;

		}else
		{
            pacman.GetComponentInChildren<SpriteRenderer>().color = Color.red;
		}
		/*foreach (var toggle in SkinGroup.ActiveToggles())
		{
			if (toggle.enabled)
			{
				var color = toggle.GetComponent<SkinButton>().SkinSpriteColor;
				pacman.GetComponentInChildren<SpriteRenderer>().color = color;
			}
		}*/

	}
	// Use this for initialization
	void Start () {      
        UpdateScoreText();
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		if (currentTime > TimeInterval)
		{
			currentTime = 0;
			updateRanking();
		}
	}
}
