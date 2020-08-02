using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CodingHandler : MonoBehaviour
{
  public TextMeshProUGUI DisplayTextTMP;
  public TextMeshProUGUI BackgroundTextTMP;
  public TMP_InputField InputTextTMP;
  public List<string> DisplayText = new List<string>();
  public List<string> UserInput = new List<string>();
  public FarmManager FarmManager;

  private Upgrade[] upgrades = {
    new Upgrade("grid tier 2", "upgrades the grid to a 2x2", 5)
    };
  private string[] commands = { "help", "clear", "plant", "harvest", "water", "upgrades", "upgrade" };
  private string lastCommand = "";
  private int currentIndex = 0;

  private void Start()
  {
    Array.Sort(commands, (x, y) => String.Compare(x, y));
    AddToDisplay("<i>Welcome. Type help for a list of commands.</i>");
  }

  // Update is called once per frame
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Return))
    {
      AppendDisplayText(InputTextTMP.text);
      HandleCommand();
    }
    else if (Input.GetKeyDown(KeyCode.UpArrow))
    {
      GetPrevious();
    }
    else if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      GetNext();
    }
    else if (Input.GetKeyDown(KeyCode.Tab))
    {
      HandleTab(InputTextTMP.text);
    }
  }

  public void ValueChanged(string txt)
  {
    string[] args = txt.Split(' ');
    if (!string.IsNullOrWhiteSpace(args[args.Length - 1]))
    {
      var word = commands.FirstOrDefault(x => x.StartsWith(args.Last()) || x == args.Last());
      if (word != null)
      {
        args[args.Length - 1] = word;
        BackgroundTextTMP.text = string.Join(" ", args);
      }
    }
    else
    {
      BackgroundTextTMP.text = "";
    }
  }

  private void HandleCommand()
  {
    string[] args = lastCommand.Split(' ');
    Debug.Log(args.Length);

    // Command switch
    switch (args[0])
    {
      case "help":
        DisplayHelpMessage(args.Length > 1 ? int.Parse(args[1]) : 1);
        break;

      case "clear":
        ClearTerminal();
        break;

      case "plant":
        if (ArgsLengthCheck(args, 3))
          Plant(args);
        break;

      case "harvest":
        if (ArgsLengthCheck(args, 3))
          Harvest(args);
        break;

      case "water":
        if (ArgsLengthCheck(args, 3))
          Water(args);
        break;

      case "upgrades":
        ShowUpgrades();
        break;
      default:
        Warn("unknown command");
        Debug.LogWarning("UNHANDLED COMMAND: " + args[0]);
        break;
    }
  }

  private bool ArgsLengthCheck(string[] args, int length)
  {
    if (args.Length == length)
    {
      return true;
    }
    Warn(args[0] + " requires " + (length - 1) + " parameters");
    return false;
  }

  private void Plant(string[] args)
  {
    if (FarmManager.Plant(int.Parse(args[1]), int.Parse(args[2])))
    {
      Say("seed planted at " + args[1] + " " + args[2]);
    }
    else
    {
      Warn("can't plant seed at " + args[1] + args[2]);
    }
  }

  private void Harvest(string[] args)
  {
    if (FarmManager.Harvest(int.Parse(args[1]), int.Parse(args[2])))
    {
      Say("tile " + args[1] + " " + args[2] + " harvested");
    }
    else
    {
      Warn("tile is not ready to harvest");
    }
  }

  private void Water(string[] args)
  {
    if (FarmManager.Water(int.Parse(args[1]), int.Parse(args[2])))
    {
      Say("tile " + args[1] + " " + args[2] + " watered");
    }
    else
    {
      Warn("tile is already watered");
    }
  }

  private void HandleTab(string txt)
  {
    string[] args = txt.Split(' ');
    var word = commands.FirstOrDefault(x => x.StartsWith(args.Last()) || x == args.Last());
    if (word != null)
    {
      args[args.Length - 1] = word;
      InputTextTMP.text = string.Join(" ", args);
      InputTextTMP.MoveTextEnd(false);
    }
  }

  private void ShowUpgrades()
  {

  }

  private void ClearTerminal()
  {
    DisplayText.Clear();
    DisplayTextTMP.text = "";
    InputTextTMP.text = "";
  }

  private void DisplayHelpMessage(int page)
  {
    switch (page)
    {
      case 1:
        AddToDisplay("<b>AVAILABLE COMMANDS</b>");
        AddToDisplay("help x - displays commands list page x");
        AddToDisplay("clear - clears the terminal");
        AddToDisplay("plant x y - plants a seed at x y");
        AddToDisplay("water x y - waters tile at x y");
        AddToDisplay("harvest x y - harvests crop at x y");
        AddToDisplay("upgrades - lists available upgrades");
        AddToDisplay("upgrade name - buys specified upgrade");
        break;
      case 2:
        break;
      default:
        break;
    }

  }

  private void Warn(string txt)
  {
    txt = "<color=#FF0000>" + txt + "</color>";
    AddToDisplay(txt);
  }

  private void Say(string txt)
  {
    txt = "<i>" + txt + "</i>";
    AddToDisplay(txt);
  }

  private void AddToDisplay(string txt)
  {
    string[] splitTxt = txt.Split(' ');
    for (int i = 0; i < splitTxt.Length; i++)
    {
      if (commands.Contains(splitTxt[i]))
      {
        Debug.Log("CONAINS: " + splitTxt[i]);
        splitTxt[i] = "<color=#00FFFF>" + splitTxt[i] + "</color>";
      }
    }
    string finalTxt = string.Join(" ", splitTxt);

    DisplayText.Add("<color=#00FFFF>></color>  " + finalTxt);

    DisplayTextTMP.text = string.Join("\n", DisplayText);
    InputTextTMP.text = "";
  }

  private void AppendDisplayText(string txt)
  {
    AddToDisplay(txt);
    Debug.Log("Added " + txt + " to user input list");
    UserInput.Add(txt);
    lastCommand = txt;
    currentIndex = UserInput.Count;
  }

  private void GetPrevious()
  {
    if (currentIndex - 1 >= 0 && UserInput.Count > currentIndex - 1)
    {
      currentIndex--;
      InputTextTMP.text = UserInput[currentIndex];
      InputTextTMP.MoveTextEnd(false);
    }
  }

  private void GetNext()
  {
    if (currentIndex + 1 >= 0 && currentIndex + 1 < UserInput.Count)
    {
      currentIndex++;
      InputTextTMP.text = UserInput[currentIndex];
      InputTextTMP.MoveTextEnd(false);
    }
  }
}