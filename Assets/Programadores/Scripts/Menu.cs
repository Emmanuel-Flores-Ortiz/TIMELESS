using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Cambiar(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);

        if (nombreEscena == "exit")
        {
            Debug.Log("Saliendo");
            Application.Quit();
        }
    }
}