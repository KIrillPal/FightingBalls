using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPointScript : MonoBehaviour
{
  public float spinningSpeed = 180f;

  public string healerControllerName = "Healer Controller";

  private GameObject healerController;

  private Rigidbody2D rb;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    healerController = GameObject.Find(healerControllerName);
  }

  void FixedUpdate()
  {
    rb.MoveRotation(rb.rotation + spinningSpeed * Time.fixedDeltaTime);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    //Debug.LogWarning("Collision: " + collision.collider.gameObject.tag);
    if (collision.collider.gameObject.tag.Equals("Player"))
    {
      collision.collider.gameObject.GetComponent<PlayerMovement>().IncreaseHealth();
    }
    else if (collision.collider.gameObject.tag.Equals("Enemy"))
    {
      collision.collider.gameObject.GetComponent<EnemyController>().IncreaseHealth();
    }
    else if (collision.collider.gameObject.tag.Equals("PlayerBone"))
    {
      GameObject bone = collision.collider.gameObject;
      bone.transform.parent.gameObject.GetComponent<PlayerMovement>().IncreaseHealth();
    }
    else if (collision.collider.gameObject.tag.Equals("EnemyBone"))
    {
      GameObject bone = collision.collider.gameObject;
      bone.transform.parent.parent.gameObject.GetComponent<EnemyController>().IncreaseHealth();
    }
    //else Debug.LogWarning("Collision " + collision.collider.gameObject.tag);
    Destroy(gameObject);
  }

  private void OnDestroy()
  {
    if (healerController != null) healerController.GetComponent<HealerControllScript>().HealerWasDestroyed();
  }
}
