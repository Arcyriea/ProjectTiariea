using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public Tilemap tilemap;
    public TileBase[] tileTypes;

    void Start()
    {
        GenerateTilemap();
    }

    public void GenerateTilemap()
    {
        // Generate the initial random tilemap.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int tileTypeIndex = UnityEngine.Random.Range(0, tileTypes.Length);
                tilemap.SetTile(new Vector3Int(x, y, 0), tileTypes[tileTypeIndex]);
            }
        }

        // Loop the tilemap horizontally.
        for (int y = 0; y < height; y++)
        {
            TileBase firstTile = tilemap.GetTile(new Vector3Int(0, y, 0));
            tilemap.SetTile(new Vector3Int(width, y, 0), firstTile);
        }

        // Loop the tilemap vertically.
        for (int x = 0; x < width + 1; x++)
        {
            TileBase bottomTile = tilemap.GetTile(new Vector3Int(x, 0, 0));
            tilemap.SetTile(new Vector3Int(x, height, 0), bottomTile);
        }
    }
}
