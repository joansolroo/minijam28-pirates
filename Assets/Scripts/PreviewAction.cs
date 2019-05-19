using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewAction : MonoBehaviour
{
    [SerializeField] CardRegion selected;
    [SerializeField] LineRenderer[] curves;

    [SerializeField] Material fullLine;
    [SerializeField] Material dottedLine;

    [SerializeField] TextMesh description;
    int curveCount = 0;

    int thisPos;
    int enemyPos;
    public void DoPreviewAction()
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
        Ship enemyShip = player.enemy.ship;

        Map map = Map.current;
        description.text = "Card description:\n";
        
        foreach (Card card in player.selected.cards)
        {
            description.text += card.rule.description;
            //if (!card.isAnimating)
            {
                if (card.visible)
                {
                    if (card.rule.damageAmount > 0)
                    {
                        Gizmos.color = Color.red;
                        Vector3 size = new Vector3(0.7f, 0.7f, 0.7f);
                        int targetPosition = Mathf.Min(enemyShip.position, ship.position + card.rule.attackMaxRange * ship.direction);
                        if (card.rule.attackTarget == CardRule.AttackTarget.All)
                        {
                           /*description.text += "Damage: " + card.rule.damageAmount + "\n"
                                + "  Range: " + card.rule.attackMaxRange + "\n";*/
                            for (int i = 0; i < 3; ++i)
                            {
                                int idx = targetPosition + enemyShip.direction * i;
                                if (idx > ship.position)
                                {
                                    Vector3 cell = map.GetPosition(idx);

                                    //Gizmos.DrawCube(cell, size);
                                    //Gizmos.DrawLine(this.transform.position, cell);
                                    Vector3 fromPos = map.GetPosition(ship.position);
                                    Vector3 toPos = map.GetPosition(targetPosition + enemyShip.direction * i);
                                    int distance = Mathf.Abs(ship.position - (targetPosition + enemyShip.direction * i));
                                    if (distance <= card.rule.attackMaxRange)
                                    {
                                        AddAffected(this.transform.position, toPos, Color.red);
                                        AddCurve(fromPos, toPos, Color.red);
                                        Map.current.tiles[idx].SetHighlight(Color.red);
                                    }
                                    else
                                    {
                                        AddCurve(fromPos, toPos, Color.gray);
                                        AddAffected(this.transform.position, toPos, Color.gray);
                                    }
                                }
                            }
                        }
                        else if (card.rule.attackTarget == CardRule.AttackTarget.Moving)
                        {
                            /*description.text += "If enemy moves:\n" + ">  Damage: " + card.rule.damageAmount + "\n"
                                + ">  Range: " + card.rule.attackMaxRange + "\n";*/
                            for (int i = 1; i < 3; ++i)
                            {
                                int idx = targetPosition + enemyShip.direction * i;
                                if (idx > ship.position)
                                {
                                    Vector3 cell = map.GetPosition(idx);
                                    // Gizmos.DrawCube(cell, size);
                                    // Gizmos.DrawLine(this.transform.position, cell);
                                    Vector3 fromPos = map.GetPosition(ship.position);
                                    Vector3 toPos = map.GetPosition(targetPosition + enemyShip.direction * i);

                                    int distance = Mathf.Abs(ship.position - (targetPosition + enemyShip.direction * i));
                                    if (distance <= card.rule.attackMaxRange)
                                    {
                                        AddAffected(this.transform.position, toPos, Color.red);
                                        Map.current.tiles[idx].SetHighlight(Color.red);
                                        AddCurve(fromPos, toPos, Color.red);
                                    }
                                    else
                                    {
                                        AddAffected(this.transform.position, toPos, Color.gray);
                                        AddCurve(fromPos, toPos, Color.gray);
                                    }
                                }
                            }
                        }
                        else if (card.rule.attackTarget == CardRule.AttackTarget.notMoving)
                        {
                            /*description.text += "If enemy NOT moves:\n"
                                + ">  Damage: " + card.rule.damageAmount + "\n"
                                 + ">  Range: " + card.rule.attackMaxRange + "\n";*/
                            int idx = targetPosition;
                            if (idx > ship.position)
                            {
                                Vector3 cell = map.GetPosition(idx);
                                //Gizmos.DrawCube(cell, size);
                                //Gizmos.DrawLine(this.transform.position, cell);

                                Vector3 fromPos = map.GetPosition(ship.position);
                                Vector3 toPos = map.GetPosition(targetPosition);

                                int distance = Mathf.Abs(ship.position - targetPosition);
                                if (distance <= card.rule.attackMaxRange)
                                {
                                    AddAffected(this.transform.position, toPos, Color.red);
                                    Map.current.tiles[idx].SetHighlight(Color.red);
                                    AddCurve(fromPos, toPos, Color.red);
                                }
                                else
                                {
                                    AddAffected(this.transform.position, toPos, Color.gray);
                                    AddCurve(fromPos, toPos, Color.gray);
                                }
                            }
                        }


                    }
                    if (card.rule.movementAmount > 0)
                    {
                        /*description.text += "Movement: " + card.rule.movementAmount + "\n";
                        if(card.rule.boarding)
                        {
                            description.text += ">  Special: BOARDING\n";
                        }*/
                        for (int i = 0; i <= card.rule.movementAmount; ++i)
                        {
                            int idx = ship.position + ship.direction * i;
                            Vector3 cell = map.GetPosition(idx);
                            Map.current.tiles[idx].SetHighlight(Color.cyan);

                            Vector3 fromPos = map.GetPosition(ship.position);
                            Vector3 toPos = map.GetPosition(ship.position + ship.direction * card.rule.movementAmount);
                            AddCurve(fromPos, toPos, Color.cyan);
                            AddAffected(this.transform.position, toPos, Color.cyan);
                        }

                        /*
                        Vector3 size = new Vector3(0.9f, 0.01f, 0.9f);
                        Gizmos.color = Color.blue;
                        for (int i = 0; i <= card.rule.movementAmount; ++i)
                        {
                            Vector3 cell = map.GetPosition(ship.position + ship.direction * i);
                            Gizmos.DrawCube(cell, size);
                            Gizmos.DrawLine(this.transform.position, cell);
                        }*/

                    }

                    if (card.rule.healAmount > 0)
                    {
                        //description.text += "Heal: " + card.rule.healAmount + "\n";
                        int idx = ship.position;
                        Vector3 cell = map.GetPosition(idx);
                        Map.current.tiles[idx].SetHighlight(Color.green);
                        AddAffected(this.transform.position, cell, Color.green);
                        /*
                        Gizmos.color = Color.green;
                        {
                            Vector3 size = new Vector3(0.8f, 0.05f, 0.8f);
                            Vector3 cell = map.GetPosition(ship.position);
                            Gizmos.DrawCube(cell, size);
                            Gizmos.DrawLine(this.transform.position, cell);

                        }
                        */
                    }

                }
            }
        }

    }

    void AddCurve(Vector3 fromPos, Vector3 toPos, Color color, int pointCount = 20)
    {
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
