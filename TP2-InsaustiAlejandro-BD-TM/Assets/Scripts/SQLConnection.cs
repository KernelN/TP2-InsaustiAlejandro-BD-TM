using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SQLConnection : MonoBehaviour
{
    public bool signSuccessful;
    [SerializeField] TMPro.TMP_InputField usernameField;
    [SerializeField] TMPro.TMP_InputField passwordField;
    [SerializeField] GameObject usernameError;
    [SerializeField] GameObject passwordError;
    [SerializeField] GameObject registerPanel;
    [SerializeField] RectTransform signPanel;
    [SerializeField] PlayerController playerController;
    [SerializeField] float hidePanelTime;
    const string dataBasePath = "http://localhost/tp2-insaustialejandro-bd-tm/";
    const string nameUnexistantError = "3: Name Unexistant";
    const string SQLEmpty = " ";
    const string inputEmpty = "";
    Vector3 originalSignPanelPos;
    float hidePanelTimer;
    string playerName;
    string playerPassword;
    private void Start()
    {
        signSuccessful = false;
        hidePanelTimer = 0;
        originalSignPanelPos = signPanel.anchoredPosition;
    }
    public void CallSign()
    {
        StartCoroutine(Sign());
    }
    public void CallRegister()
    {
        StartCoroutine(RegisterUser());
        registerPanel.SetActive(false);
    }

    //SQL Sign funcs
    IEnumerator Sign()
    {
        //check if user entered a username
        if (usernameField.text == inputEmpty)
        {
            usernameError.SetActive(true);
            yield break;
        }

        //get user data
        StartCoroutine(ReadUser());
        while (playerName == null) { yield return null; }
        //check if user exists
        if (playerName == nameUnexistantError) //if not, register to database
        {
            registerPanel.SetActive(true);
            yield break;
        }
        if (playerPassword == passwordField.text + SQLEmpty) //if pass was right, just save data
        {
            StartCoroutine(FinishSign());
            yield break;
        }

        if (playerPassword != SQLEmpty) //if had a different pass, throw error
        {
            passwordError.SetActive(true);
            yield break;
        }
        else //if didn't have pass, update and add one to database
        {
            StartCoroutine(UpdateUser());
            StartCoroutine(FinishSign());
            yield break;
        }
    }
    IEnumerator RegisterUser()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www = new UnityWebRequest();

        form.AddField("username", usernameField.text);
        playerName = usernameField.text;
        if (passwordField.text != inputEmpty)
        {
            form.AddField("password", passwordField.text);
            playerPassword = passwordField.text + SQLEmpty;
            www = UnityWebRequest.Post(dataBasePath + "register.php", form);
        }
        else
        {
            www = UnityWebRequest.Post(dataBasePath + "registerUsername.php", form);
        }
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data loaded to the sql database succesfully!");
            StartCoroutine(FinishSign());
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    }
    IEnumerator UpdateUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);
        UnityWebRequest www = UnityWebRequest.Post(dataBasePath + "update.php", form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data updated to the sql database succesfully!");
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    }
    IEnumerator ReadUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        UnityWebRequest www = UnityWebRequest.Post(dataBasePath + "read.php", form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data loaded from the sql database succesfully!");
            string originalText = www.downloadHandler.text;
            string[] textParts = originalText.Split('\t');
            playerName = textParts[0];
            if (playerName != nameUnexistantError)
            {
                playerPassword = textParts[1];
            }
            Debug.Log(originalText);
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    }
    //Unity Sign funcs
    IEnumerator FinishSign()
    {
        signSuccessful = true;
        do
        {
            signPanel.anchoredPosition = Vector3.Lerp(originalSignPanelPos, -originalSignPanelPos, hidePanelTimer / hidePanelTime);
            hidePanelTimer += Time.deltaTime;
            yield return null;
        } while (hidePanelTimer <= hidePanelTime);
        hidePanelTimer = 0;
        signPanel.gameObject.SetActive(false);
        yield break;
    }

    //SQL InGame funcs
    IEnumerator UpdateUserGame(PlayerData playerData)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", playerName);
        form.AddField("score", playerData.score.ToString());
        form.AddField("deaths", playerData.deaths.ToString());
        UnityWebRequest www = UnityWebRequest.Post(dataBasePath + "updatePlayerData.php", form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data updated to the sql database succesfully!");
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    }
    //Unity InGameFuncs
    public void SavePlayerDataToDatabase()
    {
        StartCoroutine(UpdateUserGame(playerController.data));
    }
}