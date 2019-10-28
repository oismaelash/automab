using UnityEngine;

/// <summary>
/// Classe que gerencia User Interface
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }
}