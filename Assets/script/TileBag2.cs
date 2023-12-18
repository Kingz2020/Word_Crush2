using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
//using System.Random;

public class TileBag2 : MonoBehaviour
{
    public GameObject tilePrefab; // The prefab of a tile.
    public GameObject handTileHolder; // The holder for hand tiles.

    

    private Dictionary<char, List<Tile>> tileBag; // A dictionary to store tiles by letter.

    // Custom Tile class to store letter, points, and amount.

    public List<GameObject> handTiles = new List<GameObject>();

    char[] Eng_Array = new char[129];

    // Copy the data from the nested array into the combined array
    int index = 0;


    // New method to get the current tile count in the hand
public int GetCurrentTileCount()
    {
        return handTiles.Count;
    }

   

    public class Tile
    {
        public char Letter { get; }
        public int Points { get; }
        public int Amount { get; set; }

        public Tile(char letter, int points)
        {
            Letter = letter;
            Points = points;
            Amount = 0;
        }
    }

    private void Start()
    {
        InitializeTileBag(); // Initialize the tile bag with letters and points.
        DrawRandomTiles(6); // Draw 6 random tiles from the bag.

        //handTiles = new List<GameObject>();

        GameObject newTile = Instantiate(tilePrefab, handTileHolder.transform);
    }

    private static Dictionary<char, int> letterPointValue = new Dictionary<char, int>();

    private void InitializeTileBag()
    {
        tileBag = new Dictionary<char, List<Tile>>();

        // Add tiles according to the distribution.
        AddTilesToBag('A', 9, 1);
        AddTilesToBag('B', 2, 3);
        AddTilesToBag('C', 2, 3);
        AddTilesToBag('D', 4, 2);
        AddTilesToBag('E', 12, 1);
        AddTilesToBag('F', 2, 4);
        AddTilesToBag('G', 3, 2);
        AddTilesToBag('H', 2, 4);
        AddTilesToBag('I', 9, 1);
        AddTilesToBag('J', 1, 8);
        AddTilesToBag('K', 1, 5);
        AddTilesToBag('L', 4, 1);
        AddTilesToBag('M', 2, 3);
        AddTilesToBag('N', 6, 1);
        AddTilesToBag('O', 8, 1);
        AddTilesToBag('P', 2, 3);
        AddTilesToBag('Q', 1, 10);
        AddTilesToBag('R', 6, 1);
        AddTilesToBag('S', 4, 1);
        AddTilesToBag('T', 6, 1);
        AddTilesToBag('U', 4, 1);
        AddTilesToBag('V', 2, 4);
        AddTilesToBag('W', 2, 4);
        AddTilesToBag('X', 1, 8);
        AddTilesToBag('Y', 2, 4);
        AddTilesToBag('Z', 1, 10);


        foreach (var letterValue in tileBag)
        {
            letterPointValue[letterValue.Key] = letterValue.Value[0].Points;
        }
    }
    public static int GetLetterValue(char c)
    {
        return letterPointValue[c];
    }

    private void AddTilesToBag(char letter, int amount, int points)
    {
        tileBag[letter] = new List<Tile>();
        for (int i = 0; i < amount; i++)
        {
            Tile tile = new Tile(letter, points);
            tileBag[letter].Add(tile);
        }
    }

    private void DrawRandomTiles(int numTiles)
    {
        // Create a reusable pool of tiles
        List<GameObject> tilePool = new List<GameObject>();
        

        for (int i = 1; i < numTiles + 1; i++)
        {
            // Draw a random letter from the dictionary
            char randomLetter = GetRandomLetterFromBag();
            //char randomKey = tileBag[randomLetter].Keys[0];
            // Access the `TileScript2` component directly from the prefab
            GameObject newTile = Instantiate(tilePrefab);
            TileScript2 tileScript = newTile.GetComponent<TileScript2>();

            // Assign the tile's values to the `TileScript2` component
            tileScript.InitTile(randomLetter.ToString(), tileBag[randomLetter][0].Points);

            // Remove the drawn letter from the dictionary
            tileBag[randomLetter].RemoveAt(0);

            if (tileBag[randomLetter].Count == 0)
            {
                // Remove the empty letter from the dictionary
                tileBag.Remove(randomLetter);
            }

            // Add the new tile to the pool
            tilePool.Add(newTile);
        }
    }

    public char GetRandomLetterFromBag()
    {

        // Create a list of letters
        List<char> letters = new List<char>();

        // Iterate through the tileBag dictionary
        foreach (char key in tileBag.Keys)
        {
            for (int i = 0; i < tileBag[key].Count; i++)
            {
                letters.Add(key);
            }
        }

        // Randomly select a letter from the list
        int index = UnityEngine.Random.Range(0, letters.Count);

        // Get the selected letter and remove it from the bag
        char selectedLetter = letters[index];
        tileBag.Remove(selectedLetter);
        return selectedLetter;
    }


    public void AddTileToHand(GameObject tile)
    {
        tile.transform.SetParent(handTileHolder.transform);
        // Additional adjustments if needed (e.g., position, size).
    }

    public void RefillHandTiles(int currentTileCount)
    {
        int tilesToDraw = 7 - currentTileCount; // Calculate the number of tiles needed to refill

        if (tilesToDraw > 0)
        {
            DrawRandomTiles(tilesToDraw); // Draw the calculated number of random tiles from the bag
        }
    }
}