using System.Collections.Generic;
using UnityEngine;

public class TileBag2: MonoBehaviour {
    
    public GameObject tilePrefab; // The prefab of a tile.
    public GameObject handTileHolder; // The holder for hand tiles.
    
    private List<TileScript2> tileBag = new List<TileScript2>();
    [SerializeField]
    private int startHandSize = 6;
    public List<TileScript2> handTiles = new List<TileScript2>();
    
    public int GetCurrentTileCount() {
        return handTiles.Count;
    }

    private void Start() {
        InitializeTileBag();
        for (int amount = 1; amount <= startHandSize; amount++) {
            AddTileToHand(GetRandomLetterFromBag());
        }
    }

    private void InitializeTileBag() {
        tileBag.Clear();
        AddTilesToBag("A", 9, 1);
        AddTilesToBag("B", 2, 3);
        AddTilesToBag("C", 2, 3);
        AddTilesToBag("D", 4, 2);
        AddTilesToBag("E", 12, 1);
        AddTilesToBag("F", 2, 4);
        AddTilesToBag("G", 3, 2);
        AddTilesToBag("H", 2, 4);
        AddTilesToBag("I", 9, 1);
        AddTilesToBag("J", 1, 8);
        AddTilesToBag("K", 1, 5);
        AddTilesToBag("L", 4, 1);
        AddTilesToBag("M", 2, 3);
        AddTilesToBag("N", 6, 1);
        AddTilesToBag("O", 8, 1);
        AddTilesToBag("P", 2, 3);
        AddTilesToBag("Q", 1, 10);
        AddTilesToBag("R", 6, 1);
        AddTilesToBag("S", 4, 1);
        AddTilesToBag("T", 6, 1);
        AddTilesToBag("U", 4, 1);
        AddTilesToBag("V", 2, 4);
        AddTilesToBag("W", 2, 4);
        AddTilesToBag("X", 1, 8);
        AddTilesToBag("Y", 2, 4);
        AddTilesToBag("Z", 1, 10);
    }

    private void AddTilesToBag(string letter, int amount, int points) {
        for (int i = 0; i < amount; i++) {
            GameObject tempTile = Instantiate(tilePrefab);
            TileScript2 tempScript = tempTile.GetComponent<TileScript2>();
            tempScript.InitTile(letter, points);
            tempTile.SetActive(false);
            tileBag.Add(tempScript);
        }
    }

    public TileScript2 GetRandomLetterFromBag() {
        int index = Random.Range(0, tileBag.Count);
        TileScript2 randomTile = tileBag[index];
        tileBag.Remove(randomTile);
        return randomTile;
    }

    public void AddTileToHand(TileScript2 tile) {
        tile.gameObject.SetActive(true);
        tile.transform.SetParent(handTileHolder.transform);
        handTiles.Add(tile);
    }

    public void RefillHandTiles(int currentTileCount) {
        // Calculate the number of tiles needed to refill
        int tilesToDraw = 7 - currentTileCount; 
        // Draw the calculated number of random tiles from the bag
        for (int amount = 1; amount < tilesToDraw; amount++) {
            AddTileToHand(GetRandomLetterFromBag());
        }
    }
}