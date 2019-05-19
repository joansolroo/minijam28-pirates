using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour {

    [SerializeField] Color color;
    [SerializeField]public ParticleSystem emission;
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
    public void ResetHighlight()
    {
        Color subColor = this.color;
        subColor.a = sprites[0].color.a;
        sprites[0].color = subColor;
    }
    public void SetHighlight(Color color)
    {
        Color subColor = color;
        subColor.a = sprites[0].color.a;
        sprites[0].color = subColor;
    }
}
