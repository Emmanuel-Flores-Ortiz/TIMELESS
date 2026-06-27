using System.Collections; // Necesario para usar Corrutinas (IEnumerator)
using UnityEngine;
using UnityEngine.InputSystem;

public class minutero : MonoBehaviour
{
    public Collider coll;
    public GameObject disparo;

    private bool puedeDisparar = true; 
    private float duracionActivo = 0.25f;
    private float tiempoEspera = 1f;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (puedeDisparar && disparo != null)
            {              
                StartCoroutine(SecuenciaDisparo());
            }
            else if (disparo == null)
            {
                Debug.LogWarning("¡Falta asignar el GameObject 'disparo' en el Inspector!");
            }
        }
    }

    private IEnumerator SecuenciaDisparo()
    {
        // 1. Bloqueamos futuros disparos y activamos el objeto
        puedeDisparar = false;
        disparo.SetActive(true);

        // 2. Esperamos 0.5 segundos mientras está activo
        yield return new WaitForSeconds(duracionActivo);

        // 3. Apagamos el disparo
        disparo.SetActive(false);

        // 4. Esperamos 3 segundos de enfriamiento (cooldown)
        yield return new WaitForSeconds(tiempoEspera);

        // 5. Permitimos volver a disparar
        puedeDisparar = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            MovimientoEnemigo enemmigo = other.gameObject.GetComponent<MovimientoEnemigo>();
            if (enemmigo != null)
            {
                enemmigo.quitarVida(2);
            }
        }
    }
}