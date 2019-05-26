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

    [SerializeField] TextMesh description;
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
        foreach (MapTile tile in Map.current.tiles)
        {
            tile.ResetHighlight();
        }
        Player player = selected.player;
        Ship ship = player.ship;
        if (pos1 == -1) pos1 = ship.position;
        Ship enemyShip = player.enemy.ship;
        if (pos2 == -1) pos2 = enemyShip.position;

        Map map = Map.current;
        bool visible = false;
        description.text = "Card description:\n";
        foreach (Card card in player.selected.cards)
        {
            description.text += card.rule.description;
            //if (!card.isAnimating)
            {
                if (card.visible)
                {
                    visible = true;
                    Vector3 fromPos = map.GetPosition(pos1);
                    {
                        Vector3 toPos = map.GetPosition(pos1 + enemyShip.direction * card.rule.attackMaxRange);
                        AddCurve(fromPos, toPos, Color.gray);
                    }
                    if (card.rule.damageAmount > 0)
                    {
                        int targetPosition;
                        if (ship.direction == 1)
                        {
                            targetPosition = Mathf.Min(pos2, pos1 + card.rule.attackMaxRange * ship.direction);
                        }
                        else
                        {
                            targetPosition = Mathf.Max(pos2, pos1 + card.rule.attackMaxRange * ship.direction);
                        }
                        if ((step == -1 || step == 0 || step == 2) && card.rule.attackTarget == CardData.AttackTarget.All)
                        {
                           /*description.text += "Damage: " + card.rule.damageAmount + "\n"
                                + "  Range: " + card.rule.attackMaxRange + "\n";*/
                            for (int i = 0; i < 3; ++i)
                            {
                                int idx = targetPosition + enemyShip.direction * i;
                                if (idx * ship.direction > pos1 * ship.direction)
                                {
                                    Vector3 cell = map.GetPosition(idx);

                                    //Gizmos.DrawCube(cell, size);
                                    //Gizmos.DrawLine(this.transform.position, cell);
                                   // Vector3 fromPos = map.GetPosition(pos1);
                                    Vector3 toPos = map.GetPosition(targetPosition + enemyShip.direction * i);
                                    int distance = Mathf.Abs(pos1 - (targetPosition + enemyShip.direction * i));
                                    if (distance <= card.rule.attackMaxRange)
                                    {
                                        AddAffected(this.transform.position, toPos, card.rule.color);
                                        AddCurve(fromPos, toPos, card.rule.color);
                                        Map.current.tiles[idx].SetHighlight(card.rule.color);
                                    }
                                    else
                                    {
                                        AddCurve(fromPos, toPos, Color.gray);
                                        AddAffected(this.transform.position, toPos, Color.gray);
                                    }
                                }
                            }
                        }
                        else if ((step == -1 || step == 2) && card.rule.attackTarget == CardData.AttackTarget.Moving)
                        {
                            /*description.text += "If enemy moves:\n" + ">  Damage: " + card.rule.damageAmount + "\n"
                                + ">  Range: " + card.rule.attackMaxRange + "\n";*/
                            for (int i = 1; i < 3; ++i)
                            {
                                int idx = targetPosition + enemyShip.direction * i;
                                if (idx*ship.direction > pos1*ship.direction)
                                {
                                    Vector3 cell = map.GetPosition(idx);
                                    // Gizmos.DrawCube(cell, size);
                                    // Gizmos.DrawLine(this.transform.position, cell);
                                    //Vector3 fromPos = map.GetPosition(pos1);
                                    Vector3 toPos = map.GetPosition(targetPosition + enemyShip.direction * i);

                                    int distance = Mathf.Abs(pos1 - (targetPosition + enemyShip.direction * i));
                                    if (distance <= card.rule.attackMaxRange)
                                    {
                                        AddAffected(this.transform.position, toPos, card.rule.color);
                                        Map.current.tiles[idx].SetHighlight(card.rule.color);
                                        AddCurve(fromPos, toPos, card.rule.color);
                                    }
                                    else
                                    {
                                        AddAffected(this.transform.position, toPos, Color.gray);
                                        AddCurve(fromPos, toPos, Color.gray);
                                    }
                                }
                            }
                        }
                        else if ((step == -1 || step == 0) && card.rule.attackTarget == CardData.AttackTarget.notMoving)
                        {
                            /*description.text += "If enemy NOT moves:\n"
                                + ">  Damage: " + card.rule.damageAmount + "\n"
                                 + ">  Range: " + card.rule.attackMaxRange + "\n";*/
                            int idx = targetPosition;
                            if (idx * ship.direction > pos1 * ship.direction)
                            {
                                Vector3 cell = map.GetPosition(idx);
                                //Gizmos.DrawCube(cell, size);
                                //Gizmos.DrawLine(this.transform.position, cell);

                               // Vector3 fromPos = map.GetPosition(pos1);
                                Vector3 toPos = map.GetPosition(targetPosition);

                                int distance = Mathf.Abs(pos1 - targetPosition);
                                if (distance <= card.rule.attackMaxRange)
                                {
                                    AddAffected(this.transform.position, toPos, card.rule.color);
                                    Map.current.tiles[idx].SetHighlight(card.rule.color);
                                    AddCurve(fromPos, toPos, card.rule.color);
                                }
                                else
                                {
                                    AddAffected(this.transform.position, toPos, Color.gray);
                                    AddCurve(fromPos, toPos, Color.gray);
                                }
                            }
                        }


                    }
                    if ((step == -1 || step == 1) && card.rule.movementAmount > 0)
                    {
                        /*description.text += "Movement: " + card.rule.movementAmount + "\n";
                        if(card.rule.boarding)
                        {
                            description.text += ">  Special: BOARDING\n";
                        }*/
                        for (int i = 0; i <= card.rule.movementAmount; ++i)
                        {
                            int idx = pos1 + ship.direction * i;
                            Vector3 cell = map.GetPosition(idx);
                            Map.current.tiles[idx].SetHighlight(card.rule.color);

                            //Vector3 fromPos = map.GetPosition(pos1);
                            Vector3 toPos = map.GetPosition(pos1 + ship.direction * card.rule.movementAmount);
                            AddCurve(fromPos, toPos, card.rule.color);
                            AddAffected(this.transform.position, toPos, card.rule.color);
                        }

                        /*
                        Vector3 size = new Vector3(0.9f, 0.01f, 0.9f);
                        Gizmos.color = Color.blue;
                        for (int i = 0; i <= card.rule.movementAmount; ++i)
                        {
                            Vector3 cell = map.GetPosition(pos1 + ship.direction * i);
                            Gizmos.DrawCube(cell, size);
                            Gizmos.DrawLine(this.transform.position, cell);
                        }*/

                    }

                    if ((step == -1 || step == 3) && card.rule.healAmount > 0)
                    {
                        //description.text += "Heal: " + card.rule.healAmount + "\n";
                        int idx = pos1;
                        Vector3 cell = map.GetPosition(idx);
                        Map.current.tiles[idx].SetHighlight(card.rule.color);
                        AddAffected(this.transform.position, cell, card.rule.color);
                        /*
                        Gizmos.color = Color.green;
                        {
                            Vector3 size = new Vector3(0.8f, 0.05f, 0.8f);
                            Vector3 cell = map.GetPosition(pos1);
                            Gizmos.DrawCube(cell, size);
                            Gizmos.DrawLine(this.transform.position, cell);

                        }
                        */
                    }

                }
            }
        }

        if (!visible)
        {
            description.text = "";
        }

    }

    ActionPreview AddCurve(Vector3 fromPos, Vector3 toPos, Color color, int pointCount = 20)
    {
        ActionPreview preview = previews[curveCount];
        LineRenderer curve = curves[curveCount];
        float height = Vector3.Distance(fromPos, toPos) / 6;
        curve.gameObject.SetActive(true);
        curve.positionCount = pointCount;
        curve.SetPosition(0, fromPos);
        for (int p = 0; p < pointCount; ++p)
        {
            float t = ((float)p) / (pointCount - 1);
            Vector3 point = Vector3.Lerp(fromPos, toPos, t);
            curve.SetPosition(p, point + Vector3.up * height * Mathf.Pow(1 - Mathf.Abs(0.5f - Mathf.Abs(t)) * 2, 0.25f));
        }
        curve.startColor = color;
        curve.endColor = color;

        curve.material = dottedLine;

        ++curveCount;
        return preview;
    }
    void AddAffected(Vector3 fromPos, Vector3 toPos, Color color)
    {
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
    }
}
