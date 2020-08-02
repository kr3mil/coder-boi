using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
  public FarmBlock[,] Tiles;
  public Transform TilesParent;
  public Transform DropShadowsParent;
  public GameObject TilePrefab;
  public GameObject DropShadowPrefab;

  private void Start()
  {
    var area = 1;
    Tiles = new FarmBlock[area, area];

    for (int x = 0; x < area; x++)
    {
      for (int y = 0; y < area; y++)
      {
        // Generate tile
        var tile = GameObject.Instantiate(TilePrefab, Vector3.zero, Quaternion.identity, TilesParent);
        tile.transform.localPosition = Vector3.zero;
        tile.transform.localScale = new Vector3(2, 2, 1);
        Tiles[x, y] = tile.GetComponent<FarmBlock>();

        // Generate drop shadow
        var shadow = GameObject.Instantiate(DropShadowPrefab, Vector3.zero, Quaternion.identity, DropShadowsParent);
        shadow.transform.localPosition = tile.transform.localPosition;
        shadow.transform.localScale = tile.transform.localScale;
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