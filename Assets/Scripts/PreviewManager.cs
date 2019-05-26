using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewManager : MonoBehaviour
{
    [SerializeField] CardRegion selected;
    [SerializeField] LineRenderer[] curves;
    [SerializeField] ActionPreview[] previews;

    [SerializeField] Material fullLine;
    [SerializeField] Material dottedLine;

    //[SerializeField] TextMesh description;
    int curveCount = 0;

    int thisPos;
    int enemyPos;
    public void DoPreviewAction(int step = -1, int pos1 = -1, int pos2 = -1)
    {
        curveCount = 0;
        foreach (LineRenderer curve in curves)
        {
            curve.gameObject.SetActive(false);
        }
        Player player = selected.player;
        Ship ship = player.ship;
        if (pos1 == -1) pos1 = ship.position;
        Ship enemyShip = player.enemy.ship;
        if (pos2 == -1) pos2 = enemyShip.position;

        Map map = Map.current;
        bool visible = false;

        foreach (Card card in player.selected.cards)
        {
            {
                if (card.visible)
                {
                    visible = true;

                    if (step == -1)
                    {
                        for (int a = 0; a < card.rule.rules.Length; ++a)
                        {
                            CardAction action = card.rule.rules[a];
                            if (action != null)
                            {
                                AddPreview(card, action);
                            }
                        }
                    }
                    else
                    {
                        CardAction action = card.rule.rules[step];
                        if (action != null)
                        {
                            AddPreview(card, action);
                        }
                    }
                }
            }
        }
    }

    ActionPreview AddPreview(Card card, CardAction rule)
    {
        ActionPreview preview = previews[curveCount];
        LineRenderer curve = curves[curveCount];
        rule.Preview(card, preview);
        ++curveCount;
        return preview;
    }
    void AddAffected(Vector3 fromPos, Vector3 toPos, Color color)
    {
        /*
        LineRenderer curve = curves[curveCount];
        int pointCount = 4;
        fromPos.y = 0;
        toPos.y = 0;
        curve.gameObject.SetActive(true);
        curve.positionCount = 4;
        curve.SetPosition(0, fromPos);
        curve.SetPosition(1, new Vector3(fromPos.x, 0, (fromPos.z * 0.5f + toPos.z * 0.5f)));
        curve.SetPosition(2, new Vector3(toPos.x, 0, (fromPos.z * 0.5f + toPos.z * 0.5f)));
        curve.SetPosition(3, toPos);

        curve.startColor = color;
        curve.endColor = color;

        curve.material = fullLine;

        ++curveCount;
        */
    }
}
