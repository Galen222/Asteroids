using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public AudioManager audioManager;
    public Nave nave;
    public GeneradorAsteroides generador;

    public int vidasIniciales = 3;
    public int puntosPorAsteroide = 10;

    public Renderer planetaRenderer;

    public Material planetaNivel1;
    public Material planetaNivel2;
    public Material planetaNivel3;

    public AudioClip musicaNivel1;
    public AudioClip musicaNivel2;
    public AudioClip musicaNivel3;

    int vidas;
    int puntuacion;
    int asteroidesRestantes;
    int nivelActual = 1;
    bool gameOver;
    bool victoria;
    bool cambiandoNivel;

    readonly int[] cantidadesAsteroidesPorNivel = { 10, 15, 20 };

    void Awake()
    {
        vidas = vidasIniciales;
        puntuacion = 0;
        asteroidesRestantes = 0;
        gameOver = false;
        victoria = false;
        cambiandoNivel = false;
    }

    void Start()
    {
        if (uiManager != null)
        {
            uiManager.ActualizarVidas(vidas);
            uiManager.AjustarPuntuacion(puntuacion);
        }

        if (generador != null)
        {
            StartCoroutine(ComenzarNivel(1, true));
        }
    }

    IEnumerator ComenzarNivel(int nivel, bool conFundido)
    {
        cambiandoNivel = true;
        nivelActual = nivel;

        if (nave != null)
        {
            nave.SetControlesActivos(false);
        }

        if (conFundido && uiManager != null)
        {
            if (nivelActual == 1)
            {
                uiManager.PonerPantallaNegra();
            }
            else
            {
                yield return uiManager.FundidoANegro();
            }
        }

        AplicarAspectoNivel(nivelActual);
        CambiarMusicaNivel(nivelActual);
        ReiniciarNaveParaNivel();

        if (uiManager != null)
        {
            yield return uiManager.MostrarInicioNivel(nivelActual);
        }

        if (conFundido && uiManager != null)
        {
            yield return uiManager.FundidoDesdeNegro();
        }

        int cantidad = ObtenerCantidadAsteroidesNivel(nivelActual);
        asteroidesRestantes = cantidad;

        if (generador != null)
        {
            generador.GenerarAsteroides(cantidad, nivelActual);
            asteroidesRestantes = generador.ObtenerCantidadAsteroidesGenerados();
        }

        if (nave != null && !gameOver && !victoria)
        {
            nave.SetControlesActivos(true);
        }

        cambiandoNivel = false;
    }

    public void AsteroideDestruido(bool sumarPuntos = true)
    {
        if (gameOver || victoria || cambiandoNivel)
        {
            return;
        }

        if (sumarPuntos)
        {
            puntuacion += puntosPorAsteroide;

            if (uiManager != null)
            {
                uiManager.AjustarPuntuacion(puntuacion);
            }
        }

        if (asteroidesRestantes > 0)
        {
            asteroidesRestantes--;
        }

        if (asteroidesRestantes <= 0 && vidas > 0)
        {
            if (nivelActual < 3)
            {
                StartCoroutine(ComenzarNivel(nivelActual + 1, true));
            }
            else
            {
                victoria = true;

                if (nave != null)
                {
                    nave.ReiniciarParaCambioNivel();
                    nave.SetControlesActivos(false);
                }

                if (audioManager != null)
                {
                    audioManager.DetenerMusica();
                    audioManager.ReproducirVictoria();
                }

                if (uiManager != null)
                {
                    uiManager.MostrarVictoria(puntuacion);
                }
            }
        }
    }

    public bool PerderVida()
    {
        if (gameOver || victoria)
        {
            return false;
        }

        vidas--;
        if (vidas < 0)
        {
            vidas = 0;
        }

        if (uiManager != null)
        {
            uiManager.ActualizarVidas(vidas);
        }

        if (vidas <= 0)
        {
            gameOver = true;

            if (nave != null)
            {
                nave.SetControlesActivos(false);
            }

            if (audioManager != null)
            {
                audioManager.DetenerMusica();
                audioManager.ReproducirGameOver();
            }

            if (uiManager != null)
            {
                uiManager.MostrarGameOver(puntuacion);
            }

            return false;
        }

        return true;
    }

    void ReiniciarNaveParaNivel()
    {
        if (nave != null)
        {
            nave.ReiniciarParaCambioNivel();
        }
    }

    void AplicarAspectoNivel(int nivel)
    {
        if (planetaRenderer == null)
        {
            return;
        }

        Material materialPlaneta = ObtenerMaterialPlanetaNivel(nivel);
        if (materialPlaneta != null)
        {
            planetaRenderer.sharedMaterial = materialPlaneta;
        }
    }

    void CambiarMusicaNivel(int nivel)
    {
        if (audioManager != null)
        {
            AudioClip musicaNivel = ObtenerMusicaNivel(nivel);
            audioManager.CambiarMusicaConFundido(musicaNivel);
        }
    }

    int ObtenerCantidadAsteroidesNivel(int nivel)
    {
        int indice = Mathf.Clamp(nivel - 1, 0, cantidadesAsteroidesPorNivel.Length - 1);
        return cantidadesAsteroidesPorNivel[indice];
    }

    Material ObtenerMaterialPlanetaNivel(int nivel)
    {
        switch (nivel)
        {
            case 2: return planetaNivel2 != null ? planetaNivel2 : planetaNivel1;
            case 3: return planetaNivel3 != null ? planetaNivel3 : planetaNivel1;
            default: return planetaNivel1;
        }
    }

    AudioClip ObtenerMusicaNivel(int nivel)
    {
        switch (nivel)
        {
            case 2: return musicaNivel2 != null ? musicaNivel2 : musicaNivel1;
            case 3: return musicaNivel3 != null ? musicaNivel3 : musicaNivel1;
            default: return musicaNivel1;
        }
    }
}
