// Este script controla qué ocurre cuando el jugador recoge un diamante

using UnityEngine;

public class Diamante : MonoBehaviour
{
    private bool recogido = false; // Evita que el mismo diamante se recoja varias veces

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !recogido) // Si jugador ha entrado en Trigger y no ha recogido
        {
            recogido = true; // Marcamos que ya ha sido cogido
            // Esto es obligatorio para que sume a los totales de toda la aventura
            if (GameManager.Instance != null) // Llama al GameManager para actualizar las puntuaciones
            {
                GameManager.Instance.SumarDiamante();
                GameManager.Instance.IncreaseScoreDiamantes();
            }
                
            Destroy(gameObject); // Desaparece al cogerlo
        }
    }
}
