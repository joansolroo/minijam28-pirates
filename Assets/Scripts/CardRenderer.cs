using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRenderer : MonoBehaviour {

    [SerializeField] public Card card;
    [SerializeField] public Color color;

    [SerializeField] TextMesh cardName;
    [SerializeField] TextMesh movement;
    [SerializeField] TextMesh damage;
    [SerializeField] TextMesh heal;

    [SerializeField] SpriteRenderer art;

    private void Start()
    {
        cardName.text = card.rule.name;
        movement.text = "x"+card.rule.movementAmount;
        damage.text = "x" + card.rule.damageAmount;
        heal.text = "x" + card.rule.healAmount;

        art.sprite = card.rule.art;
        art.color = card.rule.color;
    }
}
