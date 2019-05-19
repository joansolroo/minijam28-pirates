using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour {

    [SerializeField] Color color;

    [SerializeField] SpriteRenderer[] sprites;
	
    // Use this for initialization
	void Start () {
        SetColor(color);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnValidate()
    {
        SetColor(color);
    }
    public void SetColor(Color color)
    {
        this.color = color;
        foreach(SpriteRenderer renderer in sprites)
        {
            Color subColor = color;
            subColor.a = renderer.color.a;
            renderer.color = subColor;
        }
    }
}
