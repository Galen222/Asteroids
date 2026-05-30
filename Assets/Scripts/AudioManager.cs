using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource fxLaser;
    public AudioSource fxExplosion;
    public AudioSource fxGameOver;

    public void ReproducirLaser()
    {
        fxLaser.Play();
    }

    public void ReproducirExplosion()
    {
        fxExplosion.Play();
    }

    public void ReproducirGameOver()
    {
        fxGameOver.Play();
    }
}
