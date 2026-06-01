using UnityEngine;

public class RotarPlaneta : MonoBehaviour
{
    public float velocidadRotacion = 15f;

    // Eje diagonal: mezcla de X e Y
    public Vector3 ejeRotacion = new Vector3(1f, 1f, 0f);

    void Update()
    {
        transform.Rotate(ejeRotacion.normalized * velocidadRotacion * Time.deltaTime, Space.Self);
    }
}
