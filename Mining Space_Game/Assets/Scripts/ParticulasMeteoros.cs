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

    void Start()
    {
        audioSource.PlayOneShot(_gamecontroller.sonsJogo[0]);
    }
}
