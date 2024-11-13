using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasManager : MonoBehaviour
{
    public GameObject menuPanel;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float transitionSpeed = 5.0f;

    private Coroutine currentCoroutine;

    public void ToggleMenu()
    {
        //menuPanel.SetActive(!menuPanel.activeSelf);
    }

    public void ActiveMenu()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(MoveToPosition(endPosition));
    }

    public void NegativeMenu()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(MoveToPosition(startPosition));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(menuPanel.transform.position, targetPosition) > 0.01f)
        {
            menuPanel.transform.position = Vector3.Lerp(menuPanel.transform.position, targetPosition, Time.deltaTime * transitionSpeed);
            yield return null;
        }
        menuPanel.transform.position = targetPosition;
    }
}
