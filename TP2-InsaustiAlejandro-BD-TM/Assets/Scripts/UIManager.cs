using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //SQL
    [SerializeField] GameObject usernameError;
    [SerializeField] GameObject passwordError;
    [SerializeField] GameObject registerPanel;
    [SerializeField] RectTransform signPanel;
    [SerializeField] SQLConnection sqlConnector;
    [SerializeField] float hidePanelTime;
    Vector3 originalSignPanelPos;
    float hidePanelTimer;
    //InGame
    [SerializeField] GameObject inGameUI;
    [SerializeField] TMPro.TextMeshProUGUI time;
    [SerializeField] TMPro.TextMeshProUGUI score;
    [SerializeField] TMPro.TextMeshProUGUI deaths;
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] PlayerController player;

    private void Start()
    {
        //Get Sign Panel Relevant Info
        hidePanelTimer = 0;
        originalSignPanelPos = signPanel.anchoredPosition;
        //Link with Actions
        sqlConnector.usernameErrorDetected += onUserNameError;
        sqlConnector.passwordErrorDetected += onPasswordError;
        sqlConnector.newUserDetected += onNewUserDetected;
        sqlConnector.newUserRegistered += onNewUserRegistration;
        sqlConnector.signedIn += onSuccesfulSignIn;
        gameplayManager.gameStarted += onGameStart;
        gameplayManager.oneSecondPassed += onSecondPass;
        player.scoreChanged += onScoreUpdate;
        player.playerDied += onDeathsCounterUpdate;
    }

    //SQL
    void onUserNameError()
    {
        usernameError.SetActive(true);
    }
    void onPasswordError()
    {
        passwordError.SetActive(true);
    }
    void onNewUserDetected()
    {
        registerPanel.SetActive(true);
    }
    void onNewUserRegistration()
    {
        registerPanel.SetActive(false);
    }
    void onSuccesfulSignIn()
    {
        StartCoroutine(HideSignInPanel());
    }
    IEnumerator HideSignInPanel()
    {
        do
        {
            signPanel.anchoredPosition = Vector3.Lerp(originalSignPanelPos, -originalSignPanelPos, hidePanelTimer / hidePanelTime);
            hidePanelTimer += Time.deltaTime;
            yield return null;
        } while (hidePanelTimer <= hidePanelTime);
        hidePanelTimer = 0;
        signPanel.gameObject.SetActive(false);
    }
    //InGame
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
}