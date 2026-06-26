using UnityEngine;

public class SistemaDeEnemigos : MonoBehaviour
{
    public float speed;
    [SerializeField] private Transform player;
    private Vector3 forward, right;
    
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
        Vector3 direccionHaciaJugador = (player.position - transform.position).normalized;
        
        direccionHaciaJugador.y = 0;
        
        transform.position += direccionHaciaJugador * speed * Time.deltaTime;
    }
    
    
}
