using UnityEngine;

public class Disparo : MonoBehaviour
{
    public Collider coll;
    private void Awake()
    {
        coll = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            MovimientoEnemigo enemmigo = other.gameObject.GetComponent<MovimientoEnemigo>();
            if (enemmigo != null)
            {
                enemmigo.quitarVida(1);
            }
        }
    }

}
