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

    public float energiaNaveAtual;

    public float[] energiaGasta;

    [Header("Gameplay configs")]

    public LayerMask layerMeteoro;

    public float duracaoShake;

    public float recursosQtd = 200f;

    [Header("UI")]

    public Text recursosQtdText;

    public Text energiaNaveText;

    [Header("Meteoro Inativo")]

    public float vidaMeteoroMax = 100;

    public float danoLaserMeteoro = 0;

    public Color corFinalMeteoro;

    void Awake()
    {
        energiaNaveAtual = recursosQtd / 2;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        recursosQtdText.text = ((int)recursosQtd).ToString();

        energiaNaveText.text = ((int)energiaNaveAtual).ToString();
    }
}
