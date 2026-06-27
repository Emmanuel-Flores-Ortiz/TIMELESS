using UnityEngine;
using UnityEngine.EventSystems;

public class MinuteroUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("Referencias")]
    [SerializeField] private Reloj scriptReloj;
    [SerializeField] private RectTransform transformMinutero;

    private float anguloAnterior = 0f;

    void Start()
    {
        if (transformMinutero == null)
            transformMinutero = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Al hacer click, guardamos el ángulo inicial usando nuestra función corregida
        anguloAnterior = CalcularAnguloMouse(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (scriptReloj == null || !scriptReloj.Juego) return;

        // 1. Calcular el ángulo actual del mouse respecto al centro de la aguja
        float anguloActual = CalcularAnguloMouse(eventData.position);

        // 2. Calcular el cambio angular (Delta) respecto al cuadro anterior
        float deltaAngulo = Mathf.DeltaAngle(anguloActual, anguloAnterior);

        // 3. Sentido horario (Adelantar tiempo)
        if (deltaAngulo > 0f)
        {
            // 1 grado de arrastre = 10 segundos de tiempo real agregados
            float segundosAAdelantar = deltaAngulo * 10f;

            // Inyectamos el valor al script del mapa 3D
            scriptReloj.ModificarTiempoDesdeUI(segundosAAdelantar);
        }

        // 4. Actualizamos la referencia para el siguiente frame del arrastre
        anguloAnterior = anguloActual;
    }

    private float CalcularAnguloMouse(Vector2 posicionMouse)
    {
        // Convertimos la posición de la pantalla a coordenadas locales del padre (RelojBase)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transformMinutero.parent as RectTransform,
            posicionMouse,
            null, // Si usas Canvas en espacio de cámara, arrastra la cámara de UI aquí
            out Vector2 posicionLocalMouse
        );

        // CORRECCIÓN CRÍTICA: Calculamos la dirección usando la posición actual de la manecilla
        // como el origen de coordenadas (el centro del perno del reloj).
        Vector2 direccion = posicionLocalMouse - transformMinutero.anchoredPosition;

        // Calculamos el ángulo polar y lo convertimos a grados
        float anguloRad = Mathf.Atan2(direccion.y, direccion.x);
        return anguloRad * Mathf.Rad2Deg;
    }
}