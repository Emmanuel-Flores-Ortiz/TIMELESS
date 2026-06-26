using UnityEngine;

public class Reloj : MonoBehaviour
{
    [Header("Transformaciones del Reloj 3D")]
    public Transform pivothoras;
    public Transform pivotminutos;

    [Header("Configuración del Jefe")]
    public GameObject jefe;
    public CanvasDirector directorCanvas;

    [Header("Configuración de Adelanto")]
    public float velocidadAdelanto = 3600f;

    private const float tiempo = 120f;
    private const float limiteHoras = 24f;
    private float hora = 0f;
    private bool spawnboss = false;

    private float Adelantamiento = 0f;

    public bool Juego { get; private set; } = true;
    public int HoraTexto { get; private set; }
    public int MinutoTexto { get; private set; }

    void Start()
    {
        // hora = 86000f;
    }

    void Update()
    {
        if (Juego)
        {
         
            hora += Time.deltaTime * tiempo;

           
            if (Adelantamiento > 0f)
            {
                
                float pasoAdelanto = velocidadAdelanto * Time.deltaTime;

                // Evitamos pasarnos de la cantidad que queríamos adelantar
                if (pasoAdelanto > Adelantamiento)
                {
                    pasoAdelanto = Adelantamiento;
                }

                // Añadimos el paso al reloj y se lo restamos al tiempo pendiente
                hora += pasoAdelanto;
                Adelantamiento -= pasoAdelanto;
            }
        }

        // Cálculos de tiempo transcurrido
        float minutos = hora / 60f;
        float horas = minutos / 60f;
        float relojHora = horas % 12f;
        float relojMinutos = minutos % 60f;

        // Rotación de las manecillas
        float manecillaH = relojHora * 30f;
        float manecillaM = relojMinutos * 6f;

        pivothoras.localRotation = Quaternion.Euler(0, manecillaH, 0);
        pivotminutos.localRotation = Quaternion.Euler(0, manecillaM, 0);

        // Cálculo del tiempo restante para el Canvas
        float horasRestantes = limiteHoras - horas;

        if (horasRestantes < 0f)
        {
            horasRestantes = 0f;
        }

        float minutosRestantes = horasRestantes * 60f;

        HoraTexto = Mathf.FloorToInt(horasRestantes);
        MinutoTexto = Mathf.FloorToInt(minutosRestantes % 60f);

        // Lógica de aparición del jefe
        if (horas >= limiteHoras && !spawnboss)
        {
            spawnboss = true;
            jefe.SetActive(true);
            Juego = false;

            if (directorCanvas != null)
            {
                directorCanvas.ActivarTextoJefe();
            }
        }
    }

    public void adelantar()
    {
        if (Juego)
        {
            
            Adelantamiento += 3600f;
        }
    }
}