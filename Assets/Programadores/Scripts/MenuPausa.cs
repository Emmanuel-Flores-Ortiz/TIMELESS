using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPausa : MonoBehaviour
{
    //VARIABLES
    private bool pausa;
    [SerializeField] private GameObject panelMenuPausa;
    public InputSystem_Actions acciones;


    void Awake()
    {
        acciones = new InputSystem_Actions();
    }

    void OnEnable()
    {

        acciones.Player.Enable();
        acciones.Player.Pause.performed += exitPausa;
    }

    void OnDisable()
    {
        acciones.Player.Pause.performed -= exitPausa;

        acciones.Player.Disable();

    }

    public void exitPausa(InputAction.CallbackContext ctx = default)
    {
        cambioEstadoPausa();
    }
    public void cambioEstadoPausa()
    {
        if (pausa == true)
        {
            Time.timeScale = 1f;
            panelMenuPausa.SetActive(false);
            pausa = false;
        }
        else
        {
            Time.timeScale = 0f;
            panelMenuPausa.SetActive(true);
            pausa = true;
        }
    }
}