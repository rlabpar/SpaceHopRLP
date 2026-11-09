// Este script controla el portal que lleva del nivel 1 al nivel 2

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Portal : MonoBehaviour
{
    public float velocidadScroll = 4f; // La misma velocidad que los obstáculos para que avance con naturalidad
    public float velocidadGiro = 2000f;
    private bool portalActivado = false;

    // Propiedad estática para que cualquier objeto (como el fondo o los obstáculos) 
    // sepa si el portal ha atrapado al jugador y debe detenerse.
    public static bool portalEnUso = false;

    void Awake()
    {
        portalEnUso = false;
    }

    void Update()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.isGameOver) return; // Si es GameOver volvemos

        // El portal gira constantemente sobre su propio eje Z
        transform.Rotate(0, 0, velocidadGiro * Time.deltaTime);

        // Si el portal no ha sido activado, se mueve normalmente hacia la izquierda
        if (!portalActivado)
        {
            // Se mueve hacia la izquierda junto con el mundo del juego
            transform.Translate(Vector3.left * velocidadScroll * Time.deltaTime, Space.World);
                    // Space.World --> desplazamiento se realiza respecto a los ejes globales del mundo,
                    // no a los locales del objeto
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Cuando el jugadore entra en el trigger
    {
        // Si el jugador toca el portal y el portal no se ha activado antes
        if (collision.CompareTag("Player") && !portalActivado)
        {
            portalActivado = true; // Lo marcamos activado
            portalEnUso = true; // Aqui pausamos el scroll
            StartCoroutine(AnimacionEntradaPortal(collision.gameObject)); // Animacion de entrada
        }
    }

    private IEnumerator AnimacionEntradaPortal(GameObject jugador)
    {
        // Desactivamos los scripts de movimiento y físicas del jugador para que no interfieran
        // Para que el jugador no siga saltando o ejecute otros comportamientos
        MonoBehaviour[] scriptsJugador = jugador.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scriptsJugador)
        {
            if (script != this) script.enabled = false; // Desactiva script jugador
        }

        Rigidbody2D rbJugador = jugador.GetComponent<Rigidbody2D>();
        if (rbJugador != null)
        {
            rbJugador.simulated = false; // Desactiva la simulacion físicas
        }

        // Variables para la animación durante 1 segundo
        float duracion = 1f;
        float tiempoTranscurrido = 0f;

        Vector3 escalaInicial = jugador.transform.localScale;
        Vector3 posicionInicial = jugador.transform.position;
        Vector3 posicionCentroPortal = transform.position; // El centro exacto del portal

        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / duracion;

            // Actualizamos la posición del centro del portal por si este seguía un ápice en movimiento
            posicionCentroPortal = transform.position;

            // Movimiento suave hacia el centro del portal
            jugador.transform.position = Vector3.Lerp(posicionInicial, posicionCentroPortal, t);

            // Reducir tamaño progresivamente hasta 0
            jugador.transform.localScale = Vector3.Lerp(escalaInicial, Vector3.zero, t);

            // Hacer que dé vueltas sobre sí mismo (eje Z)
            jugador.transform.Rotate(0, 0, velocidadGiro * Time.deltaTime); // 720 grados por segundo

            yield return null;
        }

        // Una vez terminada la animación, cargamos el Nivel 2
        SceneManager.LoadScene("Nivel2");
    }
}
