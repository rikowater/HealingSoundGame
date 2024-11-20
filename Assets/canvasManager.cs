using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject menuPanel;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float transitionSpeed = 5.0f;

    private Coroutine currentCoroutine;
    private bool isMenuActive = false; // メニューの状態を追跡

    public void ToggleMenu()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        if (isMenuActive)
        {
            currentCoroutine = StartCoroutine(MoveToPosition(startPosition));
        }
        else
        {
            currentCoroutine = StartCoroutine(MoveToPosition(endPosition));
        }

        isMenuActive = !isMenuActive; // 状態をトグル
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
