using UnityEngine;

public class Immortale : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject.transform);
    }
}
