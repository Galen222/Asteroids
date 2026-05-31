using UnityEngine;

public class TrailMotoresMenu : MonoBehaviour
{
    public TrailRenderer motorDerecho;
    public TrailRenderer motorIzquierdo;

    public float distanciaMovimiento = 1.5f;
    public float velocidadMovimiento = 5f;
    public float tiempoTrail = 1.2f;
    public float multiplicadorAnchura = 1.6f;

    Vector3 posicionDerecha;
    Vector3 posicionIzquierda;
    float anchuraOriginalDerecha;
    float anchuraOriginalIzquierda;

    void Start()
    {
        if (motorDerecho != null)
        {
            posicionDerecha = motorDerecho.transform.localPosition;
            anchuraOriginalDerecha = motorDerecho.widthMultiplier;
            PrepararTrail(motorDerecho, anchuraOriginalDerecha);
        }

        if (motorIzquierdo != null)
        {
            posicionIzquierda = motorIzquierdo.transform.localPosition;
            anchuraOriginalIzquierda = motorIzquierdo.widthMultiplier;
            PrepararTrail(motorIzquierdo, anchuraOriginalIzquierda);
        }
    }

    void Update()
    {
        float desplazamiento = Mathf.PingPong(Time.time * velocidadMovimiento, distanciaMovimiento);
        Vector3 offset = Vector3.back * desplazamiento;

        if (motorDerecho != null)
        {
            motorDerecho.transform.localPosition = posicionDerecha + offset;
        }

        if (motorIzquierdo != null)
        {
            motorIzquierdo.transform.localPosition = posicionIzquierda + offset;
        }
    }

    void PrepararTrail(TrailRenderer trail, float anchuraOriginal)
    {
        trail.time = tiempoTrail;
        trail.widthMultiplier = anchuraOriginal * multiplicadorAnchura;
        trail.emitting = true;
        trail.Clear();
    }
}
