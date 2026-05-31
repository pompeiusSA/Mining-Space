using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditorInternal;
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

    public float vidaNave;

    [Header("Gameplay configs")]

    public LayerMask layerMeteoro;

    public float duracaoShake;

    public float recursosQtd = 200f;

    public GameObject explosaoPrefab;

    public float[] tempMeteoroSpawn;

    public float[] tempDelayMeteoroSpawn;

    [Header("UI")]

    public Text recursosQtdText;

    public Text energiaNaveText;

    [Header("Meteoro Inativo")]

    public float vidaMeteoroMax = 100;

    public float danoLaserMeteoro = 0;

    public Color corFinalMeteoro;

    [Header("Meteoro Ativo")]

    public Transform[] posicoesSpawn;

    public MeteoroAtivo meteoroAtivoPrefab;

    [SerializeField] int numMeteoroMax;

    List<GameObject> meteorosCena = new List<GameObject>();

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

        if (ExisteMeteoroNaCena() == false)
        {
            if (numMeteoroMax <= 0)
            {
                numMeteoroMax = Random.Range(10, 50);

                StartCoroutine("delaySpawnMeteoros");
            }
        }

        print($"numero meteoro: {numMeteoroMax} e existe meteoros? {ExisteMeteoroNaCena()}");
    }

    IEnumerator delaySpawnMeteoros()
    {
        yield return new WaitForSeconds(Random.Range(tempDelayMeteoroSpawn[0], tempDelayMeteoroSpawn[1]));

        if (numMeteoroMax > 0)
        {
            StartCoroutine("spawnMeteoro");
        }
    }

    IEnumerator spawnMeteoro()
    {
        if (numMeteoroMax > 0)
        {
            yield return new WaitForSeconds(Random.Range(tempMeteoroSpawn[0], tempMeteoroSpawn[1]));

            numMeteoroMax -= 1;

            Instantiate(meteoroAtivoPrefab, posicoesSpawn[Random.Range(0, posicoesSpawn.Length)].position, transform.localRotation);

            StartCoroutine("spawnMeteoro");
        }
    }

    private bool ExisteMeteoroNaCena()
    {
        return FindAnyObjectByType<MeteoroAtivo>() != null;
    }
}
