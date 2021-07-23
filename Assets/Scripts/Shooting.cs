using UnityEngine;

public class Shooting : MonoBehaviour
{
  public Transform firePosition;
  public GameObject bulletPrefab;
  public float bulletImpulse = 20f;

  private GameControllerScript gcs;
  private EnemiesAmountController eac;
  private double previousShot = 0f;

  private void Start()
  {
    gcs = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
    eac = GameObject.FindGameObjectWithTag("EnemiesAmountController").GetComponent<EnemiesAmountController>();
  }

  private void Update()
  {
    if (eac.makingWaves && Input.GetMouseButton(0) && gcs.condition != GameControllerScript.GameCondition.EnemiesWon)
      Shoot();
    if (!eac.makingWaves && Input.GetMouseButtonDown(0) && gcs.condition == GameControllerScript.GameCondition.ActiveFase) // �� ��������� Fire1 = ���, �� ���� ���� ��� ����� ��������
    {
      Shoot();
    }
  }

  private void Shoot()
  {
    // Instaniate(...) ��� ������ ���������� ������ ���� (bulletPrefab) � ������� ����� �������� (firePosition.position)
    // � ��������� �� ��� �� ����, �� ������� ��������� ����� �������� (firePosition.rotation)
    // ��� �� �����������, �� � ������ ������ �� �������� ������������ ������ � spawnedBullet ����� ����� ���� ��� �����
    if (Time.timeAsDouble > previousShot + 0.07)
    {
      GameObject spawnedBullet = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
      // �������� ������ � ���������� Rigidbody2D � �������������� ����
      Rigidbody2D rigidbody = spawnedBullet.GetComponent<Rigidbody2D>();
      // ��� ����� ��� ��� � ��������������� ����������� � � ��������������� �����
      rigidbody.AddForce(firePosition.right * bulletImpulse, ForceMode2D.Impulse);
      gameObject.GetComponent<AudioSource>().Play();
      previousShot = Time.timeAsDouble;
    }
  }
}
