using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour {

	public GameObject DotObject;
	public List<Vector2Int> map;
	public void GetMapData()
	{
		map = new List<Vector2Int>();
		for (int i = 1; i < 5;i++)
		{
			for (int j = 0; j < 20;j++)
			{

				map.Add(new Vector2Int(i * 3, j));
			}
		}
	}
    
    void InitMap()
	{
		foreach (var dot in map)
		{
			Instantiate(DotObject, new Vector3(dot.x, dot.y, 0), Quaternion.identity, this.transform);
		}
	}
	// Use this for initialization
	void Start () {
		GetMapData();
		InitMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
