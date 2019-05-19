using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggableElement : MonoBehaviour {

    [SerializeField] Transform hiddenPosition;
    public float duration = 5;
    public float interpolationTime = 1;
    public float interpolationOffset = 0;
    public bool fadeOut = true;

    public Vector3 showPos;

    private void Awake()
    {
        showPos = transform.position;
    }
    public void Play()
    {
        StartCoroutine(DoShow());
    }

    bool animating = false;
    IEnumerator DoShow()
    {
        if(!animating)
        {
            animating = true;

            Vector3 hiddenPos = hiddenPosition.transform.position;
            this.transform.position = hiddenPos;
            yield return new WaitForSeconds(interpolationOffset);

            for (float t = 0; t < interpolationTime; t += Time.deltaTime)
            {
                this.transform.position = Vector3.Lerp(hiddenPos, showPos, t / interpolationTime);
                Debug.Log("eee");
                yield return new WaitForEndOfFrame();
            }

            if (fadeOut)
            {
                yield return new WaitForSeconds(duration - interpolationOffset - 2 * interpolationTime);
                for (float t = 0; t < interpolationTime; t += Time.deltaTime)
                {
                    this.transform.position = Vector3.Lerp(showPos, hiddenPos, t / interpolationTime);
                    Debug.Log("fff");
                    yield return new WaitForEndOfFrame();
                }
            }
            animating = false;
        }
        else
        {
            Debug.LogWarning("calling twice the DoShow before it finished");
        }
    }
}
