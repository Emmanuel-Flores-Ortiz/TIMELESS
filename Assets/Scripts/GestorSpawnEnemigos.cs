using UnityEngine;
using System.Collections;

public class GestorSpawnEnemigos : MonoBehaviour
{
    public GameObject enemigoPrefab;
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
            Vector2 puntoCirculo = Random.insideUnitCircle.normalized;
            
            Vector3 posicionSpawn = new Vector3(puntoCirculo.x * radio, 2, puntoCirculo.y * radio);
            
            Instantiate(enemigoPrefab, posicionSpawn, Quaternion.identity);
            
            yield return new WaitForSeconds(tiempoDeSpawn);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
