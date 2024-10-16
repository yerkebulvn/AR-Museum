
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Assistant_Listenner : MonoBehaviour
{
    public bool isScanning = false;
    public GameObject assistant;
    public Button btnAssist;
    //public Button btnImage;
    public Button btnObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleScan()
    {
        if (isScanning)
        {
            // Остановите сканирование объектов.
            assistant.SetActive(false);
            Debug.Log("*** Assistant stopped ***.");
            btnAssist.GetComponentInChildren<TMP_Text>().text = "?";
            //btnImage.gameObject.SetActive(true);
            btnObject.gameObject.SetActive(true);
        }
        else
        {
            // Возобновите сканирование объектов.
            assistant.SetActive(true);
            Debug.Log("*** Assistant started ***");
            btnAssist.GetComponentInChildren<TMP_Text>().text = "X";
            //btnImage.gameObject.SetActive(false);
            btnObject.gameObject.SetActive(false);
        }

        isScanning = !isScanning;
    }
}
