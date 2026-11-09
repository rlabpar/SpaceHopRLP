// Este script hace que los obstaculos suban y bajen continuamente

using UnityEngine;

public class ObstaculosVertical : MonoBehaviour
{
    public float velocidadVertical = 0.5f;  // Velocidad de oscilación
    public float amplitud = 1f;         // Distancia máxima que sube y baja
    
    private Rigidbody2D rb;
    private float posicionYInicial; // Guarda la posicion vertical desde la que conienza

    void Start() // Guarda la posicion inicial del obstáculo
    {
        rb = GetComponent<Rigidbody2D>(); // Aisgnamos a rb la componente Rigidbody
        
        // Guardamos la posición Y con la que se spawneo
        posicionYInicial = transform.position.y;
    }

    void Update()
    {
        // Si es GameOver volvemos
        if (GameManager.Instance.isGameOver) return;

        // Calculamos el nuevo desplazamiento usando una onda senoidal
        float nuevaY = posicionYInicial + Mathf.Sin(Time.time * velocidadVertical) * amplitud;
                // Mathf.Sin --> funcion matematica llamada seno para generar movimiento oscilatorio
                    // Introduce valores entre -0.5 y 0.5
                // Lo multicamos por amplitud para controlar la disctancia que recorre el obstáculo

        // Actualizamos la posición manteniendo la X y la Z
      //  transform.position = new Vector3(transform.position.x, nuevaY, transform.position.z);
        transform.position = new Vector2(transform.position.x, nuevaY);
    }
}
