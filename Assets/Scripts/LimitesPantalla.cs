using UnityEngine;

public static class LimitesPantalla
{
    public static void CalcularLimitesPantalla(out Vector2 limitesX, out Vector2 limitesZ, float limiteXFallback = 18f, float limiteZFallback = 10f)
    {
        Camera camaraPrincipal = Camera.main;

        if (camaraPrincipal == null)
        {
            limitesX = new Vector2(-limiteXFallback, limiteXFallback);
            limitesZ = new Vector2(-limiteZFallback, limiteZFallback);
            return;
        }

        Plane planoJuego = new Plane(Vector3.up, Vector3.zero);

        Vector3 esquinaInferiorIzquierda = ObtenerPuntoPantallaEnPlano(camaraPrincipal, new Vector3(0f, 0f, 0f), planoJuego);
        Vector3 esquinaSuperiorDerecha = ObtenerPuntoPantallaEnPlano(camaraPrincipal, new Vector3(Screen.width, Screen.height, 0f), planoJuego);

        float minX = Mathf.Min(esquinaInferiorIzquierda.x, esquinaSuperiorDerecha.x);
        float maxX = Mathf.Max(esquinaInferiorIzquierda.x, esquinaSuperiorDerecha.x);
        float minZ = Mathf.Min(esquinaInferiorIzquierda.z, esquinaSuperiorDerecha.z);
        float maxZ = Mathf.Max(esquinaInferiorIzquierda.z, esquinaSuperiorDerecha.z);

        limitesX = new Vector2(minX, maxX);
        limitesZ = new Vector2(minZ, maxZ);
    }

    static Vector3 ObtenerPuntoPantallaEnPlano(Camera camaraPrincipal, Vector3 puntoPantalla, Plane planoJuego)
    {
        Ray rayo = camaraPrincipal.ScreenPointToRay(puntoPantalla);

        if (planoJuego.Raycast(rayo, out float distancia))
        {
            return rayo.GetPoint(distancia);
        }

        return Vector3.zero;
    }
}
