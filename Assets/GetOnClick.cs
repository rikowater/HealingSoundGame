using UnityEngine;

public class GetOnClick : MonoBehaviour
{
    public void GetAtClick()
    {
        Destroy(gameObject);
        Debug.Log("Click");
    }
}