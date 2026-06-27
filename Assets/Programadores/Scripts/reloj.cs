using UnityEngine; 
public class Reloj : MonoBehaviour
{
    [Header("Transformaciones del Reloj 3D")]
    public Transform pivothoras;
    public Transform pivotminutos;
    public RectTransform minuteroUI;

    [Header("Configuración del Jefe")]
    public GameObject jefe;
    public CanvasDirector directorCanvas;

    [Header("Configuración de Adelanto")]
    public float velocidadAdelanto = 3600f;

    private const float tiempo = 120f;
    private const float limiteHoras = 24f;
    private float hora = 0f;
    private bool spawnboss = false;

    public GameObject minutero;

    private float Adelantamiento = 0f;

    private Vector3 escalaOriginal;

    public bool Juego= true;
    public int HoraTexto;
    public int MinutoTexto;

    void Start()
    {
        
        if (minutero != null)
        {
            escalaOriginal = minutero.transform.localScale;
        }
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

                if (pasoAdelanto > Adelantamiento)
                {
                    pasoAdelanto = Adelantamiento;
                }

                hora += pasoAdelanto;
                Adelantamiento -= pasoAdelanto;

              
                if (minutero != null)
                {
                    Vector3 escalaModificada = escalaOriginal;
                    escalaModificada.x += 0.18f; 
                    minutero.transform.localScale = escalaModificada;
                }
            }
            else
            {
                
                if (minutero != null)
                {
                    minutero.transform.localScale = escalaOriginal;
                }
            }
        }

        // Cálculos de tiempo 
        float minutos = hora / 60f;
        float horas = minutos / 60f;
        float relojHora = horas % 12f;
        float relojMinutos = minutos % 60f;

        // Rotación 
        float manecillaH = relojHora * 30f;
        float manecillaM = relojMinutos * 6f;

        pivothoras.localRotation = Quaternion.Euler(0, manecillaH, 0);
        pivotminutos.localRotation = Quaternion.Euler(0, manecillaM, 0);

        // Rotación 2D para el UI (Agrégalo)
        if (minuteroUI != null)
        {
            // En UI rotamos en el eje Z. El '-' es porque la UI gira inverso al espacio 3D estándar
            minuteroUI.localRotation = Quaternion.Euler(0, 0, -manecillaM);
        }

        // Cálculo del tiempo restante
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

    public int ObtenerHoraActual()
    {
        float minutos = hora / 60f;
        float horas = minutos / 60f;
        return Mathf.FloorToInt(horas);
    }

    public void ModificarTiempoDesdeUI(float segundosAñadir)
    {
        if (Juego)
        {
            hora += segundosAñadir;
        }
    }
}