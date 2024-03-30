using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantConrtol : MonoBehaviour
{
    [Header("В ячейку перенеси объект ARCamera")]
    public GameObject ARCamera;
    [Header("В ячейку перенеси префаб Ассистента")]
    public GameObject AssistantPrefab;
    private GameObject Assistant;
    [Header("В ячейку перенесите позицию Ассистента")]
    public Transform AssistantPosition;

    void Start()
    {
        // Появление Ассистента
        Assistant = Instantiate(AssistantPrefab, ARCamera.transform.position + new Vector3(0, 1f, 0), ARCamera.transform.rotation);
        // Запуск отчёта времени
        StartCoroutine(routine: CoroutineSample());
    }

    void Update()
    {
        // Проверка на дистанцию
        if (CheckDist() >= 0.1f)
        {
            MoveObjToPos();
        }
        // Поворот Ассистента в сторону пользователя
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
        Assistant.SetActive(false);
    }
}
