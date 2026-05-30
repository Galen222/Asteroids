using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Image vida1;
    public Image vida2;
    public Image vida3;

    public GameObject gameOverPanel;

    public AudioManager audioManager;

    public TextMeshProUGUI puntuacion;

    int vidas = 3;

    Color transparente;

    void Start()
    {
        transparente = new Color (0, 0, 0, 0);

    }

    public void PerderVida()
    {
        if (vidas == 3)
        {
            vida3.color = transparente;
            vidas--;
        }
        else
        if (vidas == 2)
        {
            vida2.color = transparente;
            vidas--;
        }
        else
        if (vidas == 1)
        {
            vida1.color = transparente;
            vidas--;
        }

        if (vidas == 0)
        {
            gameOverPanel.SetActive(true);
            audioManager.ReproducirGameOver();
        }
    }

    public bool QuedanVidas()
    {
        return vidas > 0;
    }

    public void BotonJugarDeNuevo()
    {
        Invoke("RecargarEscena", 0.2f);
    }

    public void BotonJugar()
    {
        Invoke("RecargarEscena", 0.2f);
    }

    void RecargarEscena()
    {
        SceneManager.LoadScene(1);
    }

    public void AjustarPuntuacion(int puntos)
    {
        puntuacion.text = puntos.ToString();
    }
}
