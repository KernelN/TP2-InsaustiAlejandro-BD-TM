using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    public Vector2 mapSize { get { return mapSizeSetter; } }
    [SerializeField] GameObject safeTilePrefab;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform tilesEmpty;
    [SerializeField] GameplayManager gameManager;
    [SerializeField] Vector2 mapSizeSetter;
    [SerializeField] int tileDisappearCooldown;
    List<TileController> tiles;
    Action UpdateTiles;
    int tileQuantity { get { return (int)(mapSize.x * mapSize.y); } }

    private void Start()
    {
        tiles = new List<TileController>();
        for (float i = 0.5f; i < mapSize.x; i++)
        {
            for (float j = 0.5f; j < mapSize.y; j++)
            {
                GenerateTile(i, j);
            }
        }
        gameManager.oneSecondPassed += OnSecondPassed;
    }

    void GenerateTile(float i, float j)
    {
        //Generate
        GameObject go;
        if (i == mapSize.x / 2 && j == mapSize.y / 2)
        {
            go = Instantiate(safeTilePrefab, tilesEmpty);
        }
        else
        {
            go = Instantiate(tilePrefab, tilesEmpty);
        }
        Transform tileTransform = go.transform;

        //Set Transform
        float xPosition = (i - mapSize.x / 2) * tileTransform.localScale.x * 1.01f;
        float yPosition = (j - mapSize.y / 2) * tileTransform.localScale.y * 1.01f;
        tileTransform.localPosition = new Vector2(xPosition, yPosition);

        //Set TileController
        if (i == mapSize.x / 2 && j == mapSize.y / 2) { return; }
        TileController tile = go.GetComponent<TileController>();
        UpdateTiles += tile.OnStateUpdate;
        tiles.Add(tile);
    }
    void OnSecondPassed()
    {
        UpdateTiles.Invoke();
        if (gameManager.currentTime % tileDisappearCooldown == 0)
        {
            SelectTilesToDisappear();
        }
    }
    void SelectTilesToDisappear()
    {
        float tilesPercentage = (float)gameManager.currentTime / (float)gameManager.maxTime;
        for (int i = 0; i <= (int)(tilesPercentage * tileQuantity) + 1; i++) //round down and add 1
        {
            int randomTileIndex;
            do
            {
                randomTileIndex = Random.Range(0, tileQuantity - 1); //tile quantity - safeTile
            } while (tiles[randomTileIndex].currentState != TileController.State.SOLID);
            tiles[randomTileIndex].StartDisappear();
        }
    }
    public Vector2 GetMapBorders()
    {
        Vector2 borders = mapSize;
        Transform lastTile = tiles[tiles.Count - 1].transform;
        borders.x = lastTile.position.x + lastTile.localScale.x / 2;
        borders.y = lastTile.position.y + lastTile.localScale.y / 2;
        return borders;
    }
    public Vector2 GetRandomPosInsideMap()
    {
        Vector2 border = GetMapBorders();
        return new Vector2(Random.Range(-border.x, border.x), Random.Range(-border.y, border.y));
    }
}