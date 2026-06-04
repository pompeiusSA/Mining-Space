using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dialogo : MonoBehaviour
{
    public Text textoDialogo;

    [SerializeField] int index;

    public string[] linhas;

    public float velTexto;

    [Header("Cena após o diálogo")]

    [SerializeField] string cenaAtual;

    void Start()
    {
        textoDialogo.text = string.Empty;

        StartCoroutine("dialogoComeca");

        cenaAtual = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (textoDialogo.text == linhas[index])
            {
                proximaFala();
            }
        }
    }

    void dialogoComeca()
    {
        index = 0;

        StartCoroutine(dialogoDigitando());
    }

    IEnumerator dialogoDigitando()
    {
        foreach (char item in linhas[index].ToCharArray())
        {
            textoDialogo.text += item;

            yield return new WaitForSeconds(velTexto);
        }
    }

    void proximaFala()
    {
        if (index < linhas.Length - 1)
        {
            index++;
            textoDialogo.text = string.Empty;
            StartCoroutine(dialogoDigitando());
        }
        else
        {
            switch (cenaAtual)
            {
                case "EntreFases":

                    SceneManager.LoadScene("Gameplay");

                    break;

                case "FimDoJogo":

                    SceneManager.LoadScene("Menu");

                    break;
            }
        }
    }
}