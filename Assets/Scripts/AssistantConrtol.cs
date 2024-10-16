using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssistantConrtol : MonoBehaviour
{
    [Header("� ������ �������� ������ ARCamera")]
    public GameObject ARCamera;
    [Header("� ������ �������� ������ ����������")]
    public GameObject AssistantPrefab;
    private GameObject Assistant;
    [Header("� ������ ���������� ������� ����������")]
    public Transform AssistantPosition;
    public GameObject AssistantControl;
    public Button btnAssistant;

    private void OnEnable()
    {
        // ��������� ����������
        Assistant = Instantiate(AssistantPrefab, ARCamera.transform.position + new Vector3(0, 1f, 0), ARCamera.transform.rotation);
        Debug.Log("--- Assistant Prefab Spawn ---");
        // ������ ������ �������
        StartCoroutine(routine: CoroutineSample());
        Debug.Log("* Start counttime *");
    }

    void Update()
    {
        // �������� �� ���������
        if (CheckDist() >= 0.1f)
        {
            MoveObjToPos();
        }
        // ������� ���������� � ������� ������������
        Assistant.transform.LookAt(ARCamera.transform);
    }

    public float CheckDist()
    {
        float dist = Vector3.Distance(Assistant.transform.position, AssistantPosition.transform.position);
        return dist;
    }

    private void MoveObjToPos()
    {
        Assistant.transform.position = Vector3.Lerp(Assistant.transform.position, AssistantPosition.position, 1f * Time.deltaTime);
    }

    private IEnumerator CoroutineSample()
    {
        yield return new WaitForSeconds(60);
        //Assistant.SetActive(false);
        Destroy(Assistant);
        btnAssistant.GetComponentInChildren<TMP_Text>().text = "?";
        AssistantControl.gameObject.SetActive(false);
        Debug.Log("--- Assistant Killed ---");
    }

    private void OnDisable()
    {
        Destroy(Assistant);
    }
}
