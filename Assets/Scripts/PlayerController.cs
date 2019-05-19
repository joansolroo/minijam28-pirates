using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] Player player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!Fight.current.isSolving && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Card c = hit.transform.gameObject.GetComponent<Card>();
                if(c)
                {
                    if (c.region.region == CardRegion.Region.Hand && c.owner == player)
                    {
                        player.Select(c);
                    }
                }
            }
        }
        
    }
}
