// Este script controla los puntos que se consiguen al superar obstáculos

using UnityEngine;

public class ScoreObstaculo : MonoBehaviour
{
    // Variable que impide que un mismo obstáculo dé puntos varias veces
    private bool yaPuntuado = false;

    // Se llama cada vez que un objeto entra en el Trigger del punto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // PARA COLISION OBSTACULOS
        // Comprobamos que sea el jugador y que este obstáculo aún no haya dado puntos
        if (collision.CompareTag("Player") && !yaPuntuado)
        {
            yaPuntuado = true; // Marcamos este obstáculo como ya puntuado
            GameManager.Instance.IncreaseScoreObstaculos();
        }

    }

    // IMPORTANTE: Cuando el Object Pooling desactiva y vuelve a activar el obstáculo, 
    // reiniciamos el estado para que pueda puntuar cuando se vuelva a cruzar.
    private void OnEnable()
    {
        yaPuntuado = false;
    }
}
