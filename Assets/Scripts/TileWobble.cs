using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWobble : MonoBehaviour
{
  private void FixedUpdate()
  {
    transform.position += new Vector3(0, .003f * Mathf.Sin(Time.time * .75f), 0);
  }
}