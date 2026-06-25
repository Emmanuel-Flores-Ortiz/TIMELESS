using UnityEngine;
using TMPro;

public class reloj : MonoBehaviour
{
    public Transform pivothoras;
    public Transform pivotminutos;
    private const float tiempo = 120f;
    private float hora = 0f;
    public TextMeshProUGUI texto;
    public GameObject jefe;
    public GameObject textojefe;
    private bool spawnboss = false;
    private bool juego = true;
    void Start()
    {
        hora = 86000f;
    }
    void Update()
    {
        if (juego)
        {
            hora += Time.deltaTime * tiempo;
        }
       
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

        if(horas>=24f && !spawnboss)
        {
            spawnboss = true;
            jefe.SetActive(true);
            juego = false;
           
        }
    }
}
