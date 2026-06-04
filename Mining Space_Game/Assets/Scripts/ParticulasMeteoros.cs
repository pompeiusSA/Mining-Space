using UnityEngine;

public class ParticulasMeteoros : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip audioClip;

    GameController _gamecontroller;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        _gamecontroller = FindAnyObjectByType(typeof(GameController)) as GameController;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.PlayOneShot(_gamecontroller.sonsJogo[0]);
    }
}
