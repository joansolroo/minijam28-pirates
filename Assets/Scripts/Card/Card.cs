using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    [SerializeField] public Map map;
    [SerializeField] public Player owner;
    public CardRegion region;
    [SerializeField] public CardData rule;

    public static Vector3 CARD_SIZE = new Vector3(3.2f, 4.5f, 0.15f);

    public bool visible = false;

    private void OnValidate()
    {
        string name = "";
        if (owner != null)
        {
            name += owner.name;
            name += " ";
        }
        if (rule != null)
        {
            name += rule.name;
        }
        this.gameObject.name = name;
    }
    public bool WillMove()
    {
        return (rule.rules[1] != null && rule.rules[1].amount > 0);
       
    }
    public bool DoMove()
    {
        if (rule.rules[1] != null)
        {
            return rule.rules[1].Resolve(this);
        }
        return false;
       /* if (rule.movementAmount > 0)
        {
            owner.ship.MoveTo(owner.ship.position+ owner.ship.direction*rule.movementAmount);
            owner.ship.audioSource.PlayOneShot(this.rule.moveClip);
            return true;
        }
        else
        {
            return false;
        }*/
    }

    public bool DoAttackMoved(bool enemyMoved)
    {
        if (rule.rules[2] != null)
        {
            return rule.rules[2].Resolve(this);
        }
        return false;
        /*
        if (rule.damageAmount > 0)
        {
            int pos1 = owner.ship.position;
            int pos2 = owner.enemy.ship.position;
            if (rule.attackMaxRange > Mathf.Abs(pos1 - pos2))
            {
                bool hit = ((rule.attackTarget == CardData.AttackTarget.All)
                           || (rule.attackTarget == CardData.AttackTarget.Moving && enemyMoved));

                if (!hit)
                {
                    Debug.Log("Attack: attack missed");
                    ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                    parameter.startColor = Color.gray;
                    map.tiles[pos2-owner.enemy.ship.direction].emission.Emit(parameter, Random.Range(5, 15));
                    owner.ship.audioSource.PlayOneShot(this.rule.attackClip);
                }
                else
                {
                    Debug.Log("Attack: attack hit");

                    owner.enemy.Hurt(rule);

                    ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                    parameter.startColor = owner.enemy.color;
                    map.tiles[pos2].emission.Emit(parameter, Random.Range(30, 50));
                    owner.ship.audioSource.PlayOneShot(this.rule.attackClip);
                    owner.enemy.ship.audioSource.clip = this.rule.explosionClip;
                    owner.enemy.ship.audioSource.PlayDelayed(0.25f);
                }

                return hit;
                
            }
            else
            {
                Debug.Log("Attack: Out of range");
            }
            
        }
        return false;*/
    }
    public bool DoAttackNotMoved(bool enemyWillMove,int pos1, int pos2)
    {
        if (rule.rules[0] != null)
        {
            return rule.rules[0].Resolve(this);
        }
        return false;
        /*
        if (rule.damageAmount > 0)
        {

            if (rule.attackMaxRange >= Mathf.Abs(pos1 - pos2))
            {
                bool hit = ((rule.attackTarget == CardData.AttackTarget.All)
                           || (rule.attackTarget == CardData.AttackTarget.notMoving && !enemyWillMove));

                if (!hit)
                {
                    Debug.Log("Attack: attack missed");
                    ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                    parameter.startColor = Color.gray;
                    map.tiles[pos2].emission.Emit(parameter, Random.Range(5, 15));
                    owner.ship.audioSource.PlayOneShot(this.rule.attackClip);
                }
                else
                {
                    Debug.Log("Attack: attack hit");
                    
                    owner.enemy.Hurt(rule);

                    ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                    parameter.startColor = owner.enemy.color;
                    map.tiles[pos2].emission.Emit(parameter, Random.Range(30, 50));
                    owner.ship.audioSource.PlayOneShot(this.rule.attackClip);
                    owner.enemy.ship.audioSource.clip = this.rule.explosionClip;
                    owner.enemy.ship.audioSource.PlayDelayed(0.25f);
                }

                return hit;

            }
            else
            {
                Debug.Log("Attack: Out of range");
            }

        }
        return false;*/
    }
    public void DoSpecial()
    {
        if (rule.rules[3] != null)
        {
            rule.rules[3].Resolve(this);
        }
    }
    /*
    public void DoSpecial()
    {
        
        if (this.rule.healAmount > 0)
        {
            owner.Heal(this.rule.healAmount, this.rule.recoverCard);

            ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
            parameter.startColor = rule.color;
            map.tiles[owner.ship.position].emission.Emit(parameter, Random.Range(5, 15));
            owner.ship.audioSource.PlayOneShot(this.rule.healClip);
        }
        if (rule.scan)
        {
            foreach (Card c in owner.enemy.hand.cards)
            {
                
                c.SetVisible(true);

                ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                parameter.startColor = rule.color;
                map.tiles[owner.enemy.ship.position].emission.Emit(parameter, Random.Range(5, 15));
                owner.ship.audioSource.PlayOneShot(this.rule.scanClip);
            }
        }
        if(rule.boarding)
        {
            if(Mathf.Abs(owner.ship.position - owner.enemy.ship.position)<=1)
            {
                Fight.current.OnBoard(owner, owner.enemy);

                ParticleSystem.EmitParams parameter = new ParticleSystem.EmitParams();
                parameter.startColor = owner.color;
                map.tiles[owner.enemy.ship.position].emission.Emit(parameter, Random.Range(5, 15));
                owner.ship.audioSource.PlayOneShot(this.rule.boardClip);
            }
        }
    }
    */

    public void SetVisible(bool visible)
    {
        if (this.visible != visible)
        {
            this.visible = visible;
            StartCoroutine(DoMoveCardTo(this.region, this.transform.position, visible));
        }
    }
    public void MoveTo(CardRegion region, Vector3 localPosition)
    {
        this.region = region;
        Vector3 targetPosition =  region.transform.TransformPoint(localPosition);
        if (Vector3.Distance(this.transform.position, targetPosition) > 0.1f)
        {
            StopCoroutine(DoMoveCardTo(region, targetPosition, visible));
            StartCoroutine(DoMoveCardTo(region, targetPosition, visible));
        }
    }

    public bool isAnimating = false;
    IEnumerator DoMoveCardTo(CardRegion region, Vector3 targetPosition, bool visible)
    {
       // if (!isAnimating)
        {
            isAnimating = true;
            Vector3 fromPos = this.transform.position;
            Vector3 toPos = targetPosition;
            Vector3 middlePos = (fromPos + toPos) / 2 + Vector3.up * 5;
            Vector3 fromRot = this.transform.localEulerAngles;
            Vector3 toRot = visible ? new Vector3(0, 0, 0) : new Vector3(0, 0, 180);
            float animationDuration = 0.25f;
            float delta = Time.deltaTime / animationDuration;
            for (float t = 0; t < 1f; t += delta)
            {
                if (t < 0.5f)
                {
                    this.transform.position = Vector3.Lerp(fromPos, middlePos, t * 2);
                }
                else
                {
                    this.transform.position = Vector3.Lerp(middlePos, toPos, t * 2 - 1);
                }
                this.transform.localEulerAngles = Vector3.Lerp(fromRot, toRot, t);
                yield return new WaitForEndOfFrame();
            }
            this.transform.position = toPos;
            this.transform.localEulerAngles = toRot;

            isAnimating = false;
        }
    }
}
