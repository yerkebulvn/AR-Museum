using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate () {
            OnClick(param);
        });
    }
}
public class ObjectListView : MonoBehaviour
{
    [Serializable]
    public struct Exhibit
    {
        public string Name;
        public Sprite Icon;
        public string codename;
    }

    [SerializeField] Exhibit[] allExhibits;

    void Start()
    {
        GameObject buttonTemplate = transform.GetChild(0).gameObject;
        GameObject g;

        int N = allExhibits.Length;

        for (int i = 0; i < N; i++)
        {
            g = Instantiate(buttonTemplate, transform);
            g.transform.GetChild(0).GetComponent<Image>().sprite = allExhibits[i].Icon;
            g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = allExhibits[i].Name;

            /*g.GetComponent <Button> ().onClick.AddListener (delegate() {
				ItemClicked (i);
			});*/
            g.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }

        Destroy(buttonTemplate);
    }

    void ItemClicked(int itemIndex)
    {
        Debug.Log("------------item " + itemIndex + " clicked---------------");
        Debug.Log("name " + allExhibits[itemIndex].Name);
        GameObject.FindGameObjectWithTag("ViewController").GetComponent<ObjectViewLogic>().spawnObject(allExhibits[itemIndex].codename);
        GameObject.FindGameObjectsWithTag("MenuCanvas")[0].SetActive(false);
        FirebaseAnalytics.LogEvent("ObjectView", "ImageName", allExhibits[itemIndex].codename);
        FirebaseAnalytics.LogEvent("ObjectViewListSelected: " + allExhibits[itemIndex].codename);
    }
}
