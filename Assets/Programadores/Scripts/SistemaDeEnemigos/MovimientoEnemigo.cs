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

    // --- COMPONENTE FÍSICO PARA EL EMPUJE ---
    private Rigidbody rb;

    void Start()
    {
        // Guardamos la referencia al Rigidbody del enemigo para poder empujarlo
        rb = GetComponent<Rigidbody>();

        canvasDirector = FindFirstObjectByType<CanvasDirector>();

        GameObject jugadorObj = GameObject.FindWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        if (player == null || datosEnemigos == null) return;

        Vector3 direccionHaciaJugador = (player.position - transform.position).normalized;
        direccionHaciaJugador.y = 0;

        transform.position += direccionHaciaJugador * datosEnemigos.speed * Time.deltaTime;
    }

    public void InicializarEnemigo(DatosEnemigos nuevaFicha, PaloCarta paloAsignado, ValorCarta valorAsignado)
    {
        datosEnemigos = nuevaFicha;

        miPalo = paloAsignado;
        miValor = valorAsignado;

        if (datosEnemigos != null)
        {
            vidaActual = datosEnemigos.vida;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (datosEnemigos != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = datosEnemigos.spriteEnemigo;
        }

        Debug.Log($"Enemigo creado: {datosEnemigos.nombre} con la carta {miValor} de {miPalo}");
    }

    // --- NUEVO MÉTODO COMPATIBLE CON EL ATAQUE DEL JUGADOR ---
    // Esta función conecta directamente con el "enemigo.GetComponent<MovimientoEnemigo>()" del jugador
    public void RecibirDañoYEmpuje(int cantidadDaño, Vector3 direccionEmpuje, float fuerza)
    {
        // 1. Aplicamos el empuje físico si el enemigo tiene Rigidbody
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Limpiamos inercias anteriores
            rb.AddForce(direccionEmpuje * fuerza, ForceMode.Impulse); // Empujón seco hacia atrás
        }

        // 2. Reutilizamos tu función existente para quitarle vida y procesar las cartas
        quitarVida(cantidadDaño);
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