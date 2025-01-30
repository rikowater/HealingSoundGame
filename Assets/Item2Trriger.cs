using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item2Trigger : MonoBehaviour
{
    private GlobalObjectManager1 globalObjectManager1;

    void Awake()
    {
        StartCoroutine(WaitForGlobalObjectManager1());
    }

    private IEnumerator WaitForGlobalObjectManager1()
    {
        while (globalObjectManager1 == null)
        {
            globalObjectManager1 = FindObjectOfType<GlobalObjectManager1>();

            if (globalObjectManager1 != null)
            {
                Debug.Log("GlobalObjectManager1 found.");
                if (globalObjectManager1.sharedObject != null)
                {
                    Debug.Log("Setting sharedObject to inactive.");
                    globalObjectManager1.sharedObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("sharedObject is null in GlobalObjectManager1!");
                }
            }
            else
            {
                Debug.Log("Waiting for GlobalObjectManager1 to be initialized...");
            }

            yield return null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && globalObjectManager1 != null && globalObjectManager1.sharedObject != null)
        {
            globalObjectManager1.sharedObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && globalObjectManager1 != null && globalObjectManager1.sharedObject != null)
        {
            globalObjectManager1.sharedObject.SetActive(false);
        }
    }
}
