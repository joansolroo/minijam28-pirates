using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField] Player player;
    [SerializeField] public EnemyData data;

    public void SelectCard()
    {
        Card c = player.hand.cards[Random.Range(0, player.hand.cards.Count - 1)];
        player.Select(c);
    }
}
