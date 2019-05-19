using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntroduction : MonoBehaviour {

    public Game game;
    public IntroSlide[] slides;
    public void Play()
    {
        StartCoroutine(ChangeSlides());
    }

    IEnumerator ChangeSlides()
    {
        foreach (IntroSlide s in slides)
        {
            s.gameObject.SetActive(false);
        }
        foreach (IntroSlide s in slides)
        {
            s.gameObject.SetActive(true);
            yield return new WaitForSeconds(s.activatedTime);
        }
    }

    public void LaunchFight()
    {
        game.LaunchFight(this);
    }
    public void LaunchEnnemyIntruduction()
    {
        game.LaunchEnnemyIntruduction(this);
    }
}
