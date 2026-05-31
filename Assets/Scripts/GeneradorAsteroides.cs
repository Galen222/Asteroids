using UnityEngine;

public class GeneradorAsteroides : MonoBehaviour
{
    public GameObject prefabAsteroideNivel1;
    public GameObject prefabAsteroideNivel2;
    public GameObject prefabAsteroideNivel3;

    public int cantidadAsteroides = 10;

    public float limiteX = 3;
    public float limiteZ = 3;
    public float margenPantalla = 1f;

    public int asteroidesGenerados = 0;

    public int intentosMaximosPorAsteroide = 100;

    public void GenerarAsteroides(int cantidad, int nivel)
    {
        cantidadAsteroides = cantidad;
        asteroidesGenerados = 0;

        GameObject prefab = ObtenerPrefabNivel(nivel);
        if (prefab == null)
        {
            return;
        }

        Vector2 limitesX;
        Vector2 limitesZ;
        LimitesPantalla.CalcularLimitesPantalla(out limitesX, out limitesZ, 20f, 10f);

        for (int i = 0; i < cantidadAsteroides; i++)
        {
            Vector3 posicionInstancia = ObtenerPosicionAleatoria(limitesX, limitesZ);
            Instantiate(prefab, posicionInstancia, Random.rotation);
            asteroidesGenerados++;
        }
    }

    Vector3 ObtenerPosicionAleatoria(Vector2 limitesX, Vector2 limitesZ)
    {
        for (int intento = 0; intento < intentosMaximosPorAsteroide; intento++)
        {
            float posicionX = Random.Range(limitesX.x + margenPantalla, limitesX.y - margenPantalla);
            float posicionZ = Random.Range(limitesZ.x + margenPantalla, limitesZ.y - margenPantalla);

            if (!EstaEnZonaCentral(posicionX, posicionZ))
            {
                return new Vector3(posicionX, 0f, posicionZ);
            }
        }

        return ObtenerPosicionAleatoriaEnBorde(limitesX, limitesZ);
    }

    Vector3 ObtenerPosicionAleatoriaEnBorde(Vector2 limitesX, Vector2 limitesZ)
    {
        float posicionX = Random.Range(limitesX.x + margenPantalla, limitesX.y - margenPantalla);
        float posicionZ = Random.Range(limitesZ.x + margenPantalla, limitesZ.y - margenPantalla);

        if (EstaEnZonaCentral(posicionX, posicionZ))
        {
            bool usarBordeHorizontal = Random.value > 0.5f;

            if (usarBordeHorizontal)
            {
                posicionX = Random.value > 0.5f ? limitesX.y - margenPantalla : limitesX.x + margenPantalla;
            }
            else
            {
                posicionZ = Random.value > 0.5f ? limitesZ.y - margenPantalla : limitesZ.x + margenPantalla;
            }
        }

        return new Vector3(posicionX, 0f, posicionZ);
    }

    bool EstaEnZonaCentral(float posicionX, float posicionZ)
    {
        return posicionX < limiteX && posicionX > -limiteX && posicionZ < limiteZ && posicionZ > -limiteZ;
    }

    GameObject ObtenerPrefabNivel(int nivel)
    {
        switch (nivel)
        {
            case 2:
                return prefabAsteroideNivel2 != null ? prefabAsteroideNivel2 : prefabAsteroideNivel1;
            case 3:
                return prefabAsteroideNivel3 != null ? prefabAsteroideNivel3 : prefabAsteroideNivel1;
            default:
                return prefabAsteroideNivel1;
        }
    }

    public int ObtenerCantidadAsteroidesGenerados()
    {
        return asteroidesGenerados;
    }
}
