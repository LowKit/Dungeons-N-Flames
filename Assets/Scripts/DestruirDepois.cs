using UnityEngine;

public class DestruirDepois : MonoBehaviour
{
    public float tempo = 0.3f;
    void Start()
    {
        Destroy(gameObject, tempo);
    }
}