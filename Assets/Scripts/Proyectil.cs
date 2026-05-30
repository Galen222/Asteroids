using UnityEngine;

public class Proyectil : MonoBehaviour
{

    Rigidbody rb;

    public float velocidad = 10;
    public float tiempoVidaProyectil = 1;

    public void Start()
    {
        Destroy(gameObject, tiempoVidaProyectil);
    }

    public void Impulso(Vector3 direccion)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(direccion * velocidad, ForceMode.Impulse);
    }

}
