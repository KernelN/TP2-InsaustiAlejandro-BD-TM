using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SQLConnection : MonoBehaviour
{
    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        //form.AddField("username", usernameField.text);
        UnityWebRequest www = UnityWebRequest.Post
            ("http://localhost/tp2-insaustialejandro-bd-tm/register.php", form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data loaded to the sql database succesfully!");
        }
    }
}
