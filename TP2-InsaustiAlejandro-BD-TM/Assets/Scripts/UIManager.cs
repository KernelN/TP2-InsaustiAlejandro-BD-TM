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
        //gameplayManager.gameStarted += onNewUserRegistration;
        //gameplayManager.oneSecondPassed += onNewUserRegistration;
    }

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
}