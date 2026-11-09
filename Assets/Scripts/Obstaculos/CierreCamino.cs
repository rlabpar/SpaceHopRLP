using UnityEngine;
using System.Collections;

public class CierreCamino : MonoBehaviour
{
    public float posicionInicialY = 6f;    // Empieza arriba en Y = 4
    public float posicionFinalY = -3f;     // Posición a la que bajará en Y
    public float duracionAnimacion = 6f;   // Lo que tarda en bajar (7 segundos)

    private bool jugadorPasando = false;

    void Start()
    {
        // Al aparecer el prefab en pantalla, nos aseguramos de colocarlo exactamente en Y = 4 (manteniendo su X y Z)
        transform.position = new Vector2(transform.position.x, posicionInicialY);
    }

    // Se activa cuando el jugador cruza el sensor de la entrada
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !jugadorPasando)
        {
            jugadorPasando = true;
            StartCoroutine(CerrarCaminoAnimacion());
        }
    }

    // Se activa si el jugador choca contra la piedra física (mortal)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    private IEnumerator CerrarCaminoAnimacion()
    {
        float tiempo = 0f;
        float posYInicioBajada = transform.position.y;

        while (tiempo < duracionAnimacion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionAnimacion;

            // Interpolamos suavemente hacia Y = -3 mientras se mueve en horizontal con el scroll
            float nuevaY = Mathf.Lerp(posYInicioBajada, posicionFinalY, t);
            transform.position = new Vector2(transform.position.x, nuevaY);

            yield return null;
        }

        // Aseguramos la posición final exacta en Y
        transform.position = new Vector2(transform.position.x, posicionFinalY);
    }
}