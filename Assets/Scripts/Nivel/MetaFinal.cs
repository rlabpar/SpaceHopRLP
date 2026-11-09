// Es parecido al portal, pero tiene una función distinta: terminar el nivel 2

using UnityEngine;
using System.Collections;

public class MetaFinal : MonoBehaviour
{
    public float velocidadScroll = 4f; 
    public float velocidadGiro = 2000f; // Giro rápido
    private bool metaActivada = false;

    void Update()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.isGameOver) return; // Si es GameOver volvemos

        // Si el portal no ha sido activado, se mueve normalmente hacia la izquierda
        if (!metaActivada)
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
        if (collision.CompareTag("Player") && !metaActivada)
        {
            metaActivada = true; // Lo marcamos activado
            Portal.portalEnUso = true; // Aqui pausamos el scroll
            StartCoroutine(AnimacionEntradaMeta(collision.gameObject));
        }
    }

    private IEnumerator AnimacionEntradaMeta(GameObject jugador)
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

        // Calculamos el punto final: el extremo derecho del BoxCollider2D de la Meta
        BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
        Vector3 posicionFinal;

        if (boxCol != null)
        {
            float bordeDerechoX = transform.position.x + (boxCol.size.x * 0.5f * transform.localScale.x);
            posicionFinal = new Vector3(bordeDerechoX, transform.position.y, transform.position.z);
        }
        else
        {
            posicionFinal = transform.position + new Vector3(2f, 0f, 0f);
        }

        // Variables para la animación durante 1 segundo
        float duracion = 1f;
        float tiempoTranscurrido = 0f;

        Vector3 posicionInicial = jugador.transform.position;
        Vector3 escalaInicial = jugador.transform.localScale;

        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / duracion;

            // Movemos a Extra hacia el borde final del collider
            jugador.transform.position = Vector3.Lerp(posicionInicial, posicionFinal, t);

            // Reducir tamaño progresivamente hasta 0
            jugador.transform.localScale = Vector3.Lerp(escalaInicial, Vector3.zero, t);

            // Hacer que dé vueltas sobre sí mismo (eje Z)
            jugador.transform.Rotate(0, 0, velocidadGiro * Time.deltaTime);

            yield return null;
        }

        // Mostramos la victoria
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MostrarVictoria();
        }
    }
}
