using UnityEngine;

public class GeneradorAsteroides : MonoBehaviour
{
    public GameObject prefabAsteroide;
    public int cantidadAsteroides = 10;

    public float limiteX = 3;
    public float limiteZ = 3;

    public int asteroidesGenerados = 0;


    void Start()
    {
      
        for(int i = 0; i < cantidadAsteroides; i++)
        {
            float posicionX = Random.Range(-20f, 20f);
            float posicionZ = Random.Range(-10f, 10f);

            bool generarAsteroide = true;

            if (posicionX < limiteX && posicionX > -limiteX && posicionZ <  limiteZ && posicionZ > -limiteZ)
            {
                generarAsteroide = false;
            }
            if (generarAsteroide)
            {
                Vector3 posicionInstancia = new Vector3(posicionX, 0, posicionZ);
            
                GameObject asteroideInstanciado;
                asteroideInstanciado = Instantiate(prefabAsteroide, posicionInstancia, Random.rotation);

                asteroidesGenerados++;
            }

        }

    }

    public int ObtenerCantidadAsteroidesGenerados()
    {
        return asteroidesGenerados;
    }

}
