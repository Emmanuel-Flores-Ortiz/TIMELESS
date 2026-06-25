using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public class SistemaInteractuables : MonoBehaviour
{
    public enum TipoObjeto {corazon, zanahoria, pata}
    //--VARIABLES DE LOS OBJETOS--

    //VARIABLES TIPO ESTRUCTURA
    public Collider coll;
    [SerializeField] private TipoObjeto tipoObjeto;
    public ControladorJugador controlJugador;


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
            controlJugador.walkingSpeed = 10f;
            Destroy(gameObject);
            Debug.Log("zanahoria recolectada");
        }
    }

    void eventopata()
    {
        coll.enabled = false;
        controlJugador.walkingSpeed = 10f;
        Destroy(gameObject);
        Debug.Log("zanahoria recolectada");
    }

    void eventotiempo()
    {    
        coll.enabled = false;
        //Coloca aqui los eventos del tiempo
        Destroy(gameObject);
        Debug.Log("MENSAJE DE PRUEBA");
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
            }
        }
    }
}