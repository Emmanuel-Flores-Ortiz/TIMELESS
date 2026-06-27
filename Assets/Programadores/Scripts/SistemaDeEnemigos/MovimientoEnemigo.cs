using UnityEngine;
using static GestorSpawnEnemigos;

public class MovimientoEnemigo : MonoBehaviour
{
    public DatosEnemigos datosEnemigos;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private Vector3 forward, right;
    public CanvasDirector canvasDirector;
    private int vidaActual;

    // --- NUEVAS VARIABLES PARA ALMACENAR LA CARTA ---
    [Header("Identidad de la Carta")]
    public PaloCarta miPalo;
    public ValorCarta miValor;

    void Start()
    {
        canvasDirector = FindFirstObjectByType<CanvasDirector>();

        GameObject jugadorObj = GameObject.FindWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>(); // Por seguridad si no se ejecutó antes

        if (jugadorObj != null)
        {
            player = jugadorObj.transform;
        }

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Camera.main.transform.right;
        right.y = 0;
        right = Vector3.Normalize(right);
    }

    void Update()
    {
        if (player == null || datosEnemigos == null) return; // Seguridad para evitar errores de referencia nula

        Vector3 direccionHaciaJugador = (player.position - transform.position).normalized;
        direccionHaciaJugador.y = 0;

        transform.position += direccionHaciaJugador * datosEnemigos.speed * Time.deltaTime;
    }

    // Modificamos el método para aceptar la información de la carta aleatoria
    public void InicializarEnemigo(DatosEnemigos nuevaFicha, PaloCarta paloAsignado, ValorCarta valorAsignado)
    {
        datosEnemigos = nuevaFicha;

        // Guardamos su tipo de carta
        miPalo = paloAsignado;
        miValor = valorAsignado;

        // Asignamos la vida basándonos en los datos que acaban de llegar
        if (datosEnemigos != null)
        {
            vidaActual = datosEnemigos.vida;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (datosEnemigos != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = datosEnemigos.spriteEnemigo;
        }

        // Línea opcional para verificar en la consola que todo funcione:
        Debug.Log($"Enemigo creado: {datosEnemigos.nombre} con la carta {miValor} de {miPalo}");
    }

    public void quitarVida(int cantidad)
    {
        mano evaluador = FindFirstObjectByType<mano>();
        if (evaluador != null)
        {
            evaluador.RecogerCarta(miPalo, miValor);
        }
        vidaActual -= cantidad;

        if (vidaActual <= 0)
        {
            if (canvasDirector != null)
            {
               
                canvasDirector.MostrarCartaEliminada(miPalo, miValor);
            }
         
            Destroy(gameObject);
        }
    }
}