using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField] public MapTile[] tiles;
    [SerializeField] List<Ship> ships;

    public static Map current;

    private void Awake()
    {
        current = this;
    }

    public int ClipPosition(int position)
    {
        return Mathf.Min(tiles.Length - 1, Mathf.Max(0, position));
    }
    public Vector3 GetPosition(int cell)
    {  
        return tiles[ClipPosition(cell)].transform.position;
    }

    public void ClearHighlight()
    {
        foreach (MapTile tile in this.tiles)
        {
            tile.ResetHighlight();
        }
    }
}
