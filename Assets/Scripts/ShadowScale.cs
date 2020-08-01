using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScale : MonoBehaviour
	{
	private void LateUpdate()
		{
		var amount = -Mathf.Sin(Time.time * .5f) * 0.000015f;
		transform.localScale += new Vector3(amount, amount, 0);
		}
	}