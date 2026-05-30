using UnityEngine;

public class ComprobacionLimites : MonoBehaviour
{
    public float limiteZ = 10;
    public float limiteX = 18;

    public TrailRenderer MotorDerecho;
    public TrailRenderer MotorIzquierdo;
    void Start()
    {
        
    }

    void Update()
    {
        
        if (transform.position.z > limiteZ)
        {
            transform.position = new Vector3(transform.position.x, 0, -limiteZ);
            ComprobarRastreo();
        }

        if (transform.position.z < -limiteZ)
        {
            transform.position = new Vector3(transform.position.x, 0, limiteZ);
            ComprobarRastreo();
        }

        if (transform.position.x > limiteX)
        {
            transform.position = new Vector3(-limiteX, 0, transform.position.z);
            ComprobarRastreo();
        }

        if (transform.position.x < -limiteX)
        {
            transform.position = new Vector3(limiteX, 0, transform.position.z);
            ComprobarRastreo();            
        }
    }

    void ComprobarRastreo()
    {
        if (MotorDerecho != null)
        {
            MotorDerecho.Clear();
        }
        if (MotorIzquierdo != null)
        {
            MotorIzquierdo.Clear();
        }
    }
}
