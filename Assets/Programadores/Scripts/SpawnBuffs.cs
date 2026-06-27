using System.Collections;
using UnityEngine;
using static SistemaInteractuables;

public class SpawnBuffs : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public GameObject prefabBuff;
    public float radio = 15f;
    public int cantidadPorHora = 3;

    [Header("Referencia al Reloj")]
    public Reloj scriptReloj; 

    private int ultimaHoraSpawn = 0;

    void Update()
    {
       
        if (scriptReloj == null || !scriptReloj.Juego) return;
     
        int horaActual = scriptReloj.ObtenerHoraActual();

        if (horaActual > ultimaHoraSpawn)
        {
            GenerarBuffs(cantidadPorHora);
            ultimaHoraSpawn = horaActual; 
        }
    }

    void GenerarBuffs(int cantidad)
    {
        // Este bucle se repetirá la cantidad de veces que le digamos (3 veces)
        for (int i = 0; i < cantidad; i++)
        {
            Vector2 puntoCirculo = Random.insideUnitCircle.normalized;
            Vector3 posicionSpawn = new Vector3(puntoCirculo.x * radio, 2, puntoCirculo.y * radio);

            // 1. Instanciamos el prefab
            GameObject buff = Instantiate(prefabBuff, posicionSpawn, Quaternion.identity);

            // 2. Buscamos el script en el objeto creado
            SistemaInteractuables scriptInteractuable = buff.GetComponent<SistemaInteractuables>();

            if (scriptInteractuable != null)
            {
                // 3. Generamos el número aleatorio y asignamos el Enum
                int aleatorio = Random.Range(0, 3);
                scriptInteractuable.tipoObjeto = (TipoObjeto)aleatorio;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}