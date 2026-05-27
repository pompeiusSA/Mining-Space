using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Jogador : MonoBehaviour
{
    GameController _gameController;

    Rigidbody2D rbPlayer;

    float velZ, velY;

    bool isAndando;

    void Awake()
    {
        //Pegando o script do game controller

        _gameController = FindAnyObjectByType(typeof(GameController)) as GameController;

        //Pegando o Rigidbody2D do player

        rbPlayer = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Pegando o input

        velZ = Input.GetAxis("Horizontal") * -1;

        velY = Input.GetAxis("Vertical");

        isAndando = Input.GetButton("Vertical");
    }

    void FixedUpdate()
    {
        movimentacaoPlayer();
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
}
