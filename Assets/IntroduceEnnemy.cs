using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroduceEnnemy : MonoBehaviour
{
    public Image headSprite;
    public ToggableElement headSlide;
    public IntroSlide introslide;

    public GameObject dialogPrefab;

    private List<GameObject> temp = new List<GameObject>();

    public void Start()
    {
        dialogPrefab.SetActive(false);
    }

    public void Setup(EnemyData ennemy)
    {
        if(temp.Count > 0)
        {
            foreach(GameObject go in temp)
            {
                Destroy(go);
            }
        }
        introslide.elements.Clear();

        float singleTime = 3.0f;
        headSprite.sprite = ennemy.face;
        headSlide.duration = singleTime * ennemy.dialog.Count+1;
        introslide.activatedTime = headSlide.duration;

        int i = 0;
        foreach(string s in ennemy.dialog)
        {
            Debug.Log(s);
            GameObject go = Instantiate(dialogPrefab);
            temp.Add(go);
            ToggableElement te = go.GetComponent<ToggableElement>();
            te.duration = singleTime*(i+1) + 1;
            te.interpolationOffset = singleTime * i + 1;
            go.transform.GetChild(0).GetComponent<Text>().text = s;
            go.transform.parent = introslide.transform;
            go.transform.position = dialogPrefab.transform.position;
            te.showPos = dialogPrefab.transform.position;
            go.transform.rotation = dialogPrefab.transform.rotation;
            go.transform.localScale = dialogPrefab.transform.localScale;
            go.SetActive(true);
            introslide.elements.Add(te);
            i++;
        }
        this.gameObject.GetComponent<UIIntroduction>().Play();
    }
}
