using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] Fight fight;
    [SerializeField] UIIntroduction introductionScreen;
    [SerializeField] UIIntroduction wonScreen;
    [SerializeField] UIIntroduction looseScreen;
    

    public void Start()
    {
        fight.gameObject.SetActive(false);
        introductionScreen.gameObject.SetActive(true);
        looseScreen.gameObject.SetActive(false);
        wonScreen.gameObject.SetActive(false);

        introductionScreen.Play();
    }

    void Update()
    {
        if (introductionScreen.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            LaunchFight(introductionScreen);
        }
    }
    public void LaunchFight(UIIntroduction previous)
    {
        previous.gameObject.SetActive(false);
        fight.gameObject.SetActive(true);
    }
    public void WinFight()
    {
        fight.gameObject.SetActive(false);
        wonScreen.gameObject.SetActive(true);
        wonScreen.Play();
    }
    public void LoseFight()
    {
        fight.gameObject.SetActive(false);
        looseScreen.gameObject.SetActive(true);
        looseScreen.Play();
    }


    public void Equipe1()
    {
        Debug.Log("equipe card 1");
        LaunchFight(wonScreen);
    }
    public void Equipe2()
    {
        Debug.Log("equipe card 2");
        LaunchFight(wonScreen);
    }
    public void Loose1()
    {
        Debug.Log("loose card 1");
        LaunchFight(looseScreen);
    }
    public void Loose2()
    {
        Debug.Log("loose card 2");
        LaunchFight(looseScreen);
    }
}
