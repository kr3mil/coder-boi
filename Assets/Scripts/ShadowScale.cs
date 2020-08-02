using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScale : MonoBehaviour
{
  private void FixedUpdate()
  {
    var amount = -Mathf.Sin(Time.time * .75f) * 0.001f;
    transform.localScale += new Vector3(amount, amount, 0);
  }
}