using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
	{
	public FarmBlock[,] Tiles;

	private void Start()
		{
		var area = int.Parse(Mathf.Sqrt(transform.childCount).ToString());
		Tiles = new FarmBlock[area, area];
		for (int x = 0; x < area; x++)
			{
			for (int y = 0; y < area; y++)
				{
				Tiles[x, y] = transform.GetChild(x + y * area).GetComponent<FarmBlock>();
				}
			}
		}

	public bool Plant(int x, int y)
		{
		return Tiles[x - 1, y - 1].Plant();
		}

	public bool Water(int x, int y)
		{
		return Tiles[x - 1, y - 1].Water();
		}

	public bool Harvest(int x, int y)
		{
		return Tiles[x - 1, y - 1].Harvest();
		}
	}