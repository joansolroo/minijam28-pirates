using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntroduction : MonoBehaviour {

    public IntroSlide[] slides;
    public void Start()
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
}
