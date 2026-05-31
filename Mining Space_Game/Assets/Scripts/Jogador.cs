using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Jogador : MonoBehaviour
{
    GameController _gameController;

    Rigidbody2D rbPlayer;

    float velZ, velY;

    int velYInt;

    bool isAndando;

    [SerializeField] bool isAtirou;

    public bool isLaser;

    Animator animatorPlayer;

    public bool isMorreu = false;

    public Transform posAtirarPlayer;

    public Transform[] explosoesPos;

    private bool isMorreuCutsceneAtivado = false;

    public Transform[] limitesMapa;

    void Awake()
    {
        //Pegando o script do game controller

        _gameController = FindAnyObjectByType(typeof(GameController)) as GameController;

        //Pegando o Rigidbody2D do player

        rbPlayer = GetComponent<Rigidbody2D>();

        //Pegando o animator do player

        animatorPlayer = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
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
        //Ativando a animação conforme a condição do animator 

        animatorPlayer.SetInteger("velPlayer", velYInt);
    }

    private void pegandoInput()
    {
        //Pegando o input

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
        //Fazendo o player rotacionar

        if (isMorreu == false)
        {
            transform.Rotate(0, 0, velZ * _gameController.velPlayerRotacao * Time.fixedDeltaTime);
        }

        //Player se movimentar

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

    IEnumerator delayAtirar()
    {
        yield return new WaitForSeconds(_gameController.tempoDelayBala);

        isAtirou = false;
    }

    private void atirando()
    {
        if (Input.GetButton("Fire1") && isAtirou == false && isMorreu == false)
        {
            isAtirou = true;

            GameObject tempBala = Instantiate(_gameController.balaPrefabs, posAtirarPlayer.position, transform.localRotation);
            tempBala.GetComponent<Rigidbody2D>().linearVelocity = tempBala.transform.up * _gameController.velBalaPlayer;

            _gameController.energiaNaveAtual -= _gameController.energiaGasta[1];

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
    }

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
}
