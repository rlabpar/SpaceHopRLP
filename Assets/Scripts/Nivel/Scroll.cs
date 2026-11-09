// Este script desplaza los objetos horizontalmente hacia la izquierda.

using UnityEngine;

public class Scroll : MonoBehaviour
{
    private Rigidbody2D rb;
    public float velocidad = 4f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-velocidad, 0f); // Velocidad de la componente Rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        // Si hay Game Over o el portal ha sido activado
        if(GameManager.Instance.isGameOver || Portal.portalEnUso)
        {
            rb.linearVelocity = Vector2.zero; // No nos movemos
        }
    }
}
