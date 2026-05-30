using UnityEngine;

public class Asteroide : MonoBehaviour
{
    Rigidbody rb;
    public float velocidad = 3;
    public float velocidadAngular = 3;

    public AudioManager audioManager;

    GameManager gameManager;

    public GameObject particulasExplosionPrefab;

    void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 direccion = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        Vector3 direccionRotacion = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        rb.AddForce(direccion * velocidad, ForceMode.Impulse);
        rb.AddTorque(direccionRotacion * velocidadAngular, ForceMode.Impulse);

        audioManager = FindAnyObjectByType<AudioManager>();
        gameManager = FindAnyObjectByType<GameManager>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Proyectil"))
        {
            audioManager.ReproducirExplosion();
            gameManager.AsteroideDestruido();
            GameObject particulasInstanciadas = Instantiate(particulasExplosionPrefab, transform.position, Quaternion.identity);

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Nave"))
        {
            audioManager.ReproducirExplosion();

            Nave nave = collision.gameObject.GetComponent<Nave>();
            nave.Colision();
            gameManager.AsteroideDestruido();
            GameObject particulasInstanciadas = Instantiate(particulasExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}