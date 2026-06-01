using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public enum estagiosFase
{
    exploracao,
    sobrevivencia,
}

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

    Jogador _player;

    [Header("Gameplay configs")]

    public LayerMask layerMeteoro;

    public float duracaoShake;

    public float recursosQtd = 200f;

    public GameObject explosaoPrefab;

    public float[] tempMeteoroSpawn;

    public float[] tempDelayMeteoroSpawn;

    public Camera camera;

    public estagiosFase faseAtual;

    [Header("UI")]

    public Text recursosQtdText;

    public Text energiaNaveText;

    [Header("Meteoro Inativo")]

    public float vidaMeteoroMax = 100;

    public float danoLaserMeteoro = 0;

    public Color corFinalMeteoro;

    public GameObject meteoroInativoPrefab;

    public int limiteMaximoCena;

    [SerializeField] List<Vector2> posicoesMeteoroInativo = new List<Vector2>();

    [SerializeField] int limiteAtualMeteorosInativos = 0;

    [Header("Meteoro Ativo")]

    public Transform[] posicoesSpawn;

    public MeteoroAtivo meteoroAtivoPrefab;

    [SerializeField] int numMeteoroMax;

    List<GameObject> meteorosCena = new List<GameObject>();

    void Awake()
    {
        energiaNaveAtual = recursosQtd / 2;

        _player = FindAnyObjectByType(typeof(Jogador)) as Jogador;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        faseAtual = estagiosFase.exploracao;

        for (int i = 0; posicoesMeteoroInativo.Count <= limiteMaximoCena; i++)
        {
            bool isPode = false;

            while (isPode == false)
            {
                Vector2 posNova = new Vector2(Random.Range(-360, 390), Random.Range(-190, 250));

                if (posicoesMeteoroInativo.Contains(posNova))
                {
                    posNova = new Vector2(Random.Range(-360, 390), Random.Range(-190, 250));
                }
                else
                {
                    posicoesMeteoroInativo.Add(posNova);
                    isPode = true;
                }
            }

            Instantiate(meteoroInativoPrefab, posicoesMeteoroInativo[i], transform.localRotation);
        }
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

                faseAtual = estagiosFase.exploracao;
                StartCoroutine("delaySpawnMeteoros");
            }
        }
    }

    void LateUpdate()
    {
        //Mexendo camera

        if (_player != null && faseAtual == estagiosFase.exploracao)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, new Vector3(_player.transform.position.x, _player.transform.position.y, camera.transform.position.z), 0.4f);
        }
    }

    IEnumerator delaySpawnMeteoros()
    {
        yield return new WaitForSeconds(Random.Range(tempDelayMeteoroSpawn[0], tempDelayMeteoroSpawn[1]));

        faseAtual = estagiosFase.sobrevivencia;

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
