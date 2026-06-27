using System.Collections;
using UnityEngine;
using static SistemaInteractuables;

public class SpawnBuffs : MonoBehaviour
{
    public GameObject prefabBuff;

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

            // 1. Instanciamos el prefab y lo guardamos en una variable local
            GameObject buff = Instantiate(prefabBuff, posicionSpawn, Quaternion.identity);

            // 2. Buscamos el script 'SistemaInteractuables' en el objeto recién creado
            SistemaInteractuables scriptInteractuable = buff.GetComponent<SistemaInteractuables>();

            if (scriptInteractuable != null)
            {
                // 3. Generamos un número aleatorio entre 0 y 2 (Random.Range con enteros es exclusivo en el máximo)
                int aleatorio = Random.Range(0, 3);

                // 4. Asignamos el valor aleatorio convirtiendo el número al tipo de tu Enum
                // ¡IMPORTANTE! Reemplaza "NombreDeTuEnum" y "tipoObjeto" por los nombres reales que tienes en tu script SistemaInteractuables
                scriptInteractuable.tipoObjeto = (TipoObjeto)aleatorio;
            }

            yield return new WaitForSeconds(tiempoDeSpawn);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}