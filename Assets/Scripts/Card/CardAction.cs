using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cardRule", menuName = "Cards/Action", order = 1)]
public class CardAction : ScriptableObject
{

    public string name;
    public AudioClip clip;
    public Color color;

    // TODO: change this for polymorphism
    public enum RuleType
    {
        move = 2, attack = 4, scan = 8, heal = 16, boarding = 32, None = 0, Any = 64
    }
    public RuleType ruleType;
    public enum AttackTarget
    {
        All, notMoving, Moving
    }
    public AttackTarget target;

    [SerializeField] public int amount = 1;
    [SerializeField] public int range = 1;
    [SerializeField] float resolutionDuration = 1;

    public bool Resolve(Card container)
    {
        Player owner = container.owner;
        Player enemy = owner.enemy;
        Ship ownerShip = owner.ship;
        Ship enemyShip = enemy.ship;
        Map map = Map.current;
        if (ruleType == RuleType.attack)
        {

            if (amount > 0)
            {
                bool hit = false;

                int pos1 = ownerShip.position;
                int pos1prev = ownerShip.previousPosition;
                int pos2 = enemyShip.position;
                int pos2prev = enemyShip.previousPosition;

                int aimedPos = -1;
                if (target == AttackTarget.All)
                {
                    aimedPos = pos2;
                }
                else if (target == AttackTarget.Moving)
                {
                    if (enemyShip.moved)
                    {
                        aimedPos = pos2;
                    }
                    else
                    {
                        aimedPos = pos2 + enemyShip.direction;
                    }
                }
                else if (target == AttackTarget.notMoving)
                {
                    aimedPos = pos2prev;
                }

                if (range >= Mathf.Abs(pos1prev - pos2))
                {
                    hit = aimedPos != -1 && pos2 == aimedPos;
                }
                else
                {
                    if (ownerShip.direction == 1)
                    {
                        aimedPos = Mathf.Min(pos2, pos1prev + this.range * ownerShip.direction);
                    }
                    else
                    {
                        Debug.Log("Attack: Out of range");
                        aimedPos = Mathf.Max(pos2, pos1prev + this.range * ownerShip.direction);
                    }
                }
                if (!hit)
                {
                    Debug.Log("Attack: attack missed ("+pos2+")!=("+aimedPos+")");
                    ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                    parameter.startColor = Color.gray;
                    map.tiles[aimedPos].emission.Emit(parameter, Random.Range(5, 15));
                    ownerShip.audioSource.PlayOneShot(clip);
                }
                else
                {
                    Debug.Log("Attack: attack hit");

                    owner.enemy.Hurt(amount);

                    ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                    parameter.startColor = owner.enemy.color;
                    map.tiles[aimedPos].emission.Emit(parameter, Random.Range(30, 50));
                    ownerShip.audioSource.PlayOneShot(clip);
                    //enemyShip.audioSource.clip = this.rule.explosionClip;
                    //enemyShip.audioSource.PlayDelayed(0.25f);
                }
                return hit;
            }
        }
        else if (ruleType == RuleType.move)
        {
            if (this.amount > 0)
            {
                ownerShip.MoveTo(ownerShip.position + ownerShip.direction * this.amount);
                ownerShip.audioSource.PlayOneShot(this.clip);
                return true;
            }
        }
        else if (ruleType == RuleType.scan)
        {

            foreach (Card c in owner.enemy.hand.cards)
            {

                c.SetVisible(true);

                ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                parameter.startColor = container.rule.color;
                container.map.tiles[enemyShip.position].emission.Emit(parameter, Random.Range(5, 15));
                ownerShip.audioSource.PlayOneShot(this.clip);
            }
            return true;
        }
        else if (ruleType == RuleType.heal)
        {
            if (this.amount > 0)
            {
                owner.Heal(this.amount, true);

                ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                parameter.startColor = container.rule.color;
                container.map.tiles[ownerShip.position].emission.Emit(parameter, Random.Range(5, 15));
                ownerShip.audioSource.PlayOneShot(this.clip);
            }
        }
        else if (ruleType == RuleType.boarding)
        {
            if (Mathf.Abs(ownerShip.position - enemyShip.position) <= this.range)
            {
                Fight.current.OnBoard(owner, owner.enemy);

                ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                parameter.startColor = owner.color;
                Map.current.tiles[enemyShip.position].emission.Emit(parameter, Random.Range(5, 15));
                ownerShip.audioSource.PlayOneShot(clip);
            }
        }
        return false;
    }

    public void Preview(Card card, ActionPreview preview)
    {
        Player owner = card.owner;
        Player enemy = owner.enemy;
        Ship ownerShip = owner.ship;
        Ship enemyShip = enemy.ship;
        Map map = Map.current;

        int pos1 = ownerShip.position;
        int pos1prev = ownerShip.previousPosition;
        int pos2 = enemyShip.position;
        int pos2prev = enemyShip.previousPosition;

        Vector3 fromPos = map.GetPosition(pos1);

        if (this.ruleType == CardAction.RuleType.attack && this.amount > 0)
        {
            int targetPosition;
            if (ownerShip.direction == 1)
            {
                targetPosition = Mathf.Min(pos2prev, pos1prev + this.range * ownerShip.direction);
            }
            else
            {
                targetPosition = Mathf.Max(pos2prev, pos1prev + this.range * ownerShip.direction);
            }

            if (this.target == CardAction.AttackTarget.All)
            {
                for (int i = 0; i <= 1; ++i)
                {
                    int idx = targetPosition + enemyShip.direction * i;
                    if (idx * ownerShip.direction > pos1 * ownerShip.direction)
                    {
                        Vector3 cell = map.GetPosition(idx);

                        Vector3 toPos = map.GetPosition(targetPosition + enemyShip.direction * i);
                        int distance = Mathf.Abs(pos1prev - (targetPosition + enemyShip.direction * i));
                        if (distance <= this.range)
                        {
                            //AddAffected(this.transform.position, toPos, card.rule.color);
                            preview.Set(card, this, fromPos, toPos, this.color);
                            Map.current.tiles[idx].SetHighlight(this.color);
                        }
                        else
                        {
                            preview.Set(card, this, fromPos, toPos, Color.gray);
                            // AddAffected(this.transform.position, toPos, Color.gray);
                        }
                    }
                }
            }
            else if (this.target == CardAction.AttackTarget.Moving)
            {
                int i = 1;
                //for (int i = 1; i < 3; ++i)
                {
                    int idx = targetPosition + enemyShip.direction * i;
                    if (idx * ownerShip.direction > pos1 * ownerShip.direction)
                    {
                        Vector3 cell = map.GetPosition(idx);
                        Vector3 toPos = map.GetPosition(targetPosition + enemyShip.direction * i);

                        int distance = Mathf.Abs(pos1prev - (targetPosition + enemyShip.direction * i));
                        if (distance <= this.range)
                        {
                            //AddAffected(this.transform.position, toPos, card.rule.color);
                            Map.current.tiles[idx].SetHighlight(this.color);
                            preview.Set(card, this, fromPos, toPos, this.color);
                        }
                        else
                        {
                            //AddAffected(this.transform.position, toPos, Color.gray);
                            preview.Set(card, this, fromPos, toPos, Color.gray);
                        }
                    }
                }
            }
            else if (this.target == CardAction.AttackTarget.notMoving)
            {
                int idx = targetPosition;
                if (idx * ownerShip.direction > pos1 * ownerShip.direction)
                {
                    Vector3 cell = map.GetPosition(idx);
                    Vector3 toPos = map.GetPosition(targetPosition);

                    int distance = Mathf.Abs(pos1prev - targetPosition);
                    if (distance <= this.range)
                    {
                        //AddAffected(this.transform.position, toPos, card.rule.color);
                        Map.current.tiles[idx].SetHighlight(this.color);
                        preview.Set(card, this, fromPos, toPos, this.color);
                    }
                    else
                    {
                        //AddAffected(this.transform.position, toPos, Color.gray);
                        preview.Set(card, this, fromPos, toPos, Color.gray);
                    }
                }
            }


        }
        if (this.ruleType == CardAction.RuleType.move && this.amount > 0)
        {
            for (int i = 0; i <= this.amount; ++i)
            {
                int idx = pos1 + ownerShip.direction * i;
                Vector3 cell = map.GetPosition(idx);
                Map.current.tiles[idx].SetHighlight(this.color);

                Vector3 toPos = map.GetPosition(pos1prev + ownerShip.direction * this.amount);
                preview.Set(card, this, fromPos, toPos, this.color);
                //AddAffected(this.transform.position, toPos, card.rule.color);
            }

        }

        if (this.ruleType == CardAction.RuleType.heal && this.amount > 0)
        {
            int idx = pos1;
            Vector3 cell = map.GetPosition(idx);
            Map.current.tiles[idx].SetHighlight(this.color);
            //AddAffected(this.transform.position, cell, card.rule.color);
        }
    }
}
