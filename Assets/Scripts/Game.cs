using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    [SerializeField] Player player1;
    [SerializeField] Player player2;
    [SerializeField] Map map;

    // Use this for initialization
    void Start () {
        Setup();
	}
	
    void Setup()
    {
        player1.RemakeDeck();
        player1.enemy = player2;
        player2.RemakeDeck();
        player2.enemy = player1;
        map.Move(player1.ship, 0);
        map.Move(player2.ship, 0);
        for (int c = 0; c < 3; ++c)
        {
            player1.Draw();
            player2.Draw();
        }
        BeginTurn();
    }

    private void OnDrawGizmos()
    {
        //PreviewPlayer(player1);
        //PreviewPlayer(player2);
        
    }
    void PreviewPlayer(Player player)
    {
        {
            
        }
    }
    public void BeginTurn()
    {
       // Debug.Log("Turn begins");
        DummyAI();
    }

    public void OnResolveRequest()
    {
        if (player1.selected.cards.Count > 0)
        {
            ResolveAction();
            EndRound();
        }
        else
        {
            Debug.Log("no card picked");
        }
    }

    public void ResolveAction()
    {
        //Debug.Log("Solving turn");
        bool ship1moved = false;
        bool ship2moved = false;
        // move
        foreach (Card c1 in player1.selected.cards)
        {
            ship1moved |= c1.DoMove();
        }
        foreach (Card c2 in player2.selected.cards)
        {
            ship2moved |= c2.DoMove();
        }

        bool ship1hit = false;
        bool ship2hit = false;
        // attack
        foreach (Card c1 in player1.selected.cards)
        {
            ship1hit |= c1.DoAttack(ship2moved);
        }
        foreach (Card c2 in player2.selected.cards)
        {
            ship2hit |= c2.DoAttack(ship1moved);
        }

        // repair
        foreach (Card c1 in player1.selected.cards)
        {
            c1.DoRepair();
        }
        foreach (Card c2 in player2.selected.cards)
        {
            c2.DoRepair();
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
        //Debug.Log("Ending turn");
        // discard
        player1.DiscardSelected();
        player1.Draw();

        player2.DiscardSelected();
        player2.Draw();

        BeginTurn();
    }


    void DummyAI()
    {
      //  Debug.Log("Enemy movement");
        Card c = player2.hand.cards[0];
        player2.Select(c);
    }

}
