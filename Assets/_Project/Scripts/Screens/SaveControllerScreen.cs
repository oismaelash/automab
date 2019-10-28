using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveControllerScreen : MonoBehaviour
{
    public int idController;
    public string nameController;

    public List<GameObject> checksSelection;
    public static Action<ControllerModel> OnNewControllerSave;

    private void Start()
    {
        nameController = "TV";
    }

    private void Update()
    {
        //print("checksSelection.Count:: " + checksSelection.Count);
    }

    public void OnButtonBackClicked(GameObject go)
    {
        AppManager.Instance.OnButtonBackScreenClicked(go);
    }

    public void OnButtonSaveControllerClicked()
    {
        var controllerModel = new ControllerModel
        {
            id = idController,
            Name = nameController
        };

        if (OnNewControllerSave != null)
            OnNewControllerSave(controllerModel);

        Destroy(gameObject);
    }

    public void DisableMakers()
    {
        print("checksSelection.Count:: " + checksSelection.Count);

        foreach (var maker in checksSelection)
            maker.SetActive(false);
    }

    public void GetIdController(int id)
    {
        DisableMakers();
        idController = id;
    }

    public void GetNameController(Text name)
    {
        nameController = name.text;
    }

    public void ActiveMakerController(GameObject maker)
    {
        maker.SetActive(true);
    }
}