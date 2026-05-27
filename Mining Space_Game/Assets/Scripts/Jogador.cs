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

    public Transform posAtirarPlayer;

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

        if (Input.GetButton("Fire1") && isAtirou == false)
        {
            isAtirou = true;

            GameObject tempBala = Instantiate(_gameController.balaPrefabs, posAtirarPlayer.position, transform.localRotation);
            tempBala.GetComponent<Rigidbody2D>().linearVelocity = tempBala.transform.up * _gameController.velBalaPlayer;

            StartCoroutine("delayAtirar");
        }

        if (Input.GetButton("Fire2"))
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

        transform.Rotate(0, 0, velZ * _gameController.velPlayerRotacao * Time.fixedDeltaTime);

        //Player se movimentar

        if (isAndando)
        {
            rbPlayer.linearVelocity = transform.up * velY * _gameController.velPlayer;
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
}
