using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    [SerializeField] Player owner;
    [SerializeField] public int position;
    [SerializeField] public int direction;
    [SerializeField] public bool moved;

    [SerializeField] SpriteRenderer[] shipRenderer;
    [SerializeField] SpriteRenderer shipBase;
    private void Start()
    {
        foreach(SpriteRenderer renderer in shipRenderer)
        {
            renderer.color = owner.color;
            if (direction == -1) renderer.flipX=true;
        }
        shipBase.color = owner.color;
    }
}
