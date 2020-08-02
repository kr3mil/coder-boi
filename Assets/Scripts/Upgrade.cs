using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
  public string Name;
  public string Description;
  public int Price;

  public Upgrade(string name, string description, int price)
  {
    Name = name;
    Description = description;
    Price = price;
  }
}