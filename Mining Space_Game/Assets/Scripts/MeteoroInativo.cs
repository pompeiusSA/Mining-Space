using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        meteoroVida = _gameController.vidaMeteoroMax;

        switch (this.gameObject.tag)
        {
            case "Untagged":

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

                break;

            case "meteoroFinal":

                recursosColhidos = 20;

                break;
        }
    }

    void Update()
    {
        if (_laserScr != null)
        {
            switch (this.gameObject.tag)
            {
                case "Untagged":

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

                            Instantiate(_gameController.particulasMeteoroInativo, transform.position, transform.localRotation);
                        }
                    }

                    break;

                case "meteoroFinal":

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

                            _gameController.isFim = true;

                            Instantiate(_gameController.particulasMeteoroFinal, transform.position, transform.localRotation);
                        }
                    }

                    break;
            }

        }
    }

    // Faz o meteoro tremer enquanto recebe dano do laser.
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
