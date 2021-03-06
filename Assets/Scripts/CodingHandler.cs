﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CodingHandler : MonoBehaviour
	{
	public TextMeshProUGUI DisplayTextTMP;
	public TMP_InputField InputTextTMP;
	public List<string> DisplayText = new List<string>();
	public FarmManager FarmManager;

	private string[] commands = { "help", "clear", "plant", "harvest", "water" };
	private string lastCommand = "";
	private int currentIndex = 0;

	private void Start()
		{
		AddToDisplay("<i>Welcome. Type help for a list of commands.</i>");
		}

	// Update is called once per frame
	private void Update()
		{
		if (Input.GetKeyDown(KeyCode.Return))
			{
			Debug.Log("PRESSED RETURN");
			AppendDisplayText(InputTextTMP.text);
			HandleCommand();
			}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
			Debug.Log("PRESSED UP");
			GetPrevious();
			}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
			Debug.Log("PRESSED DOWN");
			GetNext();
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
				DisplayHelpMessage();
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

	private void ClearTerminal()
		{
		DisplayText.Clear();
		DisplayTextTMP.text = "";
		InputTextTMP.text = "";
		}

	private void DisplayHelpMessage()
		{
		AddToDisplay("<b>AVAILABLE COMMANDS</b>");
		AddToDisplay("help - displays commands list");
		AddToDisplay("clear - clears the terminal");
		AddToDisplay("plant x y - plants a seed at x y");
		AddToDisplay("water x y - waters tile at x y");
		AddToDisplay("harvest x y - harvest crop at x y");
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
		lastCommand = txt;
		currentIndex = DisplayText.Count;
		}

	private void GetPrevious()
		{
		if (currentIndex - 1 >= 0 && DisplayText.Count > currentIndex - 1)
			{
			currentIndex--;
			InputTextTMP.text = DisplayText[currentIndex];
			InputTextTMP.MoveTextEnd(false);
			}
		}

	private void GetNext()
		{
		if (currentIndex + 1 >= 0 && currentIndex < DisplayText.Count)
			{
			currentIndex++;
			InputTextTMP.text = DisplayText[currentIndex];
			InputTextTMP.MoveTextEnd(false);
			}
		}
	}