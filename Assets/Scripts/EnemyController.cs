using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyController : MonoBehaviour
{
  public enum BotSmartness { Idiot, Normal, Usatyuk, CheatingUsatyuk };

  public int health = 3;
  public int maxHealth = 6;

  [SerializeField]
  private int middleHealth = 5;

  private float addingHealth;

  public BotSmartness smartness;

  public GameObject shootingPoint;
  public GameObject enemyBulletPrefab;
  public GameObject gun;
  public GameObject enemyBall;

  public GameObject targeter;

  public float standartSD = 8;
  public float standartERD = 6;

  public float runRadius = 5;

  public float shootingDistance = 20f;

  [Header("Idiot Smartness Settings")]
  public float idiotBulletImpulse;
  public double idiotShootingPause = 5;
  public float idiotMissingRange = 3f;
  public float idiotGoAwayDistance = 1;
  public float idiotPathRandom = 0.5f;
  public float idiotSpeed = 8f;

  [Header("Normal Smartness Settings")]
  public float normalBulletImpulse;
  public double normalShootingPause = 5;
  public float normalMissingRange = 3f;
  public float normalGoAwayDistance = 1;
  public float normalPathRandom = 0.5f;
  public float normalSpeed = 8f;

  [Header("Usatyuk Smartness Settings")]
  public float usatyukBulletImpulse;
  public double usatyukShootingPause = 1;
  public float usatyukMissingRange = 1f;
  public float usatyukGoAwayDistance = 1;
  public float usatyukSpeed = 6f;
  public float usatyukPathRandom = 2f;


  private GameControllerScript gcs;
  private AIDestinationSetter destinationSetter;
  private AIPath aIPath;
  private Rigidbody2D rb;
  private float currentBulletImpulse;
  private double currentShootingPause;
  private float currentMissingRange;
  private float currentGoAwayDistance;
  private float currentPathRandom;
  private bool tryingToAvoidBullets = false;
  private bool isRunning = false;
  private double previousShotTime = 0;
  private double previousAvoidTime = 0;
  private double previousBoneTime = 0;
  private double previousDamageTime = 0;
  Vector3 prevousPosition;
  Vector3 lastPlayerPos;
  Vector3 playerSpeed;

  public void SetSmartness(BotSmartness smartness)
  {
    this.smartness = smartness;
    if (smartness == BotSmartness.Normal)
    {
      currentBulletImpulse = normalBulletImpulse;
      currentShootingPause = normalShootingPause;
      currentMissingRange = normalMissingRange;
      currentGoAwayDistance = normalGoAwayDistance;
      currentPathRandom = normalPathRandom;
      aIPath.maxSpeed = normalSpeed;
    }
    else if (smartness == BotSmartness.Usatyuk)
    {
      currentBulletImpulse = usatyukBulletImpulse;
      currentShootingPause = usatyukShootingPause;
      currentMissingRange = usatyukMissingRange;
      currentGoAwayDistance = usatyukGoAwayDistance;
      currentPathRandom = usatyukPathRandom;
      aIPath.maxSpeed = usatyukSpeed;
    }
    else if (smartness == BotSmartness.Idiot)
    {
      currentBulletImpulse = idiotBulletImpulse;
      currentShootingPause = idiotShootingPause;
      currentMissingRange = idiotMissingRange;
      currentGoAwayDistance = idiotGoAwayDistance;
      currentPathRandom = idiotPathRandom;
      aIPath.maxSpeed = idiotSpeed;
    }
  }

  private void Start()
  {
    gcs = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
    destinationSetter = GetComponent<AIDestinationSetter>();
    addingHealth = middleHealth - health;
    rb = GetComponent<Rigidbody2D>();
    aIPath = GetComponent<AIPath>();
    this.smartness = smartness;
    if (smartness == BotSmartness.Normal)
    {
      currentBulletImpulse = normalBulletImpulse;
      currentShootingPause = normalShootingPause;
      currentMissingRange = normalMissingRange;
      currentGoAwayDistance = normalGoAwayDistance;
      currentPathRandom = normalPathRandom;
      aIPath.maxSpeed = normalSpeed;
    }
    else if (smartness == BotSmartness.Usatyuk)
    {
      currentBulletImpulse = usatyukBulletImpulse;
      currentShootingPause = usatyukShootingPause;
      currentMissingRange = usatyukMissingRange;
      currentGoAwayDistance = usatyukGoAwayDistance;
      currentPathRandom = usatyukPathRandom;
      aIPath.maxSpeed = usatyukSpeed;
    }
    else if (smartness == BotSmartness.Idiot)
    {
      currentBulletImpulse = idiotBulletImpulse;
      currentShootingPause = idiotShootingPause;
      currentMissingRange = idiotMissingRange;
      currentGoAwayDistance = idiotGoAwayDistance;
      currentPathRandom = idiotPathRandom;
      aIPath.maxSpeed = idiotSpeed;
    }
  }

  private void Update()
  {
    if (gcs.condition != GameControllerScript.GameCondition.ActiveFase)
    {
      targeter.transform.position = gameObject.transform.position;
    }
    else
    {
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      AvoidBullets();
      if (!tryingToAvoidBullets)
      {
        if (Time.timeAsDouble > previousBoneTime + 3f)
          UpdateBones();
        Vector3 position = player.transform.position;
        aIPath.slowdownDistance = standartSD;
        aIPath.endReachedDistance = standartERD;
        int health_diff = player.GetComponent<PlayerMovement>().GetHealth() - health;
        // здесь определяется необходимость отсупить и найти аптечки
        if (smartness != BotSmartness.Idiot && health_diff > 0 || health_diff > 2)
        {
          GameObject[] healerObjects = GameObject.FindGameObjectsWithTag("Healer");
          Vector3 nearest_pos = player.transform.position;
          float nearest_dist = -1;
          foreach (GameObject healer in healerObjects)
          {
            float dist = Vector3.Distance(position, healer.transform.position);
            if (dist >= runRadius)
            {
              float dist2 = Vector3.Distance(gameObject.transform.position, healer.transform.position);
              if (nearest_dist > dist2 || nearest_dist == -1)
              {
                nearest_dist = dist2;
                nearest_pos = healer.transform.position;
              }
            }
          }
          if (nearest_dist != -1)
          {
            position = nearest_pos;
            aIPath.slowdownDistance = 0;
            aIPath.endReachedDistance = 0;
          }
        }
        targeter.transform.position = position + new Vector3(
        UnityEngine.Random.Range(-currentPathRandom, currentPathRandom),
        UnityEngine.Random.Range(-currentPathRandom, currentPathRandom),
        UnityEngine.Random.Range(-currentPathRandom, currentPathRandom)
        );
      }

      if (Time.timeAsDouble > previousShotTime + currentShootingPause)
      {
        // здесь берётся упреждение
        playerSpeed = player.transform.position - lastPlayerPos;
        float dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
        Vector3 delta = playerSpeed * (dist / (currentBulletImpulse * (float)currentShootingPause));
        Vector3 target = player.transform.position;
        if (smartness != BotSmartness.Idiot)
        {
          if (UnityEngine.Random.Range(1, health) != 1)
            target += delta;
        }
        Shoot(target, dist / 10);
        previousShotTime = Time.timeAsDouble;
        lastPlayerPos = player.transform.position;
      }
    }
  }

  public void IncreaseHealth()
  {
    if (health < maxHealth)
    {
      ++health;
      float newScale = (health + addingHealth) / middleHealth;
      transform.localScale = new Vector3(newScale, newScale, 0);
      UpdateBones();
    }
  }

  public void DecreaseHealth()
  {
    if (Time.timeAsDouble > previousDamageTime + 0.12)
    {
      --health;
      if (health > 0)
      {
        float newScale = (health + addingHealth) / middleHealth;
        transform.localScale = new Vector3(newScale, newScale, 0);
        UpdateBones();
      }
      else
      {
        gameObject.SetActive(false);
        GameObject.Find("Enemies Amount Controller").GetComponent<EnemiesAmountController>().BotDied();
      }
      previousDamageTime = Time.timeAsDouble;
    }
  }

  void Shoot(Vector2 target, float distance)
  {
    target.x += UnityEngine.Random.Range(-currentMissingRange * distance, currentMissingRange * distance);
    target.y += UnityEngine.Random.Range(-currentMissingRange * distance, currentMissingRange * distance);
    RaycastHit2D raycastHit = Physics2D.Raycast(gameObject.transform.position, GetVectorFromAngle(Vector2.Angle(gameObject.transform.position, target)), shootingDistance);
    Vector2 positionToLook = target - rb.position;
    float requiredAngle = Mathf.Atan2(positionToLook.y, positionToLook.x) * Mathf.Rad2Deg - 90;
    gun.transform.rotation = Quaternion.Euler(0, 0, requiredAngle);
    GameObject spawnedBullet = Instantiate(enemyBulletPrefab, shootingPoint.transform.position, shootingPoint.transform.rotation);
    Rigidbody2D rigidbody = spawnedBullet.GetComponent<Rigidbody2D>();
    rigidbody.AddForce(shootingPoint.transform.up * currentBulletImpulse, ForceMode2D.Impulse);
  }

  void AvoidBullets()
  {
    // уровень реакции
    float reactionDelay = 0.05f;
    if (smartness == BotSmartness.Idiot)
      reactionDelay = 0.1f;
    if (Time.timeAsDouble > previousAvoidTime + reactionDelay)
    {
      GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
      if (playerBullets.Length > 0)
      {
        // расчёт функции уворота. скорость уворота определяется переменной move_left. Она может быть как положительной, так и отрицательной(движение вправо)
        float move_left = 0;
        float nearest_dist = -1, nearest_line_dist = -1, nearest_dx = 0, nearest_dy = 0;
        float myX = gameObject.transform.position.x;
        float myY = gameObject.transform.position.y;
        // поиск самых 
        foreach (GameObject bullet in playerBullets)
        {
          float velX = bullet.GetComponent<Rigidbody2D>().velocity.x, velY = bullet.GetComponent<Rigidbody2D>().velocity.y;
          float tga = bullet.GetComponent<Rigidbody2D>().velocity.y / bullet.GetComponent<Rigidbody2D>().velocity.x;
          float dist = Vector3.Distance(bullet.transform.position, gameObject.transform.position);

          float bulletX = bullet.transform.position.x;
          float bulletY = bullet.transform.position.y;
          float distX = myX - bulletX;
          float distY = myY - bulletY;
          if (velX * distX + velY * distY > 0)
          {
            if (dist < nearest_dist || nearest_dist == -1)
            {
              nearest_dist = dist;
              nearest_dx = velX;
              nearest_dy = velY;
            }
            float sina = (velX * distY - distX * velY) / (dist * (float)Math.Sqrt(velX * velX + velY * velY));
            if (nearest_line_dist > sina * dist)
              nearest_line_dist = sina * dist;
            move_left += sina * dist;
          }
        }
        if (nearest_dist != -1 && nearest_line_dist <= 1.5 && nearest_dist < 5)
        {

          float motive = 2f;
          if (smartness == BotSmartness.Idiot)
          {
            motive = 0.02f;
          }
          float targetX = myX - nearest_dy * motive * (move_left > 0 ? 1 : -1);
          float targetY = myY - motive * (-nearest_dx) * (move_left > 0 ? 1 : -1);
          float playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().GetHealth();
          // to_attack отвечает за умение кружить вокруг противника. 
          // Также на больших расстояниях он работает как умение обходить противника и заходиь с неожиданной стороны
          // Отношение (health / playerHealth) в таком случае работает как коэффициент случайности этого поведения, т.к. to_attack даёт весьма небольшой импульс
          float to_attack = 0.2f * motive * (health / playerHealth);
          targetX -= nearest_dx * to_attack;
          targetY -= nearest_dy * to_attack;

          //Debug.LogWarning("Avoid at " + (targetX - myX).ToString() + " " + (targetY - myY).ToString());
          targeter.transform.position = new Vector3(targetX, targetY, 1);
          aIPath.slowdownDistance = 0;
          aIPath.endReachedDistance = 0;

          tryingToAvoidBullets = true;
          //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
          previousAvoidTime = Time.timeAsDouble;
        }
        else
        {
          tryingToAvoidBullets = false;
          aIPath.slowdownDistance = standartSD;
          aIPath.endReachedDistance = standartERD;
          targeter.transform.position = GameObject.FindGameObjectWithTag("Player").
           transform.position + new Vector3(
           UnityEngine.Random.Range(-currentPathRandom, currentPathRandom),
           UnityEngine.Random.Range(-currentPathRandom, currentPathRandom),
           UnityEngine.Random.Range(-currentPathRandom, currentPathRandom)
         );
        }
      }
      else
      {
        tryingToAvoidBullets = false;
        aIPath.slowdownDistance = standartSD;
        aIPath.endReachedDistance = standartERD;
        targeter.transform.position = GameObject.FindGameObjectWithTag("Player").
           transform.position + new Vector3(
           UnityEngine.Random.Range(-currentPathRandom, currentPathRandom),
           UnityEngine.Random.Range(-currentPathRandom, currentPathRandom),
           UnityEngine.Random.Range(-currentPathRandom, currentPathRandom)
         );
      }
    }
  }

  private Vector3 GetVectorFromAngle(float angle)
  {
    float angleRad = angle * Mathf.Deg2Rad;
    return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
  }

  public void UpdateBones()
  {
    for (int num = 1; num <= 8; ++num)
    {
      Transform bone_transform = enemyBall.transform.Find("bone_" + num.ToString());
      float angle = (num - 1) * Mathf.PI / 4;
      Vector3 delta = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
      bone_transform.position = gameObject.transform.position + delta * (transform.localScale.x * 0.3f);
    }
    previousBoneTime = Time.timeAsDouble;
  }
}