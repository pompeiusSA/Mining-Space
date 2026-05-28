using Unity.VisualScripting;
using UnityEngine;

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

    [Header("Meteoro Inativo")]

    public float vidaMeteoro;

    public float danoLaserMeteoro = 0;

    public Color corFinalMeteoro;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
