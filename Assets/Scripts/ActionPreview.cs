using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class ActionPreview : MonoBehaviour
{
    [SerializeField] LineRenderer curve;
    [SerializeField] Card card;
    [SerializeField] CardAction action;

    public float time = 0;
    [SerializeField] Transform actionIcon;
    [SerializeField] SpriteRenderer actionIconRenderer;
    private void OnValidate()
    {
        curve = GetComponent<LineRenderer>();
    }


    public void Set(Card card, CardAction rule, Vector3 fromPos, Vector3 toPos, Color color, int pointCount = 20)
    {
        this.SetAction(card, rule);
        this.BuildCurve(fromPos, toPos, color, pointCount);
    }
    public void SetAction(Card card, CardAction action)
    {
        this.card = card;
        this.action = action;
        time = 0;
    }

    public void BuildCurve(Vector3 fromPos, Vector3 toPos, Color color, int pointCount = 20)
    {
        float height = Vector3.Distance(fromPos, toPos) / 6;
        curve.gameObject.SetActive(true);
        curve.positionCount = pointCount;
        curve.SetPosition(0, fromPos);

        Vector3 delta = fromPos- toPos;
        for (int p = 0; p < pointCount; ++p)
        {
            float t = ((float)p) / (pointCount - 1);
            float a = t * 180 * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Cos(a)*delta.x/2, Mathf.Sin(a) * (1+Mathf.Abs(delta.x)*0.1f), 0) + (fromPos+toPos)/2;
            curve.SetPosition(p, point /*+ Vector3.up * height * Mathf.Pow(1 - Mathf.Abs(0.5f - Mathf.Abs(t)) * 2, 0.25f)*/);
        }

        curve.startColor = color;
        curve.endColor = color;
        actionIconRenderer.color = color;
       // curve.material = dottedLine;
    }

    private void Update()
    {
        time = Time.time*0.5f % 1;
        time = 0.7f+0.2f*time;
        actionIcon.position = Evaluate(time);
        actionIcon.LookAt(Evaluate(time + 0.01f));
    }

    public Vector3 Evaluate(float t)
    {
        int count = curve.positionCount;
        int idx0 = (int)(t * (count - 1));
        int idx1 = idx0+1;
        if(t <= 0)
        {
            return curve.GetPosition(0);
        }
        else if (idx1>= count-1)
        {
            return curve.GetPosition(count-1);
        }
        else
        {
            Vector3 pos0 = curve.GetPosition(idx0);
            Vector3 pos1 = curve.GetPosition(idx1);
            float dt = (t * (count - 1))-idx0;
            return pos0 + (pos1-pos0)*dt;
        }
    }
}
