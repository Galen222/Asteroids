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

        Vector3 direccionNormalizada = direccion.normalized;
        transform.forward = direccionNormalizada;
        rb.linearVelocity = direccionNormalizada * velocidad;
    }
}
