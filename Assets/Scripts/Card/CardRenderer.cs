using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRenderer : MonoBehaviour {

    [SerializeField] public Card card;
    [SerializeField] public Color color;

    [SerializeField] TextMesh cardName;
    [SerializeField] TextMesh movement;
    [SerializeField] TextMesh damage;
    [SerializeField] TextMesh range;
    [SerializeField] TextMesh type;
    [SerializeField] TextMesh heal;

    [SerializeField] SpriteRenderer art;
    [SerializeField] SpriteRenderer factionIcon;
    [SerializeField] SpriteRenderer[] PlayerColoredElements;

    private void Start()
    {
        UpdateLayout();
    }

    public void UpdateLayout()
    {
        Color color = card.owner.color;
        factionIcon.sprite = card.owner.faction;
        foreach (SpriteRenderer element in PlayerColoredElements)
        {
            element.color = color;
        }
        cardName.text = card.rule.name;
        cardName.GetComponent<MeshRenderer>().material.color = card.rule.color;
        /*if (card.rule.movementAmount <= 0)
        {
            movement.transform.parent.gameObject.SetActive(false);

        }
        else
        {
            movement.text = "x" + card.rule.movementAmount;
        }
        if (card.rule.damageAmount <= 0)
        {
            damage.transform.parent.gameObject.SetActive(false);
            range.transform.parent.gameObject.SetActive(false);
            type.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            damage.text = "x" + card.rule.damageAmount;
            range.text = "" + card.rule.attackMaxRange;
            switch (card.rule.attackTarget)
            {
                case CardData.AttackTarget.All:
                    type.text = "*";
                    break;
                case CardData.AttackTarget.Moving:
                    type.text = "M";
                    break;
                case CardData.AttackTarget.notMoving:
                    type.text = "S";
                    break;
            }

        }
        if (card.rule.healAmount <= 0)
        {
            heal.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            heal.text = "x" + card.rule.healAmount;
        }*/
        art.sprite = card.rule.art;
        art.color = card.rule.color;
    }
}
