using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour {
  public void StartShrink(Transform t, float delay) {
    float shrinkFactor = 2f;
    StartCoroutine(Shrink(t, delay, shrinkFactor));
  }

  IEnumerator Shrink(Transform t, float delay, float factor) {
    yield return new WaitForSeconds(delay);

    Vector3 newScale = t.localScale;

    while (newScale.x >= 0)
    {
      newScale -= new Vector3(factor, factor, factor);

      t.localScale = newScale;
      yield return new WaitForSeconds(0.05f);
    }
  }
}
