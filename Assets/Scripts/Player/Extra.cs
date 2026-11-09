// Este script controla toda la interacción básica del jugador, incluyendo su movimiento vertical, animaciones y detección de colisiones

using UnityEngine;

public class Salto : MonoBehaviour
{
    private Rigidbody2D rb; // Permite controlar la fisica del personaje, como su velocidad y saltos
    private Animator an; // Permite activar las animaciones del personaje

    public float fuerzasalto = 350f; // fuerza con la que salta el personaje

    private bool muerto = false; // Indica si el personaje ha muerto

    // Variables para controlar la inclinación (rotación)
    public float inclinacionSubida = 10f;
    public float inclinacionBajada = -20f;
    public float velocidadRotacion = 5f;

    void Start() // Se ejecuta al comenzar el juego
    {
        // GetComponent --> Busca un componente que esté en el mismo GameObject que el script
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
    }

    void Update() // Se ejecuta continuamente mientras el juego está funcionando
    {
        // Si (se pulsa boton derecho o se pulsa espacio) y muerto=false (no esta muerto)
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !muerto)
        {
            Aleteo();
        }

        // Si no está muerto, aplicamos la rotación suave según si sube o baja
        if (!muerto)
        {
            RotarPersonaje();
        }
    }

    private void Aleteo()
    {
        rb.linearVelocity = Vector2.zero; // Pone la velocidad a cero
        rb.AddForce(Vector2.up * fuerzasalto); // Aplica una fueza hacia arriba
                                               // Vector2.up --> Equivale a una direccion vertical positiva (0, 1)
        an.SetTrigger("Aleteo"); // Activa animación altero
    }

    // Inclinamos personaje si subimos o bajamos
    private void RotarPersonaje()
    {
        float anguloDestino = 0f;

        // Si está subiendo (velocidad Y positiva)
        if (rb.linearVelocity.y > 0)
        {
            anguloDestino = inclinacionSubida; // Inclinamos 10º
        }
        // Si está bajando (velocidad Y negativa o cero)
        else
        {
            anguloDestino = inclinacionBajada; // Inclinamos -20º
        }

        // Aplicamos una rotación suave (Lerp) hacia el ángulo objetivo en el eje Z
        Quaternion rotacionObjetivo = Quaternion.Euler(0, 0, anguloDestino);
                // Quartenion.Euler --> Converite los angulos en una rotación
        transform.rotation = Quaternion.Lerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
                // Quaternion.Lerp --> Realiza una transición suave entre la rotación actual y la deseada
                // Time.deltaTIme --> Permite que la velocidad de rotacion sea independiente de los FPS
    }

    private void OnCollisionEnter2D() // Lo ejecutamos cuando el personaje choca fisicamente con otro collider
    {
        if (muerto) return; // Si estaba muerto volvemos para evitar que se ejecute varias veces si choca con varias cosas

        muerto = true; // Lo cambiamos a muerto
        an.SetTrigger("Muerte"); // Ponemos la animacion de muerte
        GameManager.Instance.GameOver(); // Llamamos a GameManager para terminar la partida

        // Frenamos el movimiento horizontal para que caiga recto hacia abajo
        rb.linearVelocity = Vector2.zero;

        // Congelamos la rotación física para que las físicas de Unity no lo hagan girar al golpear
        rb.freezeRotation = true;

        //  Al morir se queda en un ángulo fijo
        transform.rotation = Quaternion.Euler(0, 0, -90f);
    }
}