using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SQLConnection : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField usernameField;
    [SerializeField] TMPro.TMP_InputField passwordField;
    const string dataBasePath = "http://localhost/tp2-insaustialejandro-bd-tm/";
    string empty = "";
    string playerName;
    string playerPassword;
    public void CallRegister()
    {
        StartCoroutine(RegisterUser());
    }

    IEnumerator RegisterUser()
    {
        if (usernameField.text == empty) { yield break; }
        //GetPassword();
        //if (playerPassword != "0" && passwordField.text != playerPassword)
        //{

        //}

        WWWForm form = new WWWForm();
        UnityWebRequest www = new UnityWebRequest();

        form.AddField("username", usernameField.text);
        if (passwordField.text != empty)
        {
            form.AddField("password", passwordField.text);
            www = UnityWebRequest.Post(dataBasePath + "register.php", form);
        }
        else
        {
            //use "registerUserOnly.php"
            www = UnityWebRequest.Post(dataBasePath + "register.php", form);
        }
        yield return www.SendWebRequest();
        Debug.Log("Echo: " + www.downloadHandler.text);
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data loaded to the sql database succesfully!");
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
        if (passwordField.text != null)
        {
            form.AddField("password", passwordField.text);
        }
        else
        {
            form.AddField("password", 0);
        }
        UnityWebRequest www = UnityWebRequest.Post(dataBasePath + "update.php", form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data loaded to the sql database succesfully!");
        }
    }
    IEnumerator ReadUser()
    {
        if (usernameField.text == "") { yield break; }
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        if (passwordField.text != empty)
        {
            form.AddField("password", passwordField.text);
        }
        else
        {
            form.AddField("password", 0);
        }
        UnityWebRequest www = UnityWebRequest.Post(dataBasePath+"read.php", form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data loaded from the sql database succesfully!");
            string originalText = www.downloadHandler.text;
            string[] textParts = originalText.Split('\t');
            playerName = textParts[0];
            playerPassword = textParts[1];
            Debug.Log(originalText);
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    }
    private void GetPassword()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        if (passwordField.text != empty)
        {
            form.AddField("password", passwordField.text);
        }
        else
        {
            form.AddField("password", 0);
        }
        UnityWebRequest www = UnityWebRequest.Post(dataBasePath + "read.php", form);
        www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data loaded from the sql database succesfully!");
            string originalText = www.downloadHandler.text;
            playerPassword = originalText;
            Debug.Log(originalText);
            Debug.Log(www.downloadHandler.text);
        }
    }
}
