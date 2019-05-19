using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSlide : MonoBehaviour {

    public float activatedTime = 0;
    public List<ToggableElement> elements = new List<ToggableElement>();

    public void Play()
    {
        foreach(ToggableElement e in elements)
        {
            e.Play();
        }
    }

}
