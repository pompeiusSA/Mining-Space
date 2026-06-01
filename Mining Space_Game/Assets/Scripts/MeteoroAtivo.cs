using NUnit.Framework;
using UnityEngine;

public class MeteoroAtivo : MonoBehaviour
{
    Rigidbody2D rb;

    GameController _gameController;

    Jogador _jogador;

    bool isJogadorVivo;

    float meteoroVel;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        _gameController = FindAnyObjectByType(typeof(GameController)) as GameController;

        _jogador = FindAnyObjectByType(typeof(Jogador)) as Jogador;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meteoroVel = Random.Range(100, 150);

        ChecandoPlayerVivo();

        if (ChecandoPlayerVivo() == true)
        {
            transform.right = _jogador.transform.position - transform.position;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = transform.right * meteoroVel * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        ChecandoPlayerVivo();
    }

    void OnTriggerEnter2D(Collider2D colidido)
    {
        if (isJogadorVivo == true)
        {
            switch (colidido.gameObject.tag)
            {
                case "balaPlayer":
                    Instantiate(_gameController.explosaoPrefab, transform.position, transform.localRotation);
                    Instantiate(_gameController.particulasMeteoro, transform.position, transform.localRotation);
                    Destroy(this.gameObject);
                    Destroy(colidido.gameObject);
                    break;

                case "Player":
                    _gameController.vidaNave -= 10;

                    _gameController.isDano = true;

                    _gameController._camera.isShakeMeteoro = true;

                    Instantiate(_gameController.explosaoPrefab, transform.position, transform.localRotation);

                    _gameController._camera.isShakeMeteoro = true;

                    Destroy(this.gameObject);
                    break;
            }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    bool ChecandoPlayerVivo()
    {
        if (_jogador != null)
        {
            isJogadorVivo = true;
        }
        else
        {
            isJogadorVivo = false;
        }

        bool resultado = isJogadorVivo;

        return resultado;
    }
}
