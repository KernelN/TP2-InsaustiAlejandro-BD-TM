using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //SQL
    [SerializeField] GameObject usernameError;
    [SerializeField] GameObject passwordError;
    [SerializeField] GameObject registerPanel;
    [SerializeField] GameObject passwordPanel;
    [SerializeField] RectTransform signPanel;
    [SerializeField] SQLConnection sqlConnector;
    [SerializeField] float hidePanelTime;
    Vector3 originalSignPanelPos;
    float hidePanelTimer;

}