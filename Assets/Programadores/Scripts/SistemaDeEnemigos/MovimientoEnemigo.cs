using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    public DatosEnemigos datosEnemigos;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private Vector3 forward, right;

    private int vidaActual;

    void Start()
    {
        if (datosEnemigos != null)
        {
            vidaActual = datosEnemigos.vida;
        }
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
        Vector3 direccionHaciaJugador = (player.position - transform.position).normalized;
        
        direccionHaciaJugador.y = 0;
        
        transform.position += direccionHaciaJugador * datosEnemigos.speed * Time.deltaTime;
    }
    
    public void InicializarEnemigo(DatosEnemigos nuevaFicha)
    {
        datosEnemigos = nuevaFicha;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (datosEnemigos != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = datosEnemigos.spriteEnemigo;
        }
    }

    public void quitarVida(int cantidad)
    {
        vidaActual -= cantidad;

        if (vidaActual <= 0)
        {
            Destroy(gameObject);
        }
    }
}
