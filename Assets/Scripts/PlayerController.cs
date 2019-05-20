using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField] TextMesh cardDescription;
    [SerializeField] Card helpCard;

    [SerializeField] LineRenderer lineHand1;
    [SerializeField] LineRenderer lineHand2;
    [SerializeField] LineRenderer lineSelect1;
    [SerializeField] LineRenderer lineSelect2;
    [SerializeField] LineRenderer lineDiscard1;
    [SerializeField] LineRenderer lineDiscard2;
    // Use this for initialization


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Fight.current.isSolving && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Card c = hit.transform.gameObject.GetComponent<Card>();
                if (c)
                {
                    if (c.region.region == CardRegion.Region.Hand && c.owner == player)
                    {
                        player.Select(c);
                    }
                    else if (c.region.region == CardRegion.Region.Selected && c.owner == player)
                    {
                        player.DeSelect(c);
                    }
                }
            }
        }
        else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            cardDescription.text = "";
            helpCard.gameObject.SetActive(false);

            lineHand1.gameObject.SetActive(false);
            lineHand2.gameObject.SetActive(false);
            lineSelect1.gameObject.SetActive(false);
            lineSelect2.gameObject.SetActive(false);
            lineDiscard1.gameObject.SetActive(false);
            lineDiscard2.gameObject.SetActive(false);
            if (Physics.Raycast(ray, out hit))
            {
                Card c = hit.transform.gameObject.GetComponent<Card>();
                if (c)
                {
                    if (c.visible)
                    {
                        cardDescription.text = "Actions in Order:\n"+c.rule.description;
                        helpCard.owner = c.owner;
                        helpCard.rule = c.rule;
                        helpCard.gameObject.SetActive(true);
                        helpCard.GetComponent<CardRenderer>().UpdateLayout();

                        if (c.owner == this.player)
                        {
                            if (c.region.region == CardRegion.Region.Hand)
                            {
                                lineHand1.gameObject.SetActive(true);
                            }
                            else if (c.region.region == CardRegion.Region.Selected)
                            {
                                lineSelect1.gameObject.SetActive(true);
                            }
                            else if (c.region.region == CardRegion.Region.Discard)
                            {
                                lineDiscard1.gameObject.SetActive(true);
                            }
                        }
                        else
                        {

                            if (c.region.region == CardRegion.Region.Hand)
                            {
                                lineHand2.gameObject.SetActive(true);
                            }
                            else if (c.region.region == CardRegion.Region.Selected)
                            {
                                lineSelect2.gameObject.SetActive(true);
                            }
                            else if (c.region.region == CardRegion.Region.Discard)
                            {
                                lineDiscard2.gameObject.SetActive(true);
                            }
                        }
                    }
                }

            }

        }
    }
}
