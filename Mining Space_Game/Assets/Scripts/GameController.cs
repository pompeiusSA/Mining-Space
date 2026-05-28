using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Player")]

    public float velPlayer;

    public float velPlayerRotacao;

    public float delayRePlayer;

    public GameObject balaPrefabs;

    public GameObject laserObject;

    public float velBalaPlayer;

    public float tempoDelayBala;

    [Header("Gameplay configs")]

    public LayerMask layerMeteoro;

    public float duracaoShake;

    public float recursosQtd = 200f;

    public float recursosColhidos;

    [Header("UI")]

    public Text recursosQtdText;

    [Header("Meteoro Inativo")]

    public float vidaMeteoroMax = 100;

    public float danoLaserMeteoro = 0;

    public Color corFinalMeteoro;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        recursosQtdText.text = ((int)recursosQtd).ToString();
    }
}
