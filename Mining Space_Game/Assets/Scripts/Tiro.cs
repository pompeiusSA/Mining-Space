using UnityEngine;

public class Tiro : MonoBehaviour
{
    void OnBecameInvisible()
    {
        // Destroi o tiro quando ele sai da tela.
        Destroy(this.gameObject);
    }
}
