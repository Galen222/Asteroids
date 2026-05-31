using System.Collections;
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

    public GameManager gameManager;
    public AudioManager audioManager;

    public GameObject particulasExplosionPrefab;

    public TrailRenderer MotorDerecho;
    public TrailRenderer MotorIzquierdo;

    public float tiempoReaparicion = 1f;
    public float tiempoInvulnerable = 2f;
    public float intervaloParpadeo = 0.12f;

    bool invulnerable;
    bool destruida;
    bool controlesActivos = true;

    Renderer[] renderersNave;
    Collider[] collidersNave;
    Coroutine corrutinaReaparicion;
    Coroutine corrutinaParpadeo;

    Vector3 posicionInicial;
    Quaternion rotacionInicial;

    public bool EsInvulnerable => invulnerable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        renderersNave = GetComponentsInChildren<Renderer>(true);
        collidersNave = GetComponentsInChildren<Collider>(true);

        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
    }

    void Start()
    {
        SetTrailsEmitiendo(false);
        LimpiarTrails();
    }

    void FixedUpdate()
    {
        if (!destruida && controlesActivos)
        {
            ProcesarMovimientoConFisicas();
        }
        else
        {
            SetTrailsEmitiendo(false);
        }
    }

    void Update()
    {
        if (!destruida && controlesActivos)
        {
            ProcesarDisparo();
        }
    }

    void ProcesarMovimientoConFisicas()
    {
        float ejeVertical = Input.GetAxis("Vertical");
        float ejeHorizontal = Input.GetAxis("Horizontal");

        if (ejeVertical < 0)
        {
            ejeVertical = 0;
        }

        rb.AddRelativeForce(Vector3.forward * velocidad * ejeVertical, ForceMode.Acceleration);
        transform.Rotate(0, ejeHorizontal * velocidadAngular, 0);

        rb.maxLinearVelocity = velocidadMaxima;

        bool motoresActivos = ejeVertical > 0.01f;
        SetTrailsEmitiendo(motoresActivos);
    }

    void ProcesarDisparo()
    {
        bool disparo = Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.JoystickButton0);

        if (disparo)
        {
            Vector3 posicionDisparo = cañon.position;
            instanciaDisparo = Instantiate(prefabDisparo, posicionDisparo, Quaternion.identity);

            Proyectil proyectil = instanciaDisparo.GetComponent<Proyectil>();
            proyectil.Impulso(cañon.forward);

            if (audioManager != null)
            {
                audioManager.ReproducirLaser();
            }
        }
    }

    public void Colision()
    {
        if (invulnerable || destruida)
        {
            return;
        }

        bool quedanVidas = false;

        if (gameManager != null)
        {
            quedanVidas = gameManager.PerderVida();
        }

        destruida = true;
        invulnerable = false;

        SetNaveVisible(false);
        SetCollidersActivos(false);
        SetTrailsEmitiendo(false);
        LimpiarTrails();

        if (particulasExplosionPrefab != null)
        {
            Instantiate(particulasExplosionPrefab, transform.position, Quaternion.identity);
        }

        if (quedanVidas)
        {
            corrutinaReaparicion = StartCoroutine(ReaparecerNave());
        }
    }

    IEnumerator ReaparecerNave()
    {
        yield return new WaitForSeconds(tiempoReaparicion);

        ReiniciarFisicasYTransform();

        LimpiarTrails();
        SetCollidersActivos(true);
        SetNaveVisible(true);

        destruida = false;

        corrutinaParpadeo = StartCoroutine(ParpadearInvulnerable());

        yield return corrutinaParpadeo;

        corrutinaReaparicion = null;
    }


    public void SetControlesActivos(bool activos)
    {
        controlesActivos = activos;

        if (!controlesActivos)
        {
            SetTrailsEmitiendo(false);
        }
    }

    public void ReiniciarParaCambioNivel()
    {
        if (corrutinaReaparicion != null)
        {
            StopCoroutine(corrutinaReaparicion);
            corrutinaReaparicion = null;
        }

        if (corrutinaParpadeo != null)
        {
            StopCoroutine(corrutinaParpadeo);
            corrutinaParpadeo = null;
        }

        destruida = false;
        invulnerable = false;

        ReiniciarFisicasYTransform();

        SetCollidersActivos(true);
        SetNaveVisible(true);
        SetTrailsEmitiendo(false);
        LimpiarTrails();
    }

    void ReiniciarFisicasYTransform()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.position = posicionInicial;
            rb.rotation = rotacionInicial;

            rb.Sleep();
        }

        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;
    }

    IEnumerator ParpadearInvulnerable()
    {
        invulnerable = true;

        float tiempo = 0f;
        bool visible = true;

        while (tiempo < tiempoInvulnerable)
        {
            visible = !visible;
            SetNaveVisible(visible);

            yield return new WaitForSeconds(intervaloParpadeo);
            tiempo += intervaloParpadeo;
        }

        SetNaveVisible(true);
        invulnerable = false;
        corrutinaParpadeo = null;
    }

    void SetNaveVisible(bool visible)
    {
        if (renderersNave == null)
        {
            return;
        }

        foreach (Renderer rendererNave in renderersNave)
        {
            if (rendererNave != null && !(rendererNave is TrailRenderer))
            {
                rendererNave.enabled = visible;
            }
        }
    }

    void SetCollidersActivos(bool activos)
    {
        if (collidersNave == null)
        {
            return;
        }

        foreach (Collider colliderNave in collidersNave)
        {
            if (colliderNave != null)
            {
                colliderNave.enabled = activos;
            }
        }
    }

    void SetTrailsEmitiendo(bool emitiendo)
    {
        if (MotorDerecho != null)
        {
            MotorDerecho.emitting = emitiendo;
        }

        if (MotorIzquierdo != null)
        {
            MotorIzquierdo.emitting = emitiendo;
        }
    }

    void LimpiarTrails()
    {
        if (MotorDerecho != null)
        {
            MotorDerecho.Clear();
        }

        if (MotorIzquierdo != null)
        {
            MotorIzquierdo.Clear();
        }
    }
}
