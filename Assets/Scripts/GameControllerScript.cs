using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
  public enum GameCondition { ChooseLevelScreen, ActiveFase, Pause, EnemiesWon, PlayerWon};

  public GameCondition condition = GameCondition.ChooseLevelScreen;

  public EnemiesAmountController eaController;
  public EnemyController enemy;

  void Start()
  {
    enemy = GameObject.Find("Enemy").GetComponent<EnemyController>();
  }

  private void OnGUI()
  {
    Event ev = Event.current;
    if (ev.isKey && ev.keyCode == KeyCode.Escape)
      Application.Quit();
    if (ev.isKey && (condition == GameCondition.EnemiesWon || condition == GameCondition.PlayerWon))
    {
      if (ev.keyCode == KeyCode.R)
      {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
    }
  }

  public void IdiotButtonPressed()
  {
    enemy.SetSmartness(EnemyController.BotSmartness.Idiot);
    condition = GameCondition.ActiveFase;
  }

  public void NormalButtonPressed()
  {
    enemy.SetSmartness(EnemyController.BotSmartness.Normal);
    condition = GameCondition.ActiveFase;
  }

  public void UsatyukButtonPressed()
  {
    enemy.SetSmartness(EnemyController.BotSmartness.Usatyuk);
    condition = GameCondition.ActiveFase;
  }

  public void AllOneByOneButtonPressed()
  {
    enemy.gameObject.SetActive(false);
    eaController.BeginWaves();
    condition = GameCondition.ActiveFase;
  }

  public void PlayerDied()
  {
    condition = GameCondition.EnemiesWon;
  }

  public void EnemyDied()
  {
    //if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
      condition = GameCondition.PlayerWon;
  }
}
