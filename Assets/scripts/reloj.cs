using UnityEngine;
using TMPro;

public class reloj : MonoBehaviour
{
    public Transform pivothoras;
    public Transform pivotminutos;
    private const float tiempo = 120f;
    private float hora = 0f;
    public TextMeshProUGUI texto;

    void Start()
    {
        hora = 86400f;
    }
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

        int horaTexto = Mathf.FloorToInt(horas % 24f);
        int minutoTexto = Mathf.FloorToInt(relojMinutos);

        if(texto != null )
        {
            texto.text = string.Format("{0:00}:{1:00}", horaTexto, minutoTexto);
        }
    }
}
