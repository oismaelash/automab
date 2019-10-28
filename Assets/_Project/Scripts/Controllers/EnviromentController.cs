using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe de controle sobre ambientes
/// </summary>
public class EnviromentController : MonoBehaviour
{
    public static EnviromentController Instance;
    [SerializeField] private Sprite[] iconEnvironments;
    [HideInInspector] public List<EnvironmentModel> allEnvironments = new List<EnvironmentModel>();

    private void Awake()
    {
        Instance = this;
    }

    public void SetIcon(int idIcon, Image imageIcon)
    {
        imageIcon.sprite = iconEnvironments[idIcon];
    }

    public int GetCountIcons()
    {
        return iconEnvironments.Length;
    }
}