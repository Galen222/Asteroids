using UnityEngine;

public class ComprobacionLimites : MonoBehaviour
{
    public float limiteZ = 10;
    public float limiteX = 18;
    public float margen = 0.5f;

    public TrailRenderer MotorDerecho;
    public TrailRenderer MotorIzquierdo;

    void Start()
    {
        ComprobarRastreo();
    }

    void Update()
    {
        Vector2 limitesX;
        Vector2 limitesZ;
        LimitesPantalla.CalcularLimitesPantalla(out limitesX, out limitesZ, limiteX, limiteZ);

        Vector3 posicion = transform.position;
        bool reposicionado = false;

        if (posicion.z > limitesZ.y + margen)
        {
            posicion.z = limitesZ.x - margen;
            reposicionado = true;
        }
        else if (posicion.z < limitesZ.x - margen)
        {
            posicion.z = limitesZ.y + margen;
            reposicionado = true;
        }

        if (posicion.x > limitesX.y + margen)
        {
            posicion.x = limitesX.x - margen;
            reposicionado = true;
        }
        else if (posicion.x < limitesX.x - margen)
        {
            posicion.x = limitesX.y + margen;
            reposicionado = true;
        }

        if (reposicionado)
        {
            posicion.y = 0f;
            transform.position = posicion;
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
