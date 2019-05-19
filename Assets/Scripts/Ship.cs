using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    [SerializeField] Player owner;
    [SerializeField] public int position;
    [SerializeField] public int direction;
    [SerializeField] public bool moved;

    [SerializeField] public SpriteRenderer[] shipRenderer;
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

    public void MoveTo(int targetPosition, bool animate = true)
    {
        if(targetPosition!=position)
        {
            Vector3 to = Map.current.GetPosition(targetPosition);
            if (animate)
            {
                Vector3 from = Map.current.GetPosition(position);
                StartCoroutine(DoMove(from, to));
            }
            else
            {
                this.transform.position = to;
            }
            this.position = targetPosition;
        }
    }
    IEnumerator DoMove(Vector3 from, Vector3 to)
    {
        for(float t = 0; t <1; t+=Time.deltaTime*3)
        {
            this.transform.position = Vector3.Lerp(from, to, t);
            yield return new WaitForEndOfFrame();
        }
        this.transform.position = to;
    }
}
