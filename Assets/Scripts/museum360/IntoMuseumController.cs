using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntoMuseumController : MonoBehaviour
{

    // Assuming you have a reference to the GameObject
    public GameObject myObject;
    public List<Material> Materials;
    private int currentMaterialIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        myObject.GetComponent<Renderer>().material = Materials[currentMaterialIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToNextMaterial()
    {
        // Increment the index (loop back to 0 if at the end)
        currentMaterialIndex = (currentMaterialIndex + 1) % Materials.Count;

        // Assign the new material
        myObject.GetComponent<Renderer>().material = Materials[currentMaterialIndex];
    }

    public void SwitchToPreviousMaterial()
    {
        // Decrement the index (loop to the end if at 0)
        currentMaterialIndex = (currentMaterialIndex - 1 + Materials.Count) % Materials.Count;

        // Assign the new material
        myObject.GetComponent<Renderer>().material = Materials[currentMaterialIndex];
    }

    public void goToMainScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
