using UnityEngine;

public class Nave : MonoBehaviour
{
    public float velocidad = 0.25F;
    public float velocidadAngular = 2f;
    public float velocidadMaxima = 5f;
    public Transform cañon;
    public GameObject prefabDisparo;

    Rigidbody rb;

    GameObject instanciaDisparo;

    public UIManager uiManager;

    public AudioManager audioManager;

    public GameObject particulasExplosionPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ProcesarMovimientoConFisicas();
    }

    void Update()
    {
        ProcesarDisparo();
    }

    void ProcesarMovimientoConFisicas()
    {
        float ejeVertical = Input.GetAxis("Vertical");
        float ejeHorizontal = Input.GetAxis("Horizontal");

        if(ejeVertical < 0) ejeVertical = 0;

        rb.AddRelativeForce(Vector3.forward * velocidad * ejeVertical, ForceMode.Acceleration);
        //rb.AddTorque(Vector3.up * velocidadAngular * ejeHorizontal, ForceMode.Acceleration);
        transform.Rotate(0,ejeHorizontal * velocidadAngular,0);

        rb.maxLinearVelocity = velocidadMaxima;
        //rb.maxAngularVelocity = 2;
    }

    void ProcesarDisparo()
    {  
        bool disparo = Input.GetButtonUp("Jump");
        if(disparo)
        {
            Vector3 posicionDisparo = cañon.position;
            instanciaDisparo = Instantiate(prefabDisparo, posicionDisparo, Quaternion.identity);
            Proyectil proyectil = instanciaDisparo.GetComponent<Proyectil>();
            proyectil.Impulso(cañon.forward);

            audioManager.ReproducirLaser();
        }
    }

    public void Colision()
    {
        uiManager.PerderVida();
        gameObject.SetActive(false);
        GameObject particulasInstanciadas = Instantiate(particulasExplosionPrefab, transform.position, Quaternion.identity);

        if(uiManager.QuedanVidas())
        {
            Invoke("ActivarNave", 1);
        }
    }

    void ActivarNave()
    {
        rb.linearVelocity = Vector3.zero;
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);
    }

}
