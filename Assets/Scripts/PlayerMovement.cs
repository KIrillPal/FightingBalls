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
      /* �������� ������ � ���� X � Y (WS = Y, AD = X)
       * ��� �������� � ���� ������������ ����� �� ������� [-1.0, 1.0]
       * ������ � = 1 - �� ����� ������
       * Y = 1 - �� ����� �����
      */
      horizontalInput = Input.GetAxis("Horizontal");
      verticalInput = Input.GetAxis("Vertical");

      mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
      // Input.mousePosition ���������� ������� ���� �� ������
      // camera.ScreenToWorldPoint ������������ ������� �� ������ � ������ � ������� ����
    }
    else rigidbody.velocity *= 0.9f;
  }

  private void FixedUpdate()
  {
    if (gcs.condition == GameControllerScript.GameCondition.ActiveFase)
    {
      // ������ ������ (�� ��� ������� ������������ ������, � ��� ������� � ���������� ������������, ��������)
      Vector2 movement = new Vector2(horizontalInput * speed * Time.fixedDeltaTime, verticalInput * speed * Time.fixedDeltaTime);
      /* 
       * � Unity ���� ��� ������� ������� ���������� ���������:
       * Update - ���������� ������ ����
       * FixedUpdate - ���������� ������ ������������ ������� �������
       * ��������� � ������ ������� �� ������������ ��������, �� ��� ����� ������������ FixedUpdate. ��� ����� ��� ����, ����� �������� ��������� �� �������� �� 
       * �������� ����� ������.
       * Time.fixedDeltaTime - ��� ����� ������� �������, ������� �������� ����� ����� �������� FixedUpdate
       * ���� ��� ������ �������� ����� �������� ��� ��������
      */
      // ������ ������ ����� ������� ������� � ���������� � ��� ����������� ��������� ���������
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
