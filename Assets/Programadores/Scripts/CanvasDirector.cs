using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasDirector : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TextMeshProUGUI texto;
    public GameObject textojefe; // Ahora gestionado 100% por el Canvas

    [Header("Referencias de Scripts")]
    public Reloj scriptReloj;
    public ControladorJugador jugador;

    //VARIABLES RELACIONADAS CON LA VIDA
    [Header("Referencias de Scripts")]
    [SerializeField] private Image[] listaCorazones;
    [SerializeField] private Sprite spriteCorazonLleno;
    [SerializeField] private Sprite spriteCorazonVacio;
    [SerializeField] private GameObject panelGameOver;


    private void Awake()
    {
        jugador = FindFirstObjectByType<ControladorJugador>();
    }

    void OnEnable()
    {
        jugador.ActualizacionVida += ActualizarInterfaz;
        jugador.GameOver += ActualizarGameOver;
    }

    void OnDisable()
    {
        jugador.ActualizacionVida -= ActualizarInterfaz;
        jugador.GameOver -= ActualizarGameOver;
    }


    void Update()
    {
        // Solo actualizamos el texto si tenemos las referencias correctas
        if (scriptReloj != null && texto != null)
        {
            texto.text = string.Format("te quedan {0:00}:{1:00} horas", scriptReloj.HoraTexto, scriptReloj.MinutoTexto);
        }
    }

    // Función que será llamada por el script Reloj cuando sea la hora
    public void ActivarTextoJefe()
    {
        if (textojefe != null)
        {
            textojefe.SetActive(true);
        }
    }

    void ActualizarInterfaz(int vidaActual)
    {
        for (int i = 0; i < listaCorazones.Length; i++)
        {
            if (i < jugador.vidaActual)
            {
                listaCorazones[i].enabled = true;
                // listaCorazones[i].sprite = spriteCorazonLleno; (OCUPO LOS SPRITES)
            }
            else
            {
                listaCorazones[i].enabled = false; 
                // listaCorazones[i].sprite = spriteCorazonVacio;
            }
        }
    }

    void ActualizarGameOver()
    {
        Time.timeScale = 0f;
        panelGameOver.SetActive(true);
    }
}