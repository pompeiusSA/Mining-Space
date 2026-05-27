using UnityEngine;

public class LaserScr : MonoBehaviour
{
    Jogador _jogador;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _jogador = FindAnyObjectByType(typeof(Jogador)) as Jogador;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D colidido)
    {
        destruindoMeteoro(colidido);
    }

    void OnTriggerStay2D(Collider2D colidido)
    {
        destruindoMeteoro(colidido);
    }

    void destruindoMeteoro(Collider2D colidido)
    {
        if (_jogador.isLaser == true)
        {
            if (colidido.gameObject.tag == "meteoroInativo")
            {
                Destroy(colidido.gameObject);
            }
        }
    }
}
