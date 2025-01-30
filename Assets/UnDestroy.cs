using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnDestroy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
