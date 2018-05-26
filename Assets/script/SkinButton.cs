using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    public int Price;
    private Toggle toggle;
    public Color SkinSpriteColor;
    private Text PriceText;

    private void Awake()
    {
		this.toggle = this.GetComponentInChildren<Toggle>();
		SkinSpriteColor = this.transform.Find("Toggle").Find("Background").GetComponent<Image>().color;
		PriceText = this.GetComponentInChildren<Text>();
		this.PriceText.text = string.Format("{0}", Price);      
	}

    private void Update()
    {
		//toggle.interactable = (UIManager.Instance.Score >= this.Price);
    }
   

}
