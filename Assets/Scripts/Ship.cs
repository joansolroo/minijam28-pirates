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

    Vector3 initPos;
    Vector3 initRot;
    private void Start()
    {
        foreach(SpriteRenderer renderer in shipRenderer)
        {
            renderer.color = owner.color;
            if (direction == -1) renderer.flipX=true;
        }
        shipBase.color = owner.color;
        initPos = shipRenderer[0].transform.localPosition;
        initRot = shipRenderer[0].transform.localEulerAngles;
    }
    private void Update()
    {
        shipRenderer[0].transform.localPosition = initPos + Vector3.up * Mathf.Sin(Time.time * 3)*0.1f;
        shipRenderer[0].transform.localEulerAngles = initRot + new Vector3(Mathf.Sin(Time.time * 3) * 0.5f,0, Mathf.Sin(Time.time * 5) * 3f);
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
