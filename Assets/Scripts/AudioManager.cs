using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musica;
    public AudioSource fxLaser;
    public AudioSource fxExplosion;
    public AudioSource fxGameOver;
    public AudioSource fxVictoria;

    float volumenMusicaObjetivo;
    Coroutine corrutinaMusica;

    public float duracionFundidoMusica = 0.75f;

    void Awake()
    {
        if (musica != null)
        {
            musica.loop = true;
            volumenMusicaObjetivo = musica.volume;
        }
    }

    void Start()
    {
        if (musica != null)
        {
            StartCoroutine(FundidoEntradaMusica());
        }
    }

    public void ReproducirLaser()
    {
        if (fxLaser != null)
        {
            fxLaser.Play();
        }
    }

    public void ReproducirExplosion()
    {
        if (fxExplosion != null)
        {
            fxExplosion.Play();
        }
    }

    public void ReproducirGameOver()
    {
        if (fxGameOver != null)
        {
            fxGameOver.Play();
        }
    }

    public void ReproducirVictoria()
    {
        if (fxVictoria != null)
        {
            fxVictoria.Play();
        }
    }

    public void DetenerMusica()
    {
        if (corrutinaMusica != null)
        {
            StopCoroutine(corrutinaMusica);
            corrutinaMusica = null;
        }

        if (musica != null)
        {
            musica.Stop();
            musica.volume = volumenMusicaObjetivo;
        }
    }

    public IEnumerator FundidoSalidaMusica(float duracion)
    {
        if (corrutinaMusica != null)
        {
            StopCoroutine(corrutinaMusica);
            corrutinaMusica = null;
        }

        if (musica == null || !musica.isPlaying)
        {
            yield break;
        }

        float volumenInicial = musica.volume;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.unscaledDeltaTime;
            float progreso = Mathf.Clamp01(tiempo / duracion);
            musica.volume = Mathf.Lerp(volumenInicial, 0f, progreso);
            yield return null;
        }

        musica.Stop();
        musica.volume = volumenMusicaObjetivo;
    }

    public void CambiarMusicaConFundido(AudioClip nuevoClip)
    {
        if (musica == null || nuevoClip == null)
        {
            return;
        }

        if (musica.clip == nuevoClip && musica.isPlaying)
        {
            return;
        }

        if (corrutinaMusica != null)
        {
            StopCoroutine(corrutinaMusica);
        }

        corrutinaMusica = StartCoroutine(FundidoCambioMusica(nuevoClip));
    }

    IEnumerator FundidoEntradaMusica()
    {
        if (musica == null || musica.clip == null)
        {
            yield break;
        }

        if (!musica.isPlaying)
        {
            musica.Play();
        }

        float volumenFinal = volumenMusicaObjetivo;
        musica.volume = 0f;

        float tiempo = 0f;
        while (tiempo < duracionFundidoMusica)
        {
            tiempo += Time.unscaledDeltaTime;
            musica.volume = Mathf.Lerp(0f, volumenFinal, tiempo / duracionFundidoMusica);
            yield return null;
        }

        musica.volume = volumenFinal;
    }

    IEnumerator FundidoCambioMusica(AudioClip nuevoClip)
    {
        float volumenFinal = volumenMusicaObjetivo;
        float volumenInicial = musica.volume;
        float tiempo = 0f;

        while (tiempo < duracionFundidoMusica)
        {
            tiempo += Time.unscaledDeltaTime;
            musica.volume = Mathf.Lerp(volumenInicial, 0f, tiempo / duracionFundidoMusica);
            yield return null;
        }

        musica.Stop();
        musica.clip = nuevoClip;
        musica.volume = 0f;
        musica.Play();

        tiempo = 0f;
        while (tiempo < duracionFundidoMusica)
        {
            tiempo += Time.unscaledDeltaTime;
            musica.volume = Mathf.Lerp(0f, volumenFinal, tiempo / duracionFundidoMusica);
            yield return null;
        }

        musica.volume = volumenFinal;
        corrutinaMusica = null;
    }
}
