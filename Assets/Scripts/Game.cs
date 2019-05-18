using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    [SerializeField] Player player1;
    [SerializeField] Player player2;

    // Use this for initialization
    void Start () {
        Setup();
	}
	
    void Setup()
    {
        for (int c = 0; c < 3; ++c)
        {
            player1.Draw();
            player2.Draw();
        }
        BeginTurn();
    }
    public void BeginTurn()
    {
        DummyAI();
    }

    public void OnResolveRequest()
    {
        if (player1.selected.cards.Count > 0)
        {
            ResolveAction();
            EndRound();
        }
    }
    public void ResolveAction()
    {
        bool ship1moved = false;
        bool ship2moved = false;
        // move
        foreach (Card c1 in player1.selected.cards)
        {
            ship1moved |= c1.Move();
        }
        foreach (Card c2 in player2.selected.cards)
        {
            ship2moved |= c2.Move();
        }

        bool ship1hit = false;
        bool ship2hit = false;
        // attack
        foreach (Card c1 in player1.selected.cards)
        {
            ship1hit |= c1.Attack(ship2moved);
        }
        foreach (Card c2 in player2.selected.cards)
        {
            ship2hit |= c2.Attack(ship1moved);
        }

        // repair
        foreach (Card c1 in player1.selected.cards)
        {
            c1.Repair();
        }
        foreach (Card c2 in player2.selected.cards)
        {
            c2.Repair();
        }

        if (player1.hp <= 0)
        {
            Debug.Log("You are dead!");
        }
        else if (player2.hp <= 0)
        {
            Debug.Log("You win!");
        }
    }
    void EndRound()
    {
        // discard
        player1.DiscardSelected();
        player1.Draw();

        player2.DiscardSelected();
        player2.Draw();

        BeginTurn();
    }


    void DummyAI()
    {
        Card c = player2.hand.cards[0];
        c.gameObject.SetActive(true);
        player2.Select(c);
    }

}
