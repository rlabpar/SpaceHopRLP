// Este script sirve para impedir que un objeto rote.

using UnityEngine;

public class NoRotar : MonoBehaviour
{
    void LateUpdate() // Se ejecuta despues de Update
    {
        // Fuerza a que la rotación de este hijo sea siempre cero (apuntando recto), 
        // ignorando los giros del objeto padre.
        transform.rotation = Quaternion.identity;
                // transform.rotation --> Establece rotacion del objeto
                // Quaternion.identity --> Representa la rotacion neutra: 0 grados en todos los ejes
    }
}