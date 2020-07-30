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
	public TMP_InputField InputTextTMP;
	public List<string> DisplayText = new List<string>();

	private string[] commands = { "help", "clear" };
	private string lastCommand = "";
	private int currentIndex = 0;

	private void Start()
		{
		AddToDisplay("Welcome. Type help for a list of commands.");
		}
	// Update is called once per frame
	void Update()
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
			default:
				Debug.LogWarning("UNHANDLED COMMAND: " + args[0]);
				break;
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
		AddToDisplay("AVAILABLE COMMANDS");
		AddToDisplay("help - displays commands list");
		AddToDisplay("clear - clears the terminal");
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
