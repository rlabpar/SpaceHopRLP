// Este script hace que algunos diamantes se muevan verticalmente

using UnityEngine;

public class MovimientoDiamante : MonoBehaviour
{
    private bool esMovil = false;
    private float frecuencia = 3f;  // Velocidad del ciclo de subida y bajada
    private float amplitud = 1.2f;   // Cuánto sube y baja respecto a su posición inicial
    private Vector3 posicionInicial;

    // Al nacer (start): Cada diamante decide por si mismo mediante una tirada aleatoria (random.value)
    // si se queda quieto o se convierte en un diamante flotante con movimiento vertical.
    // Ademas, varia ligeranmente su velocidad y altura para que cada uno sea unico
    void Start()
    {
        posicionInicial = transform.position; // Guarda su posicion inicial

        // Decidimos aleatoriamente: 50% de probabilidad de que sea estático, 50% de que se mueva
        esMovil = Random.value > 0.5f;
    }

    // En el bucle (Update): Si es movil, utiliza una onda sinusoidal (Mathf.Sin) para subir
    // y bajar de forma fluida y organica alrededor de la altura en la que aparecio
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        // Si le tocó ser móvil, oscila suavemente en el eje Y usando una función seno
        if (esMovil)
        {
            float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * frecuencia) * amplitud;
            transform.position = new Vector2(transform.position.x, nuevaY);
        }
    }
}
