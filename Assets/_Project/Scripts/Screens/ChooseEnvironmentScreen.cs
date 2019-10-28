using System;
using UnityEngine;

/// <summary>
/// Classe que controla quando usuario estar escolhendo um ambiente
/// </summary>
public class ChooseEnvironmentScreen : MonoBehaviour
{
    public static Action<EnvironmentModel> OnChoosedEnvironment;
    private int lastEnvironmentChoosed;
    [SerializeField] private Transform contentShowButtonEnviroment;
    private EnvironmentModel environmentModelChoosed;

    private void Start()
    {
        AppManager.Instance.ShowButtonEnvironments(contentShowButtonEnviroment, OnButtonEnvironmentChoosedClicked);
    }

    public void OnButtonBackClicked(GameObject go)
    {
        if(OnChoosedEnvironment != null)
            OnChoosedEnvironment(environmentModelChoosed);

        AppManager.Instance.OnButtonBackScreenClicked(go);
    }

    public void OnButtonEnvironmentChoosedClicked(EnvironmentModel environment)
    {
        environmentModelChoosed = environment;
        print("environmentModelChoosed.nameEnvironment:: " + environmentModelChoosed.nameEnvironment);
    }
}