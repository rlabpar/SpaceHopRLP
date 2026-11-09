// Este script Es el encargado de gestionar el estado general
// de la partida, las puntuaciones, la derrota y la victoria.

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    public GameObject gameOverText;

    public bool isGameOver; // Para saber si la partida ha terminado
    public TMP_Text ScoreObstaculos;
    public TMP_Text ScoreDiamantes;
    public static int diamantesTotalesAcumulados = 0; // Almacena los diamantes acumulados
    public int diamantesPartidaActual = 0; // Cuenta los diamantes obtenidos durante la partida actual

    // Contenedores internos para obstaculos y diamantes
    private int scoreO;
    private int scoreD;

    private static GameManager instance;
    public static GameManager Instance {get {return instance;}}

    // Añade estas variables arriba con tus otras referencias de UI:
    public GameObject panelVictoria;        // Arrastra aquí el END desde el Canvas del Nivel 2
    public TextMeshProUGUI textoPuntuacion; // Arrastra aquí el texto de los diamantes



    void Awake() // Comprueba si ya existe un GameManager
    {
        // NO usamos DontDestroyOnLoad para evitar bloqueos al morir y recargar
        if (instance == null) // Si no existe, guarda el objeto como instancia
        {
            instance = this;
        }
        else // Si existe
        {
            Destroy(gameObject); // Destruye el duplicado
            return;
        }

        // Si estamos empezando desde el Nivel 1, borramos la memoria de los diamantes anteriores
        if (SceneManager.GetActiveScene().name == "Nivel1")
        {
            PlayerPrefs.SetInt("DiamantesAcumulados", 0);
            PlayerPrefs.Save();
            // PlayerPrefs --> Guarda datos sencillos entre escenas y ejecuciones del juego
        }

        // Cargamos los diamantes acumulados que llevamos hasta ahora
        diamantesTotalesAcumulados = PlayerPrefs.GetInt("DiamantesAcumulados", 0);

        Portal.portalEnUso = false; // Reinicia el estado del portal
    }


    void Update()
    {
        if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && isGameOver) // Si se pulsa clic derecho o espacio y es game over
        {
            RestartGame(); // Reempezamos juego
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        // Comprobamos que el texto exista en esta escena antes de intentar activarlo
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }
    }

    private void  RestartGame()
    {
        Time.timeScale = 1f; 

        // Al morir y reiniciar, restamos lo que cogimos en este intento fallido para no inflar la cuenta
        int diamantesPrevios = diamantesTotalesAcumulados - diamantesPartidaActual;
        PlayerPrefs.SetInt("DiamantesAcumulados", diamantesPrevios); // Calcula los diamantes que había antes de la partida actual
        PlayerPrefs.Save(); // Guarda esa cantidad

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarga la escena actual
    }

    // Aumenta en 1 puntuacion
    public void IncreaseScoreObstaculos()
    {
        scoreO++; // Suma un punto por superar obstáculos
        ScoreObstaculos.text = scoreO.ToString(); // Actualiza el texto
    }

    public void IncreaseScoreDiamantes()
    {
        scoreD++;
        ScoreDiamantes.text = scoreD.ToString();
    }

    // Llama a esto cada vez que cojas un diamante
    public void SumarDiamante()
    {
        // Incrementa los dos contadores
        diamantesPartidaActual++;
        diamantesTotalesAcumulados++;

        // Guardamos en tiempo real en la memoria del juego
        PlayerPrefs.SetInt("DiamantesAcumulados", diamantesTotalesAcumulados);
        PlayerPrefs.Save();
    }

    public void MostrarVictoria()
    {
        // Pausamos el juego
        Time.timeScale = 0f;

        // Activamos el panel END si está asignado
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }

        // Mostramos la puntuación total acumulada
        if (textoPuntuacion != null)
        {
            textoPuntuacion.text = diamantesTotalesAcumulados.ToString();
        }
    }
    
}
