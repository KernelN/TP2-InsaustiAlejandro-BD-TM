using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform coinEmpty;
    [SerializeField] GameplayManager gameManager;
    [SerializeField] TileManager tileManager;
    [SerializeField] PlayerController player;
    [SerializeField] int maxCoins;
    [SerializeField] float coinSpawnChance;
    List<CoinController> activeCoins;
    List<CoinController> inactiveCoins;

    private void Start()
    {
        activeCoins = new List<CoinController>();
        inactiveCoins = new List<CoinController>();
        for (int i = 0; i < maxCoins; i++)
        {
            GenerateCoin();
        }
        gameManager.oneSecondPassed += OnSecondPassed;
    }

    void GenerateCoin()
    {
        GameObject coinGO = Instantiate(coinPrefab, coinEmpty);
        coinGO.SetActive(false);
        CoinController coin = coinGO.GetComponent<CoinController>();
        inactiveCoins.Add(coin);
        coin.coinPickedUp += OnCoinPickUp;
    }
    void OnSecondPassed()
    {
        ChangeCoinStates();
        if (inactiveCoins.Count > 0 && Random.Range(0, 100) < coinSpawnChance)
        {
            ActivateCoin();
        }
    }
    void ActivateCoin()
    {
        //Get Coin from Inactive List
        CoinController coin = inactiveCoins[0];
        inactiveCoins.Remove(coin);

        //Set position and reactivate
        coin.transform.position = GetFreePosition();
        coin.gameObject.SetActive(true);
        activeCoins.Add(coin);
    }
    Vector3 GetFreePosition()
    {
        Vector2 mapBorder = tileManager.GetMapBorders();
        Vector2 freePos = new Vector2(Random.Range(-mapBorder.x, mapBorder.x), Random.Range(-mapBorder.y, mapBorder.y));
        //while (Physics2D.CircleCast(freePos, coinPrefab.transform.localScale.x * 2, Vector2.up, 0, LayerMask.GetMask("Map")))
        //{
        //    if (freePos.x < mapBorder.x)
        //    {
        //        freePos.x++;
        //    }
        //    else if (freePos.y < mapBorder.y)
        //    {
        //        freePos.y++;
        //    }
        //    else
        //    {
        //        freePos.x = -mapBorder.x;
        //    }
        //} 
        return freePos;
    }
    void ChangeCoinStates()
    {
        for (int i = 0; i < activeCoins.Count; i++)
        {
            activeCoins[i].ChangeType();
        }
    }
    void OnCoinPickUp(CoinController coin)
    {
        activeCoins.Remove(coin);
        inactiveCoins.Add(coin);
        switch (coin.type)
        {
            case CoinController.Type.YELLOW:
                player.IncreaseScore(10);
                break;
            case CoinController.Type.RED:
                player.Die(false);
                break;
            default:
                break;
        }
    }
}
