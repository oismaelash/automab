using UnityEngine;
using UnityEngine.UI;

public class InputFieldUtility : MonoBehaviour
{
    [SerializeField] private GameObject txtHelp;
    [SerializeField] private GameObject txtPlaceHolder;
    [SerializeField] private Text txtUser;

    private void Update()
    {
        if (GetComponent<InputField>().isFocused)
        {
            txtHelp.SetActive(true);
            txtPlaceHolder.SetActive(false);
        }
        else
        {
            if(txtUser.text.Length != 0)
                txtHelp.SetActive(true);
            else
                txtHelp.SetActive(false);

            txtPlaceHolder.SetActive(true);
        }
    }
}