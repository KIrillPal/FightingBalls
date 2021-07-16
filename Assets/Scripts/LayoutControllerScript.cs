using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutControllerScript : MonoBehaviour
{
  [SerializeField]
  private GameObject layoutsObject;

  private void Start()
  {
    Transform layoutsObjectTransform = layoutsObject.transform;
    foreach(Transform child in layoutsObjectTransform)
    {
      child.gameObject.SetActive(false);
    }

    int activeLayoutIndex = Mathf.CeilToInt(Random.Range(0f, layoutsObjectTransform.childCount));
    activeLayoutIndex = Mathf.Min(3, activeLayoutIndex);
    //Debug.LogWarning(activeLayoutIndex);
    layoutsObjectTransform.GetChild(activeLayoutIndex).gameObject.SetActive(true);
  }
}
