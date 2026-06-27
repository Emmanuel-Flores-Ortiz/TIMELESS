using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static SistemaInteractuables;

public class ControladorJugador : MonoBehaviour
{
    // VARIABLES RELACIONADAS CON LA VIDA DEL PERSONAJE
    [Header("Estadísticas de Vida")]
    public int vidaMaxima = 3;
    public int vidaActual;

    // VARIABLES RELACIONADAS CON ATAQUE
    [Header("Configuración de Ataque")]
    [SerializeField] private Transform puntoAtaque; // Objeto vacío al frente del jugador
    [SerializeField] private float rangoAtaque = 1.5f; // Radio del círculo de golpe
    [SerializeField] private LayerMask Enemigos; // Capa exclusiva de los enemigos
    [SerializeField] private float fuerzaEmpuje = 15f;
    [SerializeField] private int dañoAtaque = 1; // Daño base (modificable por power-ups)

    private Vector3 ultimaDireccionMirado = Vector3.forward;

    // VARIABLES RELACIONADAS CON EL MOVIMIENTO
    [Header("Configuración de Movimiento")]
    public float speed;
    public float jumpForce;
    private Vector3 forward, right;
    private Vector2 inputMovimiento;
    private bool enElSuelo;

    // EVENTOS (Notificaciones para el CanvasDirector)
    public event Action<int> ActualizacionVida;
    public event Action GameOver;

    // VARIABLES RELACIONADAS CON OTROS SCRIPTS/COMPONENTES
    public InputSystem_Actions actions;
    private Rigidbody rb;
    public DatosEnemigos enemigoDatos; // Ficha técnica global de daño enemigo
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        vidaActual = vidaMaxima;
        actions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
    }

    // SISTEMA DE HABILITACION DEL INPUT SYSTEM
    void OnEnable()
    {
        actions.Player.Enable();

        actions.Player.Move.performed += Movement;
        actions.Player.Move.canceled += Movement;

        actions.Player.Jump.performed += Jumping;

        actions.Player.AltAttack.performed += Attacking;
    }

    void OnDisable()
    {
        actions.Player.Disable();

        actions.Player.Move.performed -= Movement;
        actions.Player.Move.canceled -= Movement;

        actions.Player.Jump.performed -= Jumping;

        actions.Player.AltAttack.performed -= Attacking;
    }

    // DETECCION DE LA TECLA DE INPUT PRESIONADA
    void Movement(InputAction.CallbackContext ctx)
    {
        inputMovimiento = ctx.ReadValue<Vector2>();
    }

    void Jumping(InputAction.CallbackContext ctx)
    {
        if (enElSuelo)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            animator.SetFloat("vY", rb.linearVelocity.y);
            enElSuelo = false;
        }
    }

    public void Attacking(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // Validar que el punto de ataque esté asignado en el inspector para evitar Crash
            if (puntoAtaque == null)
            {
                Debug.LogWarning("¡Falta asignar el PuntoAtaque en el Inspector!");
                return;
            }

            rb.linearVelocity = Vector3.zero;
            Debug.Log("¡Ataque realizado!");

            // Detectamos los colliders en el rango de ataque
            Collider[] enemigosGolpeados = Physics.OverlapSphere(puntoAtaque.position, rangoAtaque, Enemigos);

            foreach (Collider enemigo in enemigosGolpeados)
            {
                // CORRECCIÓN: Buscamos el SCRIPT de vida del enemigo, no el ScriptableObject entero
                MovimientoEnemigo scriptEnemigo = enemigo.GetComponent<MovimientoEnemigo>();

                if (scriptEnemigo != null)
                {
                    scriptEnemigo.RecibirDañoYEmpuje(dañoAtaque, ultimaDireccionMirado, fuerzaEmpuje);
                }
            }
        }
    }

    // Función para animación (OPCIONAL)
    public void EndAttack()
    {
        // Lógica para terminar animación si es necesario
    }

    void Start()
    {
        // Configuración de movimiento relativo a la cámara de tu entorno 3D
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Camera.main.transform.right;
        right.y = 0;
        right = Vector3.Normalize(right);
    }

    // ACTUALIZACION DE LAS FISICAS (MOVIMIENTOS)
    void Update()
    {
        Vector3 direccion = inputMovimiento.x * right + inputMovimiento.y * forward;

        if (direccion.magnitude > 0.1f)
        {
            transform.position += direccion * speed * Time.deltaTime;

            // Guardamos la última dirección a la que se movió de forma limpia
            ultimaDireccionMirado = direccion.normalized;

            // Hace que el objeto rote hacia donde camina
            transform.forward = ultimaDireccionMirado;
        }

        // Pasamos la velocidad vertical real del personaje (física) al parámetro vY
        animator.SetFloat("vY", rb.linearVelocity.y);

        // Pasamos el estado del suelo al parámetro enSuelo
        animator.SetBool("enSuelo", enElSuelo);
    }

    // Dibuja la esfera en la ventana 'Scene' de Unity sin dar play para medir el alcance
    private void OnDrawGizmosSelected()
    {
        if (puntoAtaque == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoAtaque.position, rangoAtaque);
    }

    // DETECTORES DE SUELO (EVITA SALTO INFINITO)
    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contacto in collision.contacts)
        {
            if (contacto.normal.y > 0.6f)
            {
                enElSuelo = true;
                break;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contacto in collision.contacts)
        {
            if (contacto.normal.y > 0.6f)
            {
                enElSuelo = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        enElSuelo = false;
    }

    // EVENTOS RELACIONADOS CON LA VIDA DEL JUGADOR
    void dañoRecibido()
    {
        if (enemigoDatos != null)
        {
            vidaActual -= enemigoDatos.damage;
        }
        else
        {
            vidaActual -= 1; // Daño por defecto si no hay ficha asignada
        }

        ActualizacionVida?.Invoke(vidaActual);

        if (vidaActual <= 0)
        {
            GameOver?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            dañoRecibido();
            Debug.Log("Jugador colisionó con enemigo y recibió daño");
        }
    }

    // ==========================================================
    //  MÉTODOS PÚBLICOS PARA MEJORAS (SISTEMA INTERACTUABLES)
    // ==========================================================

    // Power-up Corazón: Sana una vida sin pasarse del máximo, y actualiza el Canvas
    public void CurarVida(int cantidad)
    {
        vidaActual += cantidad;
        if (vidaActual > vidaMaxima)
        {
            vidaActual = vidaMaxima;
        }
        ActualizacionVida?.Invoke(vidaActual); // Notifica al CanvaDirector para encender el corazón
    }

    // Power-up Daño: Cambia el daño del ataque normal (ej. pasar de 0 a 1)
    public void MejorarDaño(int nuevoDaño)
    {
        dañoAtaque = nuevoDaño;
        Debug.Log("¡Poder de ataque mejorado a: " + dañoAtaque + "!");
    }

    // Power-up Movimiento: Incrementa la velocidad del jugador
    public void AumentarVelocidad(float incremento)
    {
        speed += incremento;
        Debug.Log("¡Velocidad incrementada! Nueva velocidad: " + speed);
    }
}