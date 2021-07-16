using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneMovement : MonoBehaviour
{
  public GameObject pointToFollow;
  private Vector3 positionToFollow;

  private void Update()
  {
    positionToFollow = pointToFollow.transform.position;
    positionToFollow.z = 0f;
    gameObject.transform.position = positionToFollow;
  }
}
