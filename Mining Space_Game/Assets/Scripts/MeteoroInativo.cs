using System.Collections;
using UnityEngine;

public class MeteoroInativo : MonoBehaviour
{
    GameController _gameController;

    LaserScr _laserScr;

    public AnimationCurve animCurveMeteoro;

    void Awake()
    {
        _gameController = FindAnyObjectByType(typeof(GameController)) as GameController;

        _laserScr = FindAnyObjectByType(typeof(LaserScr)) as LaserScr;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_laserScr.isShakeMeteoro == true)
        {
            _laserScr.isShakeMeteoro = false;
            StartCoroutine(shakeMeteoro());
        }

        if (_gameController.vidaMeteoro <= 0)
        {
            Destroy(this.gameObject);
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

        _laserScr.isShakeMeteoro = true;
    }
}
