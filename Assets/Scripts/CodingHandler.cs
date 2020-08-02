using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CodingHandler : MonoBehaviour
{
  public TextMeshProUGUI DisplayTextTMP;
  public TextMeshProUGUI BackgroundTextTMP;
  public TMP_InputField InputTextTMP;
  public List<string> UserInput = new List<string>();
  public FarmManager FarmManager;

  private Upgrade[] upgrades = {
    new Upgrade("grid tier 2", "upgrades the grid to a 2x2", 5)
    };
  private string[] commands = { "help", "clear", "plant", "harvest", "water", "upgrades", "upgrade" };
  private string lastCommand = "";
  private int currentIndex = 0;
  private bool displayingText = false;

  private List<Tuple<string, bool>> DisplayTextQueue = new List<Tuple<string, bool>>();

  private void Start()
  {
    Array.Sort(commands, (x, y) => String.Compare(x, y));
    AddToDisplay("Welcome. Type help for a list of commands.", false);
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
      AddToDisplay("seed planted at " + args[1] + " " + args[2], false);
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
      AddToDisplay("tile " + args[1] + " " + args[2] + " harvested", false);
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
      AddToDisplay("tile " + args[1] + " " + args[2] + " watered", false);
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
    DisplayTextTMP.text = "";
    InputTextTMP.text = "";
  }

  private void DisplayHelpMessage(int page)
  {
    switch (page)
    {
      case 1:
        AddToDisplay("<b>AVAILABLE COMMANDS</b>", false);
        AddToDisplay("help x - displays commands list page x", false);
        AddToDisplay("clear - clears the terminal", false);
        AddToDisplay("plant x y - plants a seed at x y", false);
        AddToDisplay("water x y - waters tile at x y", false);
        AddToDisplay("harvest x y - harvests crop at x y", false);
        AddToDisplay("upgrades - lists available upgrades", false);
        AddToDisplay("upgrade name - buys specified upgrade", false);
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
    AddToDisplay(txt, false);
  }

  private void Say(string txt)
  {
    txt = "<i>" + txt + "</i>";
    AddToDisplay(txt, false);
  }

  private void AddToDisplay(string txt, bool isUser)
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

    DisplayTextQueue.Add(new Tuple<string, bool>("<color=#00FFFF>></color>  " + finalTxt, isUser));
    if (!displayingText)
    {
      StartCoroutine(ShowText());
    }
    InputTextTMP.text = "";
  }

  IEnumerator ShowText()
  {
    if (DisplayTextQueue.Count > 0)
    {
      displayingText = true;
      var fullText = DisplayTextQueue.First().Item1;
      var isUser = DisplayTextQueue.First().Item2;
      //var isUser = true;

      Debug.Log("Displaying: " + fullText);

      if (DisplayTextTMP.text != "")
      {
        DisplayTextTMP.text += "\n";
      }

      if (!isUser)
      {
        List<string> groups = new List<string>();

        // Group colour and letters into each
        while (fullText.Length > 0)
        {
          string group = fullText[0].ToString();

          // Check for colour
          if (group == "<")
          {
            if (fullText.StartsWith("<color"))
            {
              group = fullText.Substring(0, fullText.IndexOf("</color>") + 8);
              fullText = fullText.Substring(fullText.IndexOf("</color>") + 7);
            }
            else if (fullText.StartsWith("<b"))
            {
              group = fullText.Substring(0, fullText.IndexOf("</b>") + 4);
              fullText = fullText.Substring(fullText.IndexOf("</b>") + 3);
            }
            else if (fullText.StartsWith("<i"))
            {
              group = fullText.Substring(0, fullText.IndexOf("</i>") + 4);
              fullText = fullText.Substring(fullText.IndexOf("</i>") + 3);
            }
          }
          groups.Add(group);
          fullText = fullText.Substring(1);
        }

        foreach (var group in groups)
        {
          yield return new WaitForSeconds(.02f);
          DisplayTextTMP.text += group;
        }
      }
      else
      {
        DisplayTextTMP.text += fullText;
      }
      DisplayTextQueue.RemoveAt(0);
    }
    if (DisplayTextQueue.Count > 0)
    {
      StartCoroutine(ShowText());
    }
    else
    {
      displayingText = false;
    }
  }

  private void AppendDisplayText(string txt)
  {
    AddToDisplay(txt, true);
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