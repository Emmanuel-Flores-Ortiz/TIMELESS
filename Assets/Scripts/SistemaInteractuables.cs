using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public class SistemaInteractuables : MonoBehaviour
{
    public enum TipoObjeto {corazon, zanahoria, pata, minutero}
    //--VARIABLES DE LOS OBJETOS--

    //VARIABLES TIPO ESTRUCTURA
    public Collider coll;
    [SerializeField] private TipoObjeto tipoObjeto;
    public ControladorJugador controlJugador;
    public Reloj scriptReloj;
    private bool enEnfriamiento = false;

    //INICIALIZO TODAS MIS VARIABLES QUE USARE EN TODO MOMENTO
    private void Awake()
    {
        coll = GetComponent<Collider>();
        controlJugador = FindFirstObjectByType<ControladorJugador>();
    }


    //CORAZON
    void eventoCorazon()
    {
        if (controlJugador != null)
        {
            coll.enabled = false;
            Destroy(gameObject);
            Debug.Log("Se borro el objeto");
        }
    }


    void eventoZanahoria()
    {
        if (controlJugador != null)
        {
            coll.enabled = false;
            controlJugador.speed = 10f;
            Destroy(gameObject);
            Debug.Log("zanahoria recolectada");
        }
    }

    void eventopata()
    {
        coll.enabled = false;
        controlJugador.speed = 10f;
        Destroy(gameObject);
        Debug.Log("zanahoria recolectada");
    }

    void eventotiempo()
    {
        if (enEnfriamiento) return;

        
        scriptReloj.adelantar(); 
        coll.enabled = false; 

        Debug.Log("MENSAJE DE PRUEBA - El tiempo se adelant�.");

      
        StartCoroutine(RutinaEnfriamiento());

        IEnumerator RutinaEnfriamiento()
    {
        enEnfriamiento = true;

        yield return new WaitForSeconds(30f);

        enEnfriamiento = false;

        if (coll != null)
        {
            coll.enabled = true; 
        }

        Debug.Log("Enfriamiento terminado. Listo para usarse de nuevo.");
    }

}


    //ONTRIGGER
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (tipoObjeto)
            {
                case TipoObjeto.corazon:
                    eventoCorazon();
                    break;

                case TipoObjeto.zanahoria:
                    eventoZanahoria();
                    break;

                case TipoObjeto.minutero:
                    eventotiempo();
                    break;
            }
        }
    }
}