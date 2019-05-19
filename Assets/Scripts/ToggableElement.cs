using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggableElement : MonoBehaviour {

    [SerializeField] Transform hiddenPosition;
    public float duration = 5;
    public float interpolationTime = 1;
    public float interpolationOffset = 0;
    public bool fadeOut = true;
    // Use this for initialization
    void Start () {
        StartCoroutine(DoShow());
	}
	
    IEnumerator DoShow()
    {
        Vector3 hiddenPos = hiddenPosition.transform.position;
        Vector3 showPos = this.transform.position;
        this.transform.position = hiddenPos;
        yield return new WaitForSeconds(interpolationOffset);

        for (float t = 0; t < interpolationTime; t+=Time.deltaTime) {
            this.transform.position = Vector3.Lerp(hiddenPos, showPos, t / interpolationTime);
            yield return new WaitForEndOfFrame();
        }

        if(fadeOut)
        {
            yield return new WaitForSeconds(duration - interpolationOffset - 2* interpolationTime);
            for (float t = 0; t < interpolationTime; t += Time.deltaTime)
            {
                this.transform.position = Vector3.Lerp(showPos, hiddenPos, t / interpolationTime);
                yield return new WaitForEndOfFrame();
            }
            this.gameObject.SetActive(false);
        }
    }
}
