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
    public Reloj scriptReloj; // ¡IMPORTANTE! Arrastra el GameObject que tiene el script Reloj aquí desde el Inspector

    private int ultimaHoraSpawn = -1; // Iniciamos en -1 para que el primer spawn ocurra inmediatamente en la hora 0

    void Update()
    {
        // Si no hemos asignado el reloj o el juego se detuvo (ej. apareció el jefe), no hacemos nada
        if (scriptReloj == null || !scriptReloj.Juego) return;

        // Obtenemos la hora actual del juego (0, 1, 2, 3...)
        int horaActual = scriptReloj.ObtenerHoraActual();

        // Si la hora actual es mayor a la última hora en la que spawneamos, es hora de crear más buffs
        if (horaActual > ultimaHoraSpawn)
        {
            GenerarBuffs(cantidadPorHora);
            ultimaHoraSpawn = horaActual; // Actualizamos el registro para no volver a spawnear hasta la siguiente hora
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