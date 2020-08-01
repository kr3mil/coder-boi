using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FarmBlock : MonoBehaviour
	{
	public Sprite BlockTexture;
	public Sprite SeedTexture;
	public Sprite WaterTexture;

	public float Growth;

	private bool isWatered;
	private bool isPlanted;
	private int chanceToGrow = 5;
	private int chanceToDry = 20;
	private float timeLeft = Config.TickRate;
	private SpriteRenderer blockRenderer;
	private SpriteRenderer seedRenderer;
	private SpriteRenderer waterRenderer;

	private void Start()
		{
		var renderers = GetComponentsInChildren<SpriteRenderer>();
		blockRenderer = renderers[0];
		waterRenderer = renderers[1];
		seedRenderer = renderers[2];
		}

	private void Update()
		{
		timeLeft -= Time.deltaTime;
		if (timeLeft <= 0)
			{
			if (isPlanted && Growth < 100)
				{
				Grow();
				}
			if (isWatered)
				Dry();
			timeLeft = Config.TickRate;
			}
		}

	public bool Water()
		{
		if (!isWatered)
			{
			waterRenderer.sprite = WaterTexture;
			return isWatered = true;
			}
		return false;
		}

	public bool Plant()
		{
		if (!isPlanted)
			{
			seedRenderer.sprite = SeedTexture;
			return isPlanted = true;
			}
		return false;
		}

	public bool Harvest()
		{
		if (Growth == 100)
			{
			isPlanted = false;
			seedRenderer.sprite = null;
			return true;
			}
		return false;
		}

	private void Dry()
		{
		if (Random.Range(0, chanceToDry) == 0)
			{
			Debug.Log("DRY");
			isWatered = false;
			waterRenderer.sprite = null;
			}
		}

	private void Grow()
		{
		if (Random.Range(0, chanceToGrow) == 0)
			{
			Debug.Log("GROW");
			Growth += isWatered ? 33.3f : 12.5f;
			if (Growth > 99)
				{
				Growth = 100;
				}
			}
		}
	}