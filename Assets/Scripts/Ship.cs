using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    [SerializeField] Player owner;
    [SerializeField] public int position;
    [SerializeField] public int direction;
    [SerializeField] public bool moved;
}
