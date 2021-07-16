using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerControllScript : MonoBehaviour
{
  [SerializeField]
  private int maxAmountOfHealers = 10;
  private int currentAmountOfHealers = 0;

  [SerializeField]
  private GameObject healerPrefab;
  [SerializeField]
  private GameObject topLeftPoint;
  [SerializeField]
  private GameObject downRightPoint;

  private float minX, maxX, minY, maxY;

  private void Start()
  {
    minX = topLeftPoint.transform.position.x;
    maxX = downRightPoint.transform.position.x;
    minY = downRightPoint.transform.position.y;
    maxY = topLeftPoint.transform.position.y;
  }

  private void Update()
  {
    while (currentAmountOfHealers < maxAmountOfHealers)
    {
      SpawnNewHealer();
      ++currentAmountOfHealers;
    }
  }

  private void SpawnNewHealer()
  {
    float spawnX = Random.Range(minX, maxX);
    float spawnY = Random.Range(minY, maxY);
    float spawnZ = 1f;
    Instantiate(healerPrefab, new Vector3(spawnX, spawnY, spawnZ), Quaternion.Euler(0, 0, 0));
  }

  public void HealerWasDestroyed()
  {
    --currentAmountOfHealers;
  }
}
