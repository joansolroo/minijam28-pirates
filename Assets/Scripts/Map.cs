using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField] MapTile[] tiles;
    [SerializeField] List<Ship> ships;

    static Map instance;
    private void Awake()
    {
        instance = this;
    }

    public void Move(Ship ship, int amount)
    {
        ship.position += amount*ship.direction;
        ship.transform.position = tiles[ship.position].transform.position + new Vector3(0,0.1f,0);
    }
}
