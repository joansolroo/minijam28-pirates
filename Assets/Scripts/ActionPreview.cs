using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class ActionPreview : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] Card card;
    [SerializeField] CardAction action;

    public float time = 0;
    [SerializeField] Transform actionIcon;
    [SerializeField] SpriteRenderer actionIconRenderer;
    private void OnValidate()
    {
        line = GetComponent<LineRenderer>();
    }

    public void SetAction(Card card, CardAction action)
    {
        this.card = card;
        this.action = action;
    }

    private void Update()
    {
        time = Time.time*0.5f % 1;
        actionIcon.position = Evaluate(time);
        actionIcon.LookAt(Evaluate(time + 0.01f));
    }

    public Vector3 Evaluate(float t)
    {
        int count = line.positionCount;
        int idx0 = (int)(t * (count - 1));
        int idx1 = idx0+1;
        if(t <= 0)
        {
            return line.GetPosition(0);
        }
        else if (idx1>= count-1)
        {
            return line.GetPosition(count-1);
        }
        else
        {
            Vector3 pos0 = line.GetPosition(idx0);
            Vector3 pos1 = line.GetPosition(idx1);
            float dt = (t * (count - 1))-idx0;
            return pos0 + (pos1-pos0)*dt;
        }
    }
}
