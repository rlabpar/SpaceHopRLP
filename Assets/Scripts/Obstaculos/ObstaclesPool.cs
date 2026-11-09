// Este script se encarga de crear y gestionar los obstáculos de cada nivel,
// además de controlar cuándo aparecen el portal, el camino y la meta

using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para leer la escena actual
using System.Collections;

public class ObstaclesPool : MonoBehaviour
{
    // CONFIGURACION DE PREFABS
    public GameObject[] obstaculoPrefabs; // Array con los distintos prefabs de obstáculos
    public int poolSize = 12; // Numeros de obstaculos que se crearán inicialmente
    
    // CONFIGURACION DE SPAWN
    public float spawnTime = 2.5f;
    public float xSpawnPosition = 14f;
    public float minYPosition = -2f;
    public float maxYPosition = 1f;

    public float SpawnObs = 0.1f; // Tiempo primer spawn obstaculos

    // LIMITE DE LA PARTIDA
    public int maxObstaculos = 12; // Limite exacto de obstaculos
    private int totalSpawned = 0; // Controlador de cuantos llevamos generados

    private float timeElapsed;
    private int obstaculoCount;
    private GameObject[] obstaculos; // Array donde guardamos los objetos que hemos creado para reutilizarlos

    // CONFIGURACION PORTAL Y META
    public GameObject portalPrefab; // El portal del Nivel 1
    public GameObject metaFinalPrefab; // Arrastra aquí la MetaFinal del Nivel 2 (puede estar deshabilitada en escena)
    public GameObject caminoPrefab; // Arrastra aquí tu prefab de Camino desde el Inspector

    private bool portalCreado = false;
    public static bool terminadosObstaculos = false;
    
    void Start() // Al empezar
    {
        terminadosObstaculos = false; // Reiniciamos la variable
        obstaculos = new GameObject[poolSize]; // Array para guardar los obstaculos

        for (int i = 0; i < poolSize; i++) // Creamos un for que se ejecuta tantas veces como indica poolSize
        {
            // Selecciona un prefab al azar del array para rellenar el pool con variedad
           GameObject prefabAleatorio = obstaculoPrefabs[Random.Range(0, obstaculoPrefabs.Length)];
                    // obstaculoPrefabs.Length --> longitud de obstaculoPrefabs
            
            obstaculos[i] = Instantiate(prefabAleatorio); // Instantiate() crea una copia del prefab
            obstaculos[i].SetActive(false); // Desactiva el objeto hasta que tenga que aparecer

            // Decimos en que momento empiezan a aparecer los obstaculos
            timeElapsed = spawnTime - SpawnObs;
        }
    }


    void Update()
    {
        timeElapsed += Time.deltaTime; // Va acumulando el tiempo transcurrido

        if (timeElapsed > spawnTime && // Si tiempo transcurrido > tiempo de spawn
            !GameManager.Instance.isGameOver) // Y el juego no esta en gameover
        {
            SpawnObstacle(); // Se generan obstaculos
        }
    }

    private void SpawnObstacle()
    {
        // Si ya hemos llegado al límite de los obstáculos
        if (totalSpawned >= maxObstaculos)
        {
            terminadosObstaculos = true; // No generamos mas obstaculos

            // Si hemos llegado al límite y aún no hemos llamado al portal, lo activamos
            if (!portalCreado)
            {
                portalCreado = true;
                StartCoroutine(EsperarYMostrarPortal());
            }
            return; // Salimos del metodo
        }

        timeElapsed = 0f;

        // Calculamos la nueva posicion aleatoria en Y
        float ySpawnPosition = Random.Range(minYPosition, maxYPosition);
        Vector2 spawnPosition = new Vector2(xSpawnPosition, ySpawnPosition); // Prepara la posicion
        
        // Si el objeto en esta posicion del pool sigue activo en pantalla, lo saltamos o lo forzamos
        // (Asegurate de que poolSize sea lo suficientemente grande para que no se reciclen antes de tiempo)
        if (obstaculos[obstaculoCount].activeSelf) // Comprobar si la estancia esta activa
        {
            obstaculos[obstaculoCount].SetActive(false); // Si estaba activo lo desactiva
        }

        // Posicionamos y activamos el obstaculo
        obstaculos[obstaculoCount].transform.position = spawnPosition;
        obstaculos[obstaculoCount].SetActive(true);

        totalSpawned++; // Sumamos 1 al contador total de generados

        // Avanzamos al siguiente indice del pool
        obstaculoCount++;

        if(obstaculoCount == poolSize)
        {
            obstaculoCount = 0; // Empiezo otra vez
        }
    }

    private System.Collections.IEnumerator EsperarYMostrarPortal()
    // IEnumerator --> Corrutina que permite realizar acciones separadas por intervalos de
    // tiempo sin bloquear todo el juego
    // Una corrutina permite realizar acciones separadas por intervalos de tiempo sin bloquear todo el juego
    {
        // Esperamos 2 segundos tal como pediste
        yield return new WaitForSeconds(2f);

        // Comprobamos en qué escena estamos exactamente
        string escenaActual = SceneManager.GetActiveScene().name;
        Vector2 posicionSpawn = new Vector2(10f, 0f);

        if (escenaActual == "Nivel2") // Si estamos en el Nivel 2
        {
            // 1. Primero instanciamos el Camino trampa un poco más a la derecha o justo a tiempo
            if (caminoPrefab != null) // Si camino no es nulo
            {
                Instantiate(caminoPrefab, posicionSpawn, Quaternion.identity);
            }

            // 2. Esperamos un par de segundos mientras el camino avanza con el scroll para luego soltar la MetaFinal
            yield return new WaitForSeconds(3f);
            
            // Si estamos en el Nivel 2, instanciamos el Prefab de la MetaFinal
            if (metaFinalPrefab != null) // Si metaFinalPrefab no es nula
            {
                Instantiate(metaFinalPrefab, posicionSpawn, Quaternion.identity);
            }
        }
        else // Si estamos en el Nivel 1, iniciamos portal normal
        {
            if (portalPrefab != null) // Si portal no es nulo
            {
                Instantiate(portalPrefab, posicionSpawn, Quaternion.identity);
            }
        }
    }

    // Devuelve true si ya se han generado todos los obstáculos permitidos
    public bool HaTerminadoObstaculos()
    {
        return totalSpawned >= maxObstaculos;
    }
}