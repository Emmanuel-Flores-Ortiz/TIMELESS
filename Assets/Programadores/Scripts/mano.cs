using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 1. DEFINICIÓN DE LOS COMBOS (Debe ir fuera de la clase)
public enum TipoCombo
{
    Nada, Par, DoblePar, Tercia, Escalera, FullHouse, CuatroDeUno
}

// 2. ESTRUCTURA DE LA CARTA (Debe ir fuera de la clase)
[System.Serializable]
public struct CartaRecogida
{
    public PaloCarta palo;
    public ValorCarta valor;
}

// 3. TU CLASE PRINCIPAL
public class mano : MonoBehaviour
{
    [Header("Mano Actual")]
    public List<CartaRecogida> cartasEnMano = new List<CartaRecogida>();

    private HashSet<TipoCombo> combosCompletados = new HashSet<TipoCombo>();
    private CanvasDirector canvasDirector;

    private void Start()
    {
        canvasDirector = FindFirstObjectByType<CanvasDirector>();

        if (canvasDirector != null)
        {
            canvasDirector.ActualizarListaDeCombos(combosCompletados);
        }
    }

    public void RecogerCarta(PaloCarta palo, ValorCarta valor)
    {
        // Agregamos la carta a la mano
        cartasEnMano.Add(new CartaRecogida { palo = palo, valor = valor });
        Debug.Log($"Carta recogida: {valor} {palo}. Llevas {cartasEnMano.Count} cartas.");

        // Evaluamos la mano INMEDIATAMENTE
        TipoCombo comboActual = EvaluarCartas(cartasEnMano);

        if (comboActual != TipoCombo.Nada)
        {
            // Si el combo no estaba en nuestra lista de logrados, lo agregamos
            if (!combosCompletados.Contains(comboActual))
            {
                combosCompletados.Add(comboActual);
                Debug.Log($"¡NUEVO COMBO DESBLOQUEADO!: {comboActual}");

                if (canvasDirector != null)
                {
                    canvasDirector.ActualizarListaDeCombos(combosCompletados);
                }
            }
        }

        // Si la mano se llenó (5 cartas), la reiniciamos para empezar de nuevo.
        if (cartasEnMano.Count >= 5)
        {
            cartasEnMano.Clear();
            Debug.Log("Mano llena evaluada. Reiniciando cartas...");
        }
    }

    private TipoCombo EvaluarCartas(List<CartaRecogida> mano)
    {
        // Si tenemos menos de 2 cartas, es imposible tener un combo
        if (mano.Count < 2) return TipoCombo.Nada;

        var grupos = mano.GroupBy(c => c.valor)
                         .OrderByDescending(g => g.Count())
                         .ToList();

        int maxIguales = grupos[0].Count();

        // Evaluamos de mayor a menor jerarquía
        if (maxIguales == 4) return TipoCombo.CuatroDeUno;

        // Full House requiere tener 5 cartas
        if (mano.Count >= 5 && maxIguales == 3 && grupos.Count > 1 && grupos[1].Count() == 2)
            return TipoCombo.FullHouse;

        if (maxIguales == 3) return TipoCombo.Tercia;

        // Doble par requiere al menos tener 4 cartas
        if (grupos.Count > 1 && maxIguales == 2 && grupos[1].Count() == 2)
            return TipoCombo.DoblePar;

        if (maxIguales == 2) return TipoCombo.Par;

        // La escalera requiere estrictamente las 5 cartas
        if (mano.Count == 5 && EsEscalera(mano)) return TipoCombo.Escalera;

        return TipoCombo.Nada;
    }

    private bool EsEscalera(List<CartaRecogida> mano)
    {
        List<int> valoresOrdenados = mano.Select(c => (int)c.valor).OrderBy(v => v).ToList();

        if (valoresOrdenados.SequenceEqual(new List<int> { 1, 10, 11, 12, 13 }))
        {
            return true;
        }

        for (int i = 0; i < valoresOrdenados.Count - 1; i++)
        {
            if (valoresOrdenados[i + 1] != valoresOrdenados[i] + 1)
            {
                return false;
            }
        }

        return true;
    }
}