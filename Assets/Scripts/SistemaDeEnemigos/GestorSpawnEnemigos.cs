using UnityEngine;
using System.Collections;

public class GestorSpawnEnemigos : MonoBehaviour
{
    GameObject nuevoEnemigo;
    
    public GameObject prefabEnemigoFicha;
    public DatosEnemigos[] listaDeFichas;
    
    public float radio = 15f;
    public float tiempoDeSpawn = 2f;

    void Start()
    {
        StartCoroutine(SpawnDeEnemigos());
    }

    IEnumerator SpawnDeEnemigos()
    {
        while (true)
        {
            int indiceAleatorio = Random.Range(0, listaDeFichas.Length);
            DatosEnemigos fichaElegida = listaDeFichas[indiceAleatorio];
            
            Vector2 puntoCirculo = Random.insideUnitCircle.normalized;
            Vector3 posicionSpawn = new Vector3(puntoCirculo.x * radio, 2, puntoCirculo.y * radio);
            
            nuevoEnemigo = Instantiate(prefabEnemigoFicha, posicionSpawn, Quaternion.identity);     
            nuevoEnemigo.GetComponent<MovimientoEnemigo>().InicializarEnemigo(fichaElegida);
            
            yield return new WaitForSeconds(tiempoDeSpawn);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
