using UnityEngine;

public class SavePointController : MonoBehaviour
{
    [SerializeField] SQLConnection sqlConnector;
    [SerializeField] GameplayManager gameplayManager;

    private void Start()
    {
        gameplayManager.gameStarted += OnGameStart;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerData playerData = collision.gameObject.GetComponent<PlayerController>()?.data;
            sqlConnector.UpdatePlayerData(playerData);
        }
    }

    void OnGameStart()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
