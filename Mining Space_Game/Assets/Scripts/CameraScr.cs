using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScr : MonoBehaviour
{
    [SerializeField] float duracaoShake;
    public bool isShakeMeteoro;
    [SerializeField] AnimationCurve animCurveMeteoro;
    GameController _gameController;

    void Awake()
    {
        _gameController = FindAnyObjectByType(typeof(GameController)) as GameController;
    }
    // Update is called once per frame
    void Update()
    {
        if (isShakeMeteoro == true)
        {
            StartCoroutine("shake");
        }
    }

    IEnumerator shake()
    {
        Vector3 posicaoInicial = transform.position;

        float tempoInicial = 0f;

        while (tempoInicial < duracaoShake)
        {
            _gameController.materialCam.color = _gameController.corDanoCamera;
            tempoInicial += Time.deltaTime;

            float forca = animCurveMeteoro.Evaluate(tempoInicial / duracaoShake);

            transform.position = posicaoInicial + Random.insideUnitSphere * forca;

            yield return null;
        }

        transform.position = posicaoInicial;

        _gameController.materialCam.color = _gameController.corCamera;

        isShakeMeteoro = false;

        _gameController.isDano = false;
    }
}
