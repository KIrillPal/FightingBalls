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
    if (!eac.makingWaves && Input.GetMouseButtonDown(0) && gcs.condition == GameControllerScript.GameCondition.ActiveFase) // по умолчанию Fire1 = Ћ ћ, но если надо это можно помен€ть
    {
      Shoot();
    }
  }

  private void Shoot()
  {
    // Instaniate(...) это значит заспавнить префаб пули (bulletPrefab) в позицию точки выстрела (firePosition.position)
    // и повернуть на тот же угол, на который повернута точка выстрела (firePosition.rotation)
    // это не об€зательно, но в данном случае мы сохраним получившийс€ объект в spawnedBullet чтобы потом дать ему пинка
    if (Time.timeAsDouble > previousShot + 0.07)
    {
      GameObject spawnedBullet = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
      // добываем доступ к компоненту Rigidbody2D у новоиспеченной пули
      Rigidbody2D rigidbody = spawnedBullet.GetComponent<Rigidbody2D>();
      // даЄм пинок под зад в соответствующем направлении и с соответствующей силой
      rigidbody.AddForce(firePosition.right * bulletImpulse, ForceMode2D.Impulse);
      gameObject.GetComponent<AudioSource>().Play();
      previousShot = Time.timeAsDouble;
    }
  }
}
