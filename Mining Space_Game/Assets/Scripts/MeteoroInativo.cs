using System.Collections;
using UnityEngine;

public class MeteoroInativo : MonoBehaviour
{
    GameController _gameController;

    LaserScr _laserScr;

    public AnimationCurve animCurveMeteoro;

    public float meteoroVida;

    public float qualidadeMeteoro;

    public float recursosColhidos;

    public bool isShakeMeteoro = false;

    void Awake()
    {
        _gameController = FindAnyObjectByType(typeof(GameController)) as GameController;

        _laserScr = FindAnyObjectByType(typeof(LaserScr)) as LaserScr;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meteoroVida = _gameController.vidaMeteoroMax;

        qualidadeMeteoro = Random.Range(0, 100);

        if (qualidadeMeteoro >= 85)
        {
            recursosColhidos = 1;
        }
        else if (qualidadeMeteoro >= 35)
        {
            recursosColhidos = 0.1f;
        }
        else
        {
            recursosColhidos = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_laserScr.GetComponent<SpriteRenderer>().enabled == true)
        {
            if (isShakeMeteoro == true)
            {
                isShakeMeteoro = false;
                StartCoroutine(shakeMeteoro());
            }

            if (meteoroVida <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator shakeMeteoro()
    {
        Vector3 posicaoInicial = transform.position;

        float tempoInicial = 0f;

        while (tempoInicial < _gameController.duracaoShake)
        {
            tempoInicial += Time.deltaTime;

            float forca = animCurveMeteoro.Evaluate(tempoInicial / _gameController.duracaoShake);

            transform.position = posicaoInicial + Random.insideUnitSphere * forca;

            yield return null;
        }

        transform.position = posicaoInicial;

        isShakeMeteoro = true;
    }
}
