using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item1Trigger : MonoBehaviour
{
    private GlobalObjectManager globalObjectManager;

    void Awake()
    {
        StartCoroutine(WaitForGlobalObjectManager());
    }

    private IEnumerator WaitForGlobalObjectManager()
    {
        while (globalObjectManager == null)
        {
            globalObjectManager = FindObjectOfType<GlobalObjectManager>();

            if (globalObjectManager != null)
            {
                Debug.Log("GlobalObjectManager found.");
                if (globalObjectManager.sharedObject != null)
                {
                    Debug.Log("Setting sharedObject to inactive.");
                    globalObjectManager.sharedObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("sharedObject is null in GlobalObjectManager!");
                }
            }
            else
            {
                Debug.Log("Waiting for GlobalObjectManager to be initialized...");
            }

            yield return null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && globalObjectManager != null && globalObjectManager.sharedObject != null)
        {
            globalObjectManager.sharedObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && globalObjectManager != null && globalObjectManager.sharedObject != null)
        {
            globalObjectManager.sharedObject.SetActive(false);
        }
    }
}
