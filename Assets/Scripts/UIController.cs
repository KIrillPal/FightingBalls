using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
  public GameObject SelectLevelScreen, PauseScreen, PlayerWonScreen, EnemyWonScreen, GameController;

  private GameControllerScript.GameCondition previousCondition = GameControllerScript.GameCondition.ChooseLevelScreen;
  private GameControllerScript gcs;

  private void Start()
  {
    gcs = GameController.GetComponent<GameControllerScript>();
    SetAllScreensInactive();
    SelectLevelScreen.SetActive(true);
  }

  private void Update()
  {
    if (previousCondition != gcs.condition)
    {
      previousCondition = gcs.condition;
      SetAllScreensInactive();
      switch (gcs.condition)
      {
        case GameControllerScript.GameCondition.ChooseLevelScreen:
          SelectLevelScreen.SetActive(true);
          break;
        case GameControllerScript.GameCondition.Pause:
          PauseScreen.SetActive(true);
          break;
        case GameControllerScript.GameCondition.PlayerWon:
          PlayerWonScreen.SetActive(true);
          break;
        case GameControllerScript.GameCondition.EnemiesWon:
          EnemyWonScreen.SetActive(true);
          break;
      }
    }
  }

  void SetAllScreensInactive()
  {
    SelectLevelScreen.SetActive(false);
    PauseScreen.SetActive(false);
    PlayerWonScreen.SetActive(false);
    EnemyWonScreen.SetActive(false);
  }
}
