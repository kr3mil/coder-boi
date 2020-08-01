using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWobble : MonoBehaviour
	{
	private void LateUpdate()
		{
		transform.position += new Vector3(0, 0.00012f * Mathf.Sin(Time.time * .5f), 0);
		}
	}