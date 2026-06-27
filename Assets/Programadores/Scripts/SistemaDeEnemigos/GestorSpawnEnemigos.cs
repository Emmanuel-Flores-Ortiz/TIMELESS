using UnityEngine;
using System.Collections;

public enum PaloCarta
{
    Diamantes,
    Picas
}

public enum ValorCarta
{
    As = 1, Dos, Tres, Cuatro, Cinco, Seis, Siete, Ocho, Nueve, Diez, Jota, Reina, Rey
}
public class GestorSpawnEnemigos : MonoBehaviour
{
    GameObject nuevoEnemigo;

    public GameObject prefabEnemigoFicha;
    public DatosEnemigos[] listaDeFichas;
    Collider2D col;

    public float radio = 15f;
    public float tiempoDeSpawn = 2f;
   
    void Start()
    {
        col = GetComponent<Collider2D>();
        StartCoroutine(SpawnDeEnemigos());
    }

    IEnumerator SpawnDeEnemigos()
    {
        while (true)
        {
            if (listaDeFichas.Length > 0)
            {
                // 1. Elegir datos base aleatorios (Stats)
                int indiceAleatorio = Random.Range(0, listaDeFichas.Length);
                DatosEnemigos fichaElegida = listaDeFichas[indiceAleatorio];

                // 2. Generar el palo aleatorio (Diamantes o Picas)
                // Random.value da un número entre 0.0 y 1.0. 
                // Si es mayor a 0.5 es Diamantes, de lo contrario es Picas.
                PaloCarta paloAleatorio = (Random.value > 0.5f) ? PaloCarta.Diamantes : PaloCarta.Picas;

                // 3. Generar el valor de la carta aleatorio (1 al 13)
                // El rango de enteros en Random.Range excluye el número máximo, por eso usamos 14 para llegar al 13 (Rey).
                ValorCarta valorAleatorio = (ValorCarta)Random.Range(1, 14);

                // 4. Calcular posición de Spawn
                Vector2 puntoCirculo = Random.insideUnitCircle.normalized;
                Vector3 posicionSpawn = new Vector3(puntoCirculo.x * radio, 2, puntoCirculo.y * radio);

                // 5. Instanciar e inicializar enviando todos los datos
                nuevoEnemigo = Instantiate(prefabEnemigoFicha, posicionSpawn, Quaternion.identity);
                nuevoEnemigo.GetComponent<MovimientoEnemigo>().InicializarEnemigo(fichaElegida, paloAleatorio, valorAleatorio);
            }

            yield return new WaitForSeconds(tiempoDeSpawn);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}