using UnityEngine;
using UnityEngine.SceneManagement;

public class BotaoTransicao : MonoBehaviour
{
    public void saindoCena(int numeroCena)
    {
        switch (numeroCena)
        {
            case 0:

                SceneManager.LoadScene("Menu");

                break;

            case 1:

                SceneManager.LoadScene("EntreFases");

                break;

            case 2:

                SceneManager.LoadScene("Gameplay");

                break;
        }
    }
}
