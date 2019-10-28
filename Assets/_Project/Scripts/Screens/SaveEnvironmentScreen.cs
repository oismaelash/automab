using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe para salvar um ambiente
/// </summary>
public class SaveEnvironmentScreen : MonoBehaviour
{
    public static SaveEnvironmentScreen Instance;

    [SerializeField] private Text txtUser;
    [SerializeField] private Image imgIconEnvironment;

    public Action<int, string> newEnvironmentSave;
    private int indexEnvironment = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EnviromentController.Instance.SetIcon(indexEnvironment, imgIconEnvironment);
    }

    public void OnButtonSaveEnvironmentClicked()
    {
        var nameEnvironment = txtUser.text;

        if (nameEnvironment.Length == 0)
            nameEnvironment = "Sem nome";

        var environmentModel = new EnvironmentModel
        {
            id = EnviromentController.Instance.allEnvironments.Count + 1,
            idIcon = indexEnvironment,
            nameEnvironment = nameEnvironment
        };

        if (newEnvironmentSave != null)
            newEnvironmentSave(environmentModel.id, nameEnvironment);

        EnviromentController.Instance.allEnvironments.Add(environmentModel);
        AppManager.Instance.CheckEnvironments();

        Destroy(gameObject);
    }

    public void OnButtonNextClicked()
    {
        indexEnvironment++;

        if (indexEnvironment > (EnviromentController.Instance.GetCountIcons() - 1))
            indexEnvironment = 0;

        EnviromentController.Instance.SetIcon(indexEnvironment, imgIconEnvironment);
    }

    public void OnButtonPreviousClicked()
    {
        indexEnvironment--;

        if (indexEnvironment < 0)
            indexEnvironment = (EnviromentController.Instance.GetCountIcons() - 1);

        EnviromentController.Instance.SetIcon(indexEnvironment, imgIconEnvironment);
    }

    public void OnButtonBackClicked()
    {
        Destroy(gameObject);
    }
}