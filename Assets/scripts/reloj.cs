using UnityEngine;

public class reloj : MonoBehaviour
{
    public Transform pivothoras;
    public Transform pivotminutos;
    private const float tiempo = 120f;
    private float hora = 0f;

    // Update is called once per frame
    void Update()
    {
        hora += Time.deltaTime * tiempo;
        float minutos = hora / 60f;
        float horas = minutos / 60f;
        float relojHora = horas % 12f;
        float relojMinutos = minutos % 60f;
        float manecillaH = relojHora * 30f;
        float manecillaM = relojMinutos * 6f;

        pivothoras.localRotation = Quaternion.Euler(0, manecillaH, 0);
        pivotminutos.localRotation = Quaternion.Euler(0, manecillaM, 0);
    }
}
