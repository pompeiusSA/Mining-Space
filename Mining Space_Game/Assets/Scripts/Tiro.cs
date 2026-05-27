using UnityEngine;

public class Tiro : MonoBehaviour
{
    void OnBecameInvisible()
    {
        //De destruindo após sair da tela 

        Destroy(this.gameObject);
    }
}
