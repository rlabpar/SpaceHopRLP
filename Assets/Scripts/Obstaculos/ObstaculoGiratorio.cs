// Este script hace que un obstáculo gire continuamente

using UnityEngine;

public class ObstaculoGiratorio : MonoBehaviour
{
    public float velocidadGiro = 1f; // Velocidad de rotacion

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Buscamos el Rigidbody del objeto
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver) return;

        // Rota el objeto sobre su propio eje Z (el plano 2D)
        transform.Rotate(0f, // Transform.Rotate(x,y,z) --> Rota el objeto
                         0f, // Dejamos X e Y = 0 pq en 2D solo rotamos en eje z
                         velocidadGiro * Time.deltaTime); // Time.deltaTime --> hace que la rotacion sea independiente de los FPS
    }
}
