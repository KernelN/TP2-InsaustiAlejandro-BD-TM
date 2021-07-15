using System.Collections;
using UnityEngine;

public class UIInGame : MonoBehaviour
{
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject gameOver;
    [SerializeField] TMPro.TextMeshProUGUI time;
    [SerializeField] TMPro.TextMeshProUGUI score;
    [SerializeField] TMPro.TextMeshProUGUI deaths;
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] PlayerController player;

    void Start()
    {
        gameplayManager.gameStarted += onGameStart;
        gameplayManager.oneSecondPassed += onSecondPass;
        gameplayManager.gameEnded += onGameOver;
        player.scoreChanged += onScoreUpdate;
        player.playerDied += onDeathsCounterUpdate;
    }

    void onGameStart()
    {
        inGameUI.SetActive(true);
    }
    void onSecondPass()
    {
        int minutesPassed = gameplayManager.currentTime / 60;
        int secondsPassed = gameplayManager.currentTime % 60;
        time.text = minutesPassed.ToString("00") + ":" + secondsPassed.ToString("00");
    }
    void onScoreUpdate()
    {
        score.text = player.data.score.ToString();
    }
    void onDeathsCounterUpdate()
    {
        deaths.text = player.data.deaths.ToString();
    }
    void onGameOver()
    {
        gameOver.SetActive(true);
        int minutesPassed = gameplayManager.maxTime / 60;
        int secondsPassed = gameplayManager.maxTime % 60;
        time.text = minutesPassed.ToString("00") + ":" + secondsPassed.ToString("00");
    }
}
