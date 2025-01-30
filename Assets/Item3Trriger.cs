using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3Trigger : MonoBehaviour
{
    private GlobalObjectManager2 globalObjectManager2;

    void Awake()
    {
        StartCoroutine(WaitForGlobalObjectManager2());
    }

    private IEnumerator WaitForGlobalObjectManager2()
    {
        while (globalObjectManager2 == null)
        {
            globalObjectManager2 = FindObjectOfType<GlobalObjectManager2>();

            if (globalObjectManager2 != null)
            {
                Debug.Log("GlobalObjectManager2 found.");
                if (globalObjectManager2.sharedObject != null)
                {
                    Debug.Log("Setting sharedObject to inactive.");
                    globalObjectManager2.sharedObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("sharedObject is null in GlobalObjectManager2!");
                }
            }
            else
            {
                Debug.Log("Waiting for GlobalObjectManager2 to be initialized...");
            }

            yield return null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && globalObjectManager2 != null && globalObjectManager2.sharedObject != null)
        {
            globalObjectManager2.sharedObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && globalObjectManager2 != null && globalObjectManager2.sharedObject != null)
        {
            globalObjectManager2.sharedObject.SetActive(false);
        }
    }
}
