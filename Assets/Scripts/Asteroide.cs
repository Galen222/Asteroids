using UnityEngine;

public class Asteroide : MonoBehaviour
{
    Rigidbody rb;
    public float velocidad = 3;
    public float velocidadAngular = 3;

    AudioManager audioManager;

    GameManager gameManager;

    public GameObject particulasExplosionPrefab;

    bool destruido;

    void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 direccion = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        Vector3 direccionRotacion = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        rb.AddForce(direccion * velocidad, ForceMode.Impulse);
        rb.AddTorque(direccionRotacion * velocidadAngular, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (destruido)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Proyectil"))
        {
            DestruirAsteroide(true);
            Destroy(collision.gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Nave"))
        {
            Nave nave = collision.gameObject.GetComponent<Nave>();

            if (nave == null)
            {
                nave = collision.gameObject.GetComponentInParent<Nave>();
            }

            if (nave != null && nave.EsInvulnerable)
            {
                return;
            }

            if (nave != null)
            {
                nave.Colision();
            }

            // Si el asteroide se rompe al chocar con la nave, cuenta como asteroide eliminado
            // para avanzar de nivel, pero NO suma puntos.
            DestruirAsteroide(false);
        }
    }

    void DestruirAsteroide(bool sumarPuntos)
    {
        if (destruido)
        {
            return;
        }

        destruido = true;

        ReproducirExplosionSegura();

        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }

        if (gameManager != null)
        {
            gameManager.AsteroideDestruido(sumarPuntos);
        }

        if (particulasExplosionPrefab != null)
        {
            Instantiate(particulasExplosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void ReproducirExplosionSegura()
    {
        if (audioManager == null)
        {
            audioManager = FindAnyObjectByType<AudioManager>();
        }

        if (audioManager != null)
        {
            audioManager.ReproducirExplosion();
        }
    }
}
