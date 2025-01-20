using UnityEngine;
using UnityEngine.Events;

public class GetOnClick : MonoBehaviour
{
    public void OnMouseDown()
    {
        Destroy(gameObject); // クリックでオブジェクトを破壊
    }
}
