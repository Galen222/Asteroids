using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaTransicion : MonoBehaviour
{
    public CanvasGroup panelFundido;
    public AudioManager audioManager;
    public float duracionFundido = 0.75f;

    bool transicionando;

    public void CargarEscenaConFundido(int indiceEscena)
    {
        if (transicionando)
        {
            return;
        }

        StartCoroutine(FundidoYCarga(indiceEscena));
    }


    public void PonerPantallaNegra()
    {
        if (panelFundido == null)
        {
            return;
        }

        panelFundido.gameObject.SetActive(true);
        panelFundido.alpha = 1f;
        panelFundido.blocksRaycasts = true;
    }

    public IEnumerator FundidoANegro()
    {
        if (panelFundido == null)
        {
            yield break;
        }

        panelFundido.gameObject.SetActive(true);
        panelFundido.alpha = 0f;
        panelFundido.blocksRaycasts = true;

        float tiempo = 0f;
        while (tiempo < duracionFundido)
        {
            tiempo += Time.unscaledDeltaTime;
            panelFundido.alpha = Mathf.Lerp(0f, 1f, tiempo / duracionFundido);
            yield return null;
        }

        panelFundido.alpha = 1f;
    }

    public IEnumerator FundidoDesdeNegro()
    {
        if (panelFundido == null)
        {
            yield break;
        }

        panelFundido.gameObject.SetActive(true);
        panelFundido.alpha = 1f;
        panelFundido.blocksRaycasts = true;

        float tiempo = 0f;
        while (tiempo < duracionFundido)
        {
            tiempo += Time.unscaledDeltaTime;
            panelFundido.alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFundido);
            yield return null;
        }

        panelFundido.alpha = 0f;
        panelFundido.blocksRaycasts = false;
        panelFundido.gameObject.SetActive(false);
    }

    IEnumerator FundidoYCarga(int indiceEscena)
    {
        transicionando = true;

        if (audioManager != null)
        {
            StartCoroutine(audioManager.FundidoSalidaMusica(duracionFundido));
        }

        yield return FundidoANegro();

        SceneManager.LoadScene(indiceEscena);
    }
}
