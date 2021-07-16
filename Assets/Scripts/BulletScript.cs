using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{  
  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.gameObject.tag.Equals("Player") && gameObject.tag.Equals("EnemyBullet"))
    {
      collision.collider.gameObject.GetComponent<PlayerMovement>().DecreaseHealth();
    }
    else if (collision.collider.gameObject.tag.Equals("PlayerBone") && gameObject.tag.Equals("EnemyBullet"))
    {
      GameObject bone = collision.collider.gameObject;
      bone.transform.parent.gameObject.GetComponent<PlayerMovement>().DecreaseHealth();
    }
    else if (collision.collider.gameObject.tag.Equals("Enemy") && gameObject.tag.Equals("PlayerBullet"))
    {
      collision.collider.gameObject.GetComponent<EnemyController>().DecreaseHealth();
    }
    else if (collision.collider.gameObject.tag.Equals("EnemyBone") && gameObject.tag.Equals("PlayerBullet"))
    {
      GameObject bone = collision.collider.gameObject;
      bone.transform.parent.parent.gameObject.GetComponent<EnemyController>().DecreaseHealth();
    }
    //else Debug.LogWarning("Bullet collision: " + collision.collider.gameObject.tag);
    Destroy(gameObject);
  }
}
