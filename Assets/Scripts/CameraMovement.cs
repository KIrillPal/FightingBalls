using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  public GameObject pointToFollow;
  private Vector3 positionToFollow;

  private void Update()
  {
    positionToFollow = pointToFollow.transform.position;
    positionToFollow.z = -10f;
    gameObject.transform.position = positionToFollow;
  }
}
