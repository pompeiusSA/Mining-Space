using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;

public class LaserScr : MonoBehaviour
{
    Jogador _jogador;

    GameController _gameController;

    private SpriteRenderer sr;

    void Awake()
    {
        _jogador = FindAnyObjectByType(typeof(Jogador)) as Jogador;

        _gameController = FindAnyObjectByType(typeof(GameController)) as GameController;

        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 origem = transform.position;
        Vector2 direcao = transform.up;
        float larguraOriginal = sr.sprite.bounds.size.y;
        float larguraReal = larguraOriginal * Mathf.Abs(transform.lossyScale.y);

        RaycastHit2D hit = Physics2D.Raycast(origem, direcao, larguraReal, _gameController.layerMeteoro);

        //Debug.DrawRay(origem, direcao * larguraReal, Color.red);

        if (hit.collider != null && _jogador.isLaser == true)
        {
            MeteoroInativo meteoro = hit.collider.GetComponent<MeteoroInativo>();

            meteoro.meteoroVida -= _gameController.danoLaserMeteoro * Time.fixedDeltaTime;

            _gameController.recursosQtd += meteoro.recursosColhidos * 10 * Time.fixedDeltaTime;

            _gameController.energiaNaveAtual += meteoro.recursosColhidos * 5 * Time.fixedDeltaTime;

            hit.collider.GetComponent<SpriteRenderer>().color = Color.Lerp(hit.collider.GetComponent<SpriteRenderer>().color, _gameController.corFinalMeteoro, meteoro.meteoroVida / 10000);

            meteoro.isShakeMeteoro = true;
        }
    }
}
