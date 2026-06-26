using UnityEngine;
using UnityEngine.InputSystem;

public class ControladorJugador : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    private Vector3 forward, right;
    private Vector2 inputMovimiento;
    private bool enElSuelo;
    
    public InputSystem_Actions actions;
    Rigidbody rb;

    void Awake()
    {
        actions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        actions.Player.Enable();

        actions.Player.Move.performed += Movement;
        actions.Player.Move.canceled += Movement;

        actions.Player.Jump.performed += Jumping;
    }

    void OnDisable()
    {
        actions.Player.Disable();
        
        actions.Player.Move.performed -= Movement;
        actions.Player.Move.canceled -= Movement;
        
        actions.Player.Jump.performed -= Jumping;
    }

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
    
    
    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        
        right = Camera.main.transform.right;
        right.y = 0;
        right = Vector3.Normalize(right);
    }

    
    void Update()
    {
        Vector3 direccion = inputMovimiento.x*right + inputMovimiento.y*forward;
        if (direccion.magnitude > 0.1f)
        {
            transform.position += direccion * speed * Time.deltaTime;
        }
    }
    
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

    // Opcional: Añadimos esto para cuando el personaje camine por bordes o rampas
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
}