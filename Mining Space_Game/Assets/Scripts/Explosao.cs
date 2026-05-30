using System.Collections;
using UnityEngine;

public class Explosao : MonoBehaviour
{
    SpriteRenderer sr;
    public Color corFinal;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Destroy(this.gameObject, 4f);
    }

    void Update()
    {
        StartCoroutine("delayDesparecimento");
    }

    IEnumerator delayDesparecimento()
    {
        yield return new WaitForSeconds(0.25f);

        sr.color = Color.Lerp(sr.color, corFinal, 1 * Time.deltaTime);
    }
}
