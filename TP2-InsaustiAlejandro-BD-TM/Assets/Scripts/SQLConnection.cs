using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SQLConnection : MonoBehaviour
{
    public Action usernameErrorDetected;
    public Action passwordErrorDetected;
    public Action newUserDetected;
    public Action newUserRegistered;
    public Action signedIn;
    public PlayerData playerData;
    public bool signSuccessful;
    [SerializeField] TMPro.TMP_InputField usernameField;
    [SerializeField] TMPro.TMP_InputField passwordField;
    const string dataBasePath = "http://localhost/tp2-insaustialejandro-bd-tm/";
    const string nameUnexistantError = "3: Name Unexistant";
    const string SQLEmpty = " ";
    const string inputEmpty = "";
    string playerName;
    string playerPassword;
    private void Start()
    {
        signSuccessful = false;
    }
    public void CallSign()
    {
        StartCoroutine(Sign());
    }
    public void CallRegister()
    {
        StartCoroutine(RegisterUser());
        newUserRegistered?.Invoke();
    }

    //SQL Sign funcs
    IEnumerator Sign()
    {
        //check if user entered a username
        if (usernameField.text == inputEmpty)
        {
            usernameErrorDetected?.Invoke();
            yield break;
        }

        //get user data
        StartCoroutine(ReadUser());
        while (playerName == null) { yield return null; }
        //check if user exists
        if (playerName == nameUnexistantError) //if not, register to database
        {
            newUserDetected?.Invoke();
            yield break;
        }
        if (playerPassword == passwordField.text + SQLEmpty) //if pass was right, just save data
        {
            FinishSign();
            yield break;
        }

        if (playerPassword != SQLEmpty) //if had a different pass, throw error
        {
            passwordErrorDetected?.Invoke();
            yield break;
        }
        else //if didn't have pass, update and add one to database
        {
            StartCoroutine(UpdateUser());
            FinishSign();
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
            FinishSign();
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
    void FinishSign()
    {
        signSuccessful = true;
        signedIn?.Invoke();
    }

    //SQL InGame funcs
    IEnumerator UpdateUserGame()
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
        StartCoroutine(UpdateUserGame());
    }
}