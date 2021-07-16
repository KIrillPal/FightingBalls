using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAmountController : MonoBehaviour
{
  public EnemyController.BotSmartness smartness = EnemyController.BotSmartness.Idiot;
  public GameObject enemyPrefab;
  public int currentAmountOfBots = 1;
  public GameObject waves;
  public bool makingWaves = false;
  private int wavesLeft = 0;
  private double previousBoneTime = 0;


  private void Update()
  {
    if (makingWaves)
    {
      if (currentAmountOfBots == 0)
      {
        if (wavesLeft == 2)
        {
          waves.transform.GetChild(1).gameObject.SetActive(true);
          currentAmountOfBots = 2;
          wavesLeft = 1;
        } else if (wavesLeft == 1)
        {
          waves.transform.GetChild(2).gameObject.SetActive(true);
          currentAmountOfBots = 1;
          wavesLeft = 0;
        }
      }
    }
  }

  public void BeginWaves()
  {
    wavesLeft = 2;
    waves.transform.GetChild(0).gameObject.SetActive(true);
    makingWaves = true;
    currentAmountOfBots = 3;
  }

  public void BotDied()
  {
    --currentAmountOfBots;
    if (currentAmountOfBots == 0 && wavesLeft == 0)
    {
      GameObject.Find("Game Controller").GetComponent<GameControllerScript>().EnemyDied();
    }
  }

}
