using Unity.VisualScripting;
using UnityEngine;

public class Menus : MonoBehaviour
{
    MeshRenderer meshrender;

    Material materialUsado;

    [SerializeField] float incremento;

    float valorInicial = 0;

    void Awake()
    {
        meshrender = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        materialUsado = meshrender.material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        valorInicial += incremento * Time.fixedDeltaTime;

        materialUsado.SetTextureOffset("_MainTex", new Vector2(valorInicial, transform.position.y));
    }
}
