using UnityEngine;

[CreateAssetMenu(fileName = "NuevoEnemigo", menuName = "Arcade/Datos de Enemigos")]
public class DatosEnemigos : ScriptableObject
{
    [Header("Indentidad Visual")]
    public string nombre;
    public Sprite spriteEnemigo;

    [Header("Propiedades del Enemigo")] 
    public int vida;
    public int damage;
    public float speed;
}
