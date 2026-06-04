using System.Collections;
using GLTFast.Schema;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Jogador : MonoBehaviour
{
    // Referencias principais.
    GameController _gameController;

    Rigidbody2D rbPlayer;

    Animator animatorPlayer;

    AudioSource audioPlayer;

    AudioClip clipeAudio;

    // Movimento e estado do jogador.
    float velZ, velY;

    int velYInt;

    bool isAndando;

    [SerializeField] bool isAtirou;

    public bool isLaser;

    public bool isMorreu = false;

    private bool isMorreuCutsceneAtivado = false;

    // Pontos e limites usados durante o jogo.
    public Transform posAtirarPlayer;

    public Transform[] explosoesPos;

    public Transform[] limitesMapa;

    void Awake()
    {
        // Guarda as referencias usadas durante a partida.
        _gameController = FindAnyObjectByType(typeof(GameController)) as GameController;

        rbPlayer = GetComponent<Rigidbody2D>();

        animatorPlayer = GetComponent<Animator>();

        audioPlayer = GetComponent<AudioSource>();
    }

    void Start()
    {

    }

    void Update()
    {
        pegandoInput();

        atirando();

        playerMorte();

        limiteMapaPlayer();
    }

    void FixedUpdate()
    {
        movimentacaoPlayer();
    }

    void LateUpdate()
    {
        // Atualiza a animacao conforme o movimento vertical.
        animatorPlayer.SetInteger("velPlayer", velYInt);
    }

    private void pegandoInput()
    {
        // Le os comandos de movimento do jogador.
        velZ = Input.GetAxis("Horizontal") * -1;

        velY = Input.GetAxis("Vertical");

        isAndando = Input.GetButton("Vertical");

        if (velY > 0)
        {
            velYInt = 1;
        }
        else
        {
            velYInt = 0;
        }
    }

    private void movimentacaoPlayer()
    {
        // Rotaciona enquanto o jogador estiver vivo.
        if (isMorreu == false)
        {
            transform.Rotate(0, 0, velZ * _gameController.velPlayerRotacao * Time.fixedDeltaTime);
        }

        // Move a nave ou aplica freio gradual.
        if (isAndando && isMorreu == false)
        {
            rbPlayer.linearVelocity = transform.up * velY * _gameController.velPlayer;

            _gameController.energiaNaveAtual -= _gameController.energiaGasta[0] * Time.fixedDeltaTime;
        }
        else
        {
            float freioX = Mathf.Lerp(rbPlayer.linearVelocityX, 0, _gameController.delayRePlayer);
            float freioY = Mathf.Lerp(rbPlayer.linearVelocityY, 0, _gameController.delayRePlayer);

            rbPlayer.linearVelocity = new Vector3(freioX, freioY, 0f);
        }
    }

    private void atirando()
    {
        if (Input.GetButton("Fire1") && isAtirou == false && isMorreu == false)
        {
            isAtirou = true;

            GameObject tempBala = Instantiate(_gameController.balaPrefabs, posAtirarPlayer.position, transform.localRotation);
            tempBala.GetComponent<Rigidbody2D>().linearVelocity = tempBala.transform.up * _gameController.velBalaPlayer;

            _gameController.energiaNaveAtual -= _gameController.energiaGasta[1];

            clipeAudio = _gameController.sonsJogo[2];

            audioPlayer.PlayOneShot(clipeAudio);

            StartCoroutine("delayAtirar");
        }

        if (Input.GetButton("Fire2") && isMorreu == false)
        {
            _gameController.laserObject.GetComponent<SpriteRenderer>().enabled = true;
            isLaser = true;
        }
        else
        {
            _gameController.laserObject.GetComponent<SpriteRenderer>().enabled = false;
            isLaser = false;
        }
    }

    // Verifica se a nave deve entrar na sequencia de morte.
    public void playerMorte()
    {
        if (_gameController.energiaNaveAtual <= 0 || _gameController.vidaNave <= 0)
        {
            isMorreu = true;
        }

        if (isMorreu && isMorreuCutsceneAtivado == false)
        {
            isMorreuCutsceneAtivado = true;
            StartCoroutine("playerMorrendoCutscene");
        }
    }

    // Mantem o jogador dentro dos limites do mapa.
    void limiteMapaPlayer()
    {
        if (transform.position.x >= limitesMapa[0].position.x)
        {
            transform.position = new Vector2(limitesMapa[0].position.x, transform.position.y);
        }
        else if (transform.position.x <= limitesMapa[1].position.x)
        {
            transform.position = new Vector2(limitesMapa[1].position.x, transform.position.y);
        }
        else if (transform.position.y <= limitesMapa[2].position.y)
        {
            transform.position = new Vector2(transform.position.x, limitesMapa[2].position.y);
        }
        else if (transform.position.y >= limitesMapa[3].position.y)
        {
            transform.position = new Vector2(transform.position.x, limitesMapa[3].position.y);
        }
    }

    IEnumerator delayAtirar()
    {
        yield return new WaitForSeconds(_gameController.tempoDelayBala);

        isAtirou = false;
    }

    // Executa as explosoes e troca para a cena de Game Over.
    IEnumerator playerMorrendoCutscene()
    {
        yield return new WaitForSeconds(0.5f);

        Instantiate(_gameController.explosaoPrefab, explosoesPos[0].transform.position, transform.localRotation);
        Instantiate(_gameController.explosaoPrefab, explosoesPos[2].transform.position, transform.localRotation);

        yield return new WaitForSeconds(0.5f);

        Instantiate(_gameController.explosaoPrefab, explosoesPos[1].transform.position, transform.localRotation);

        yield return new WaitForSeconds(0.5f);

        Instantiate(_gameController.explosaoPrefab, explosoesPos[2].transform.position, transform.localRotation);
        Instantiate(_gameController.explosaoPrefab, explosoesPos[1].transform.position, transform.localRotation);

        yield return new WaitForSeconds(0.5f);

        Instantiate(_gameController.explosaoPrefab, explosoesPos[1].transform.position, transform.localRotation);
        Instantiate(_gameController.explosaoPrefab, explosoesPos[0].transform.position, transform.localRotation);
        Instantiate(_gameController.explosaoPrefab, explosoesPos[2].transform.position, transform.localRotation);

        this.GetComponent<SpriteRenderer>().enabled = false;

        Instantiate(_gameController.explosaoPrefab, transform.position, transform.localRotation);

        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);

        _gameController.materialCam.color = _gameController.corCamera;

        SceneManager.LoadScene("GameOver");
    }
}
