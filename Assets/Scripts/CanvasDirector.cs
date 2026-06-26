using UnityEngine;
using TMPro;

public class CanvasDirector : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TextMeshProUGUI texto;
    public GameObject textojefe; // Ahora gestionado 100% por el Canvas

    [Header("Referencias de Scripts")]
    public Reloj scriptReloj;

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
}