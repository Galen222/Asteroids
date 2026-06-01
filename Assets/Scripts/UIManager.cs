using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    const int ESCENA_MENU = 0;
    const int ESCENA_ASTEROIDS = 1;

    public Image vida1;
    public Image vida2;
    public Image vida3;

    public GameObject gameOverPanel;
    public GameObject victoriaPanel;
    public GameObject panelNivel;

    public TextMeshProUGUI textoGameOver;
    public TextMeshProUGUI textoVictoria;
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI puntuacionGameOver;
    public TextMeshProUGUI puntuacionVictoria;
    public TextMeshProUGUI puntuacion;

    public EscenaTransicion escenaTransicion;

    public float tiempoFundidoNivel = 0.75f;
    public float tiempoTextoNivel = 1.4f;

    CanvasGroup canvasGroupNivel;

    Color visible = Color.white;
    Color transparente = new Color(1f, 1f, 1f, 0f);

    void Awake()
    {
        if (panelNivel != null)
        {
            canvasGroupNivel = panelNivel.GetComponent<CanvasGroup>();
        }
    }

    void Update()
    {
        ProcesarEscape();
    }

    void ProcesarEscape()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == ESCENA_ASTEROIDS)
        {
            BotonMenu();
        }
    }

    public void ActualizarVidas(int vidas)
    {
        SetVidaVisible(vida1, vidas >= 1);
        SetVidaVisible(vida2, vidas >= 2);
        SetVidaVisible(vida3, vidas >= 3);
    }

    void SetVidaVisible(Image vida, bool mostrar)
    {
        if (vida != null)
        {
            vida.color = mostrar ? visible : transparente;
        }
    }

    public void AjustarPuntuacion(int puntos)
    {
        if (puntuacion != null)
        {
            puntuacion.text = puntos.ToString();
        }
    }

    public void MostrarGameOver(int puntuacionFinal)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (textoGameOver != null)
        {
            textoGameOver.text = "GAME OVER";
        }

        if (puntuacionGameOver != null)
        {
            puntuacionGameOver.text = "PUNTUACION " + puntuacionFinal;
        }
    }

    public IEnumerator MostrarInicioNivel(int numeroNivel)
    {
        if (panelNivel == null || textoNivel == null)
        {
            yield break;
        }

        panelNivel.SetActive(true);
        textoNivel.text = "NIVEL " + numeroNivel;
        SetAlphaPanelNivel(1f);

        yield return new WaitForSecondsRealtime(tiempoTextoNivel);
        yield return FundidoPanelNivel(1f, 0f);

        panelNivel.SetActive(false);
        SetAlphaPanelNivel(1f);
    }

    public void PonerPantallaNegra()
    {
        if (escenaTransicion != null)
        {
            escenaTransicion.PonerPantallaNegra();
        }
    }

    public IEnumerator FundidoANegro()
    {
        if (escenaTransicion != null)
        {
            yield return escenaTransicion.FundidoANegro();
        }
    }

    public IEnumerator FundidoDesdeNegro()
    {
        if (escenaTransicion != null)
        {
            yield return escenaTransicion.FundidoDesdeNegro();
        }
    }

    public void MostrarVictoria(int puntuacionFinal)
    {
        if (victoriaPanel != null)
        {
            victoriaPanel.SetActive(true);
        }

        if (textoVictoria != null)
        {
            textoVictoria.text = "HAS GANADO";
        }

        if (puntuacionVictoria != null)
        {
            puntuacionVictoria.gameObject.SetActive(true);
            puntuacionVictoria.text = "PUNTUACION " + puntuacionFinal;
        }
    }

    IEnumerator FundidoPanelNivel(float desde, float hasta)
    {
        float tiempo = 0f;

        while (tiempo < tiempoFundidoNivel)
        {
            tiempo += Time.unscaledDeltaTime;
            SetAlphaPanelNivel(Mathf.Lerp(desde, hasta, tiempo / tiempoFundidoNivel));
            yield return null;
        }

        SetAlphaPanelNivel(hasta);
    }

    void SetAlphaPanelNivel(float alpha)
    {
        if (canvasGroupNivel != null)
        {
            canvasGroupNivel.alpha = alpha;
        }
        else if (textoNivel != null)
        {
            Color colorTexto = textoNivel.color;
            colorTexto.a = alpha;
            textoNivel.color = colorTexto;
        }
    }

    public void BotonJugarDeNuevo()
    {
        CargarEscena(ESCENA_ASTEROIDS);
    }

    public void BotonJugar()
    {
        CargarEscena(ESCENA_ASTEROIDS);
    }

    public void BotonMenu()
    {
        CargarEscena(ESCENA_MENU);
    }

    void CargarEscena(int indiceEscena)
    {
        if (escenaTransicion != null)
        {
            escenaTransicion.CargarEscenaConFundido(indiceEscena);
        }
    }

    public void BotonSalir()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
