using UnityEngine;

public class MeteoroInativo : MonoBehaviour
{
    GameController _gameController;

    void Awake()
    {
        _gameController = FindAnyObjectByType(typeof(GameController)) as GameController;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_gameController.vidaMeteoro <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
