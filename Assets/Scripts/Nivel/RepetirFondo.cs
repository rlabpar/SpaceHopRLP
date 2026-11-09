// Este script hace que el fondo parezca infinito.

using UnityEngine;

public class RepetirFondo : MonoBehaviour
{
    private float spriteWidth; // 19.2

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Medimos el ancho directamente desde el SpriteRenderer, tenga o no colisionador
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x; // Se calcula el ancho del sprite
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < -spriteWidth) // Si la posicion de x ha llegado hasta el final del ancho del sprite
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        transform.Translate(new Vector3(2 * spriteWidth, 0f, 0f)); // Desplaza el fondo 2 veces su anchura en x a la derecha
    }
}
