using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField] MapTile[] tiles;
    [SerializeField] List<Ship> ships;

    public static Map current;

    private void Awake()
    {
        current = this;
    }

    public void Move(Ship ship, int amount)
    {
        int newPos = ship.position+amount * ship.direction;
        newPos = Mathf.Min(tiles.Length - 1, Mathf.Max(0, newPos));
        ship.position = newPos;
        ship.transform.position = GetPosition(ship.position) + new Vector3(0,0.1f,0);
    }

    public Vector3 GetPosition(int cell)
    {
        return tiles[cell].transform.position;
    }
}
