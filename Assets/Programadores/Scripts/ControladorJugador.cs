using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static SistemaInteractuables;

public class ControladorJugador : MonoBehaviour
{
    //VARIABLES RELACIONADAS CON LA VIDA DEL PERSONAJE
    public int vidaMaxima = 3;
    public int vidaActual;

    //VARIABLES RELACIONADAS CON STUN
    Vector3 direccionMirada;

    //VARIABLES RELACIONADAS CON EL MOVIMIENTO
    public float speed;
    public float jumpForce;
    private Vector3 forward, right;
    private Vector2 inputMovimiento;
    private bool enElSuelo;

    public event Action<int> ActualizacionVida;
    public event Action GameOver;


    //VARIABLES RELACIONADAS CON OTROS SCRIPTS/COMPONENTES
    public InputSystem_Actions actions;
    Rigidbody rb;
    public DatosEnemigos enemigoDamage;


    void Awake()
    {
        vidaActual = vidaMaxima;
        //enemigoDamage = FindFirstObjectByType<DatosEnemigos>();
        actions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
    }


    //SISTEMA DE HABILITACION DEL INPUTSYSTEM
    void OnEnable()
    {
        actions.Player.Enable();

        actions.Player.Move.performed += Movement;
        actions.Player.Move.canceled += Movement;

        actions.Player.Jump.performed += Jumping;

        actions.Player.Attack.performed += Attacking;
    }

    void OnDisable()
    {
        actions.Player.Disable();

        actions.Player.Move.performed -= Movement;
        actions.Player.Move.canceled -= Movement;

        actions.Player.Jump.performed -= Jumping;

        actions.Player.Attack.performed -= Attacking;
    }

    //DETECCION DE LA TECLA DE INPUT PRESIONADA
    void Movement(InputAction.CallbackContext ctx)
    {
        inputMovimiento = ctx.ReadValue<Vector2>();
    }

    void Jumping(InputAction.CallbackContext ctx)
    {
        if (enElSuelo)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            enElSuelo = false;
            //animator.SetTrigger("Jump");
        }
    }

    public void Attacking(InputAction.CallbackContext ctx)
    {
        //animator.SetBool("isAttack", true);
        rb.linearVelocity = new Vector3(0, 0, 0);
        Debug.Log("Ataque realizado");
    }

    //Funcion para que el ataque no se quede bloqueado en un bucle
    public void EndAttack()
    {
        //animator.SetBool("isAttack", false);
    }

    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Camera.main.transform.right;
        right.y = 0;
        right = Vector3.Normalize(right);
    }


    //ACTUALIZACION DE LAS FISICAS (MOVIMIENTOS)
    void Update()
    {
        Vector3 direccion = inputMovimiento.x * right + inputMovimiento.y * forward;
        if (direccion.magnitude > 0.1f)
        {
            transform.position += direccion * speed * Time.deltaTime;
            direccionMirada = direccion;
        }

    }


    //FUNCIONES DETECTORAS DE SUELO (EVITA QUE EL JUGADOR SALTE INFINITAMENTE)
    private void OnCollisionEnter(Collision collision)
    {
        // Revisamos si impactamos con algo desde abajo (el suelo)
        foreach (ContactPoint contacto in collision.contacts)
        {
            // Si el vector apunta hacia arriba, es el suelo
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
        // En cuanto dejas de tocar el objeto, ya no estás en el suelo
        enElSuelo = false;
    }


    //EVENTOS RELACIONADOS CON LA VIDA DEL JUGADOR
    void dañoRecibido()
    {
        vidaActual = vidaActual - enemigoDamage.damage;
        ActualizacionVida?.Invoke(vidaActual);

        if (vidaActual <= 0)
        {
            GameOver?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Jugador detectado");
        if (other.CompareTag("enemy"))
        {
            dañoRecibido();
            Debug.Log("Jugador detectado y daño hecho");
        }
    }
}