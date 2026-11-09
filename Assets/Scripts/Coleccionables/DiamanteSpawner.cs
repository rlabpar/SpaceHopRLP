using UnityEngine;

public class DiamanteSpawner : MonoBehaviour
{
    // Definen el prefab, el tiempo entre apariciones,
    // la posición y las capas de obstáculos que queremos detectar
    public GameObject diamantePrefab;
    public float tiempoEntreSpawns = 4f; // Cada cuánto tiempo intenta salir un diamante
    public float xSpawnPosition = 14f;   // Por dónde aparece a la derecha
    public float minY = -2f;
    public float maxY = 2.5f;
    public LayerMask capaObstaculos;     // Para que sepa qué es un obstáculo y no tocarlo

    private float contadorTiempo;

    void Update()
    {
        if (GameManager.Instance.isGameOver) return;

        // Si el ObstaclesPool ya ha terminado de sacar los pinchos, no generamos más diamantes
        if (ObstaclesPool.terminadosObstaculos) return;

        // Mientras la partida continúe y todavía queden obstáculos por generar, acumula tiempo
        contadorTiempo += Time.deltaTime;

        if (contadorTiempo >= tiempoEntreSpawns) // Cada cuatro segundos llama a SpawnearDiamanteSeguro()
        {
            contadorTiempo = 0f;
            SpawnearDiamanteSeguro();
        }
    }

    void SpawnearDiamanteSeguro()
    {
        // Hacemos varios intentos para encontrar una altura libre de obstáculos
        for (int intentos = 0; intentos < 10; intentos++)
        {
            float randomY = Random.Range(minY, maxY); // Genera una posicion aleatoria
            Vector2 posicionPrueba = new Vector2(xSpawnPosition, randomY);

            // Comprobamos si hay un obstáculo en un radio de 1.2 unidades alrededor de esa posición
            Collider2D obstaculoCerca = Physics2D.OverlapCircle(posicionPrueba, 1.2f, capaObstaculos);

            // Si el resultado es nulo, significa que la zona está completamente libre
            if (obstaculoCerca == null)
            {
                Instantiate(diamantePrefab,
                            posicionPrueba,
                            Quaternion.identity); // Genera el diamante
                break; // Salimos del bucle porque ya lo hemos colocado con éxito
            }
        }
    }
}
