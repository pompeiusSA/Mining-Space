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

    public Transform[] limitesCamPlayer;

    [Header("Gameplay configs")]

    public LayerMask layerMeteoro;

    public float duracaoShake;

    public float recursosQtd = 200f;

    public GameObject explosaoPrefab;

    public float[] tempMeteoroSpawn;

    public float[] tempDelayMeteoroSpawn;

    public Camera camera;

    public Transform[] posicoesSpawnPlayer;

    public CameraScr _camera;

    public bool isDano = false;

    public estagiosFase faseAtual;

    public GameObject meteoroFinal;

    [Header("UI")]

    public Text recursosQtdText;

    public Text energiaNaveText;

    public Material materialCam;

    public Color corDanoCamera;

    public Color corCamera;

    public GameObject particulasMeteoro;
    public GameObject particulasMeteoroInativo;
    public GameObject particulasMeteoroFinal;

    [Header("Meteoro Inativo")]

    public float vidaMeteoroMax = 100;

    public float danoLaserMeteoro = 0;

    public Color corFinalMeteoro;

    public GameObject meteoroInativoPrefab;

    public int limiteMaximoCena;

    [SerializeField] List<Vector2> posicoesMeteoroInativo = new List<Vector2>();

    [SerializeField] List<Vector2> possiveisPosicoesMeteoroFinal = new List<Vector2>();

    [SerializeField] int limiteAtualMeteorosInativos = 0;

    [Header("Meteoro Ativo")]

    public Transform[] posicoesSpawn;

    public MeteoroAtivo meteoroAtivoPrefab;

    [SerializeField] int numMeteoroMax;

    List<GameObject> meteorosCena = new List<GameObject>();

    void Awake()
    {
        _player = FindAnyObjectByType(typeof(Jogador)) as Jogador;

        _camera = FindAnyObjectByType(typeof(CameraScr)) as CameraScr;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        faseAtual = estagiosFase.exploracao;

        energiaNaveAtual = recursosQtd / 2;

        instanciandoObjetos();
    }

    // Update is called once per frame
    void Update()
    {
        recursosQtdText.text = ((int)recursosQtd).ToString();

        energiaNaveText.text = ((int)energiaNaveAtual + "%").ToString();

        if (ExisteMeteoroNaCena() == false)
        {
            if (numMeteoroMax <= 0)
            {
                numMeteoroMax = Random.Range(10, 50);

                faseAtual = estagiosFase.exploracao;
                StartCoroutine("delaySpawnMeteoros");
            }
        }

        if (faseAtual == estagiosFase.sobrevivencia)
        {
            if (_player.transform.position.x <= limitesCamPlayer[0].position.x)
            {
                _player.transform.position = new Vector2(limitesCamPlayer[0].position.x, _player.transform.position.y);
            }
            else if (_player.transform.position.x >= limitesCamPlayer[1].position.x)
            {
                _player.transform.position = new Vector2(limitesCamPlayer[1].position.x, _player.transform.position.y);
            }
            else if (_player.transform.position.y >= limitesCamPlayer[2].position.y)
            {
                _player.transform.position = new Vector2(_player.transform.position.x, limitesCamPlayer[2].position.y);
            }
            else if (_player.transform.position.y <= limitesCamPlayer[3].position.y)
            {
                _player.transform.position = new Vector2(_player.transform.position.x, limitesCamPlayer[3].position.y);
            }
        }
        else
        {

        }
    }

    void LateUpdate()
    {
        //Mexendo camera

        if (_player != null && faseAtual == estagiosFase.exploracao && _camera.isShakeMeteoro == false)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, new Vector3(_player.transform.position.x, _player.transform.position.y, camera.transform.position.z), 0.4f);
        }
    }

    void instanciandoObjetos()
    {
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

        //Sorteando local de spawn do player

        Transform posicaoInicialPlayer = posicoesSpawnPlayer[Random.Range(0, posicoesSpawnPlayer.Length)];

        _player.transform.position = posicaoInicialPlayer.position;

        _camera.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, _camera.transform.position.z);

        //Instanciando o meteoro final

        for (int i = 0; possiveisPosicoesMeteoroFinal.Count < limiteMaximoCena; i++)
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
                    possiveisPosicoesMeteoroFinal.Add(posNova);
                    isPode = true;
                }
            }
        }

        //Quando a ultima posição foi criada, vamos randomizar a posição escolhida

        if (possiveisPosicoesMeteoroFinal.Count >= limiteMaximoCena)
        {
            int indiceEscolhido = Random.Range(0, possiveisPosicoesMeteoroFinal.Count);

            Vector2 posEscolhida = possiveisPosicoesMeteoroFinal[indiceEscolhido];

            float distPlayer = Vector2.Distance(_player.transform.position, posEscolhida);

            bool isPode = false;

            while (isPode == false)
            {
                if (distPlayer < 400)
                {
                    indiceEscolhido = Random.Range(0, possiveisPosicoesMeteoroFinal.Count);
                    posEscolhida = possiveisPosicoesMeteoroFinal[indiceEscolhido];
                    distPlayer = Vector2.Distance(_player.transform.position, posEscolhida);
                }
                else
                {
                    Instantiate(meteoroFinal, posEscolhida, transform.localRotation);

                    isPode = true;
                }
            }
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
