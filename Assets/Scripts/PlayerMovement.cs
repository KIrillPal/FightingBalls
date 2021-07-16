using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
  public float speed = 5.0f;
  public Rigidbody2D rigidbody;
  public Camera camera;
  public int health = 3;
  public int maxHealth = 6;

  [SerializeField]
  private int middleHealth = 5;

  private float addingHealth;

  private float horizontalInput;
  private float verticalInput;
  private Vector2 mousePosition;

  public GameObject fieldOfViewObject;
  private FieldOfViewScript fieldOfViewScript;
  private GameControllerScript gcs;

  private void Start()
  {
    addingHealth = middleHealth - health;
    fieldOfViewScript = fieldOfViewObject.GetComponent<FieldOfViewScript>();
    gcs = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();

  }

  private void Update()
  {
    if (gcs.condition == GameControllerScript.GameCondition.ActiveFase)
    {
      /* получаем данные с осей X и Y (WS = Y, AD = X)
       * Они приходят в виде вещественных чисел из отрезка [-1.0, 1.0]
       * причем Х = 1 - до упора вправо
       * Y = 1 - до упора вверх
      */
      horizontalInput = Input.GetAxis("Horizontal");
      verticalInput = Input.GetAxis("Vertical");

      mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
      // Input.mousePosition возвращает позицию мыши на экране
      // camera.ScreenToWorldPoint сопоставляет позицию на экране с точкой в игровом мире
    }
    else rigidbody.velocity *= 0.9f;
  }

  private void FixedUpdate()
  {
    if (gcs.condition == GameControllerScript.GameCondition.ActiveFase)
    {
      // создаём вектор (не тот который динамический массив, а тот который в двухмерном пространстве, привыкай)
      Vector2 movement = new Vector2(horizontalInput * speed * Time.fixedDeltaTime, verticalInput * speed * Time.fixedDeltaTime);
      /* 
       * в Unity есть две главных команды обновления состояний:
       * Update - обновление каждый кадр
       * FixedUpdate - обновление каждый определенный отрезок времени
       * Поскольку в данном скрипте мы просчитываем движение, то нам лучше использовать FixedUpdate. Это нужно для того, чтобы скорость персонажа не зависела от 
       * мощности компа игрока.
       * Time.fixedDeltaTime - тот самый отрезок времени, который проходит между двумя вызовами FixedUpdate
       * если его убрать персонаж будет носиться как угорелый
      */
      // дальше просто берем текущую позицию и прибавляем к ней необходимое изменение координат
      rigidbody.MovePosition(rigidbody.position + movement);

      Vector2 positionToLook = mousePosition - rigidbody.position;
      Debug.LogWarning(positionToLook.x + " " + positionToLook.y);
      float rotateAngle = Mathf.Atan2(positionToLook.y, positionToLook.x) * Mathf.Rad2Deg;
      rigidbody.rotation = rotateAngle;

      fieldOfViewScript.SetOrigin(rigidbody.position);
      fieldOfViewScript.SetAimDirection(positionToLook);
    } else
    {
      rigidbody.velocity = new Vector2(0, 0);
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
    --health;
    if (health > 0)
    {
      float newScale = (health + addingHealth) / middleHealth;
      transform.localScale = new Vector3(newScale, newScale, 0);
      UpdateBones();

    }
    else
    {
      gcs.PlayerDied();
    }
  }

  public int GetHealth()
  {
    return health;
  }

  public void UpdateBones()
  {
    GameObject[] bones = GameObject.FindGameObjectsWithTag("PlayerBone");
    //Debug.LogWarning("Found " + bones.Length.ToString() + ":");
    foreach (GameObject bone in bones)
    {
      int num = bone.name[5] - '0' - 1;
      float angle = num * Mathf.PI / 4;
      Vector3 delta = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
      //Debug.LogWarning("angle " + delta.x.ToString() + " " + delta.y.ToString());

      bone.transform.position = transform.position + delta * (transform.localScale.x * 0.3f);
    }
  }
}
