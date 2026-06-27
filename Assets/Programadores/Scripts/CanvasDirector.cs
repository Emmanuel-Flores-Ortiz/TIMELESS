using System.Collections; // Necesario para las Corrutinas
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Asegúrate de tener esto arriba para usar HashSet
using System;

public class CanvasDirector : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TextMeshProUGUI texto;
    public GameObject textojefe;
    public TextMeshProUGUI textoListaCombos;

    // --- NUEVO: REFERENCIA PARA LA CARTA ELIMINADA ---
    [Header("UI Cartas Eliminadas")]
    public TextMeshProUGUI textoUltimaCarta;
    private Coroutine rutinaOcultarTexto;

    [Header("Referencias de Scripts")]
    public Reloj scriptReloj;
    public ControladorJugador jugador;

    //VARIABLES RELACIONADAS CON LA VIDA
    [Header("Referencias de Vida")]
    [SerializeField] private Image[] listaCorazones;
    [SerializeField] private Sprite spriteCorazonLleno;
    [SerializeField] private Sprite spriteCorazonVacio;
    [SerializeField] private GameObject panelGameOver;

    private void Awake()
    {
        jugador = FindFirstObjectByType<ControladorJugador>();

        // Nos aseguramos de que el texto de la carta empiece apagado
        if (textoUltimaCarta != null)
        {
            textoUltimaCarta.gameObject.SetActive(false);
        }
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
        if (scriptReloj != null && texto != null)
        {
            texto.text = string.Format("te quedan {0:00}:{1:00} horas", scriptReloj.HoraTexto, scriptReloj.MinutoTexto);
        }
    }

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
            if (i <= jugador.vidaActual)
            {
                listaCorazones[i].enabled = true;
            }
            else
            {
                listaCorazones[i].enabled = false;
            }
        }
    }

    void ActualizarGameOver()
    {
        Time.timeScale = 0f;
        panelGameOver.SetActive(true);
    }

    public void MostrarCartaEliminada(PaloCarta palo, ValorCarta valor)
    {        
        if (textoUltimaCarta != null)
        {
            textoUltimaCarta.text = $"Eliminaste: {valor} de {palo}";
            textoUltimaCarta.gameObject.SetActive(true);

            if (rutinaOcultarTexto != null)
            {
                StopCoroutine(rutinaOcultarTexto);
            }
            rutinaOcultarTexto = StartCoroutine(OcultarTextoCarta());
        }      
    }

    // Corrutina para apagar el texto después de 2 segundos
    private IEnumerator OcultarTextoCarta()
    {
        yield return new WaitForSeconds(2f);
        textoUltimaCarta.gameObject.SetActive(false);
    }


    public void ActualizarListaDeCombos(HashSet<TipoCombo> combosLogrados)
    {
        if (textoListaCombos == null) return;

        string textoFinal = "<b>Combos a lograr:</b>\n";

        // Recorremos todos los combos que existen en el Enum
        foreach (TipoCombo combo in Enum.GetValues(typeof(TipoCombo)))
        {
            // Ignoramos el "Nada"
            if (combo == TipoCombo.Nada) continue;

            // Si el set de combos logrados contiene este combo, lo marcamos en verde
            if (combosLogrados.Contains(combo))
            {
                textoFinal += $"[OK] {combo}\n";
            }
            // Si no lo contiene, lo marcamos en rojo
            else
            {
                textoFinal += $"[ - ] {combo}\n";
            }
        }

        textoListaCombos.text = textoFinal;
    }
}