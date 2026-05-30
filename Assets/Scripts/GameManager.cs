using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GeneradorAsteroides generador;
    public GameObject victoriaUI;

    int asteroidesDestruidos = 0;
    int puntuacion = 0;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AsteroideDestruido()
    {
        asteroidesDestruidos++;
        puntuacion += 10;

        GetComponent<UIManager>().AjustarPuntuacion(puntuacion);

        if(asteroidesDestruidos >= generador.ObtenerCantidadAsteroidesGenerados())
        {
            victoriaUI.SetActive(true);
        }
        
    }
}
