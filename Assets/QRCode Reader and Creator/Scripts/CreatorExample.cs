using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreatorExample : MonoBehaviour {

	public CodeWriter codeWtr;// drag the codewriter into this
	public InputField input; // content input
	public RawImage previewImg; // code image preview
	public Text errorText;// tip:error tips
	public CodeWriter.CodeType codetype;

	// Use this for initialization
	void Start () {
		CodeWriter.onCodeEncodeFinished += GetCodeImage;
		CodeWriter.onCodeEncodeError += errorInfo;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Creates the code.
	/// </summary>
	public void create_Code()
	{
		if (codeWtr != null) {
			codeWtr.CreateCode (codetype,input.text);
		}
	}

	/// <summary>
	/// Sets the type of the code by dropdown list.
	/// </summary>
	/// <param name="typeId">Type identifier.</param>
	public void setCodeType(int typeId)
	{
		codetype = (CodeWriter.CodeType)(typeId);
		Debug.Log ("clicked typeid is " + typeId);
	}

	/// <summary>
	/// Gets the code image.
	/// </summary>
	/// <param name="tex">Tex.</param>
	public void GetCodeImage(Texture2D tex)
	{
		RectTransform component = this.previewImg.GetComponent<RectTransform>();
		float y = component.sizeDelta.x * (float)tex.height / (float)tex.width;
		component.sizeDelta = new Vector2(component.sizeDelta.x, y);
		previewImg.texture = tex;
		errorText.text = "";
	}

	/// <summary>
	/// Errors the info.
	/// </summary>
	/// <param name="str">String.</param>
	public void errorInfo(string str)
	{
		errorText.text = str;
	}

	public void GotoReader()
	{
		Application.LoadLevel ("ReaderExample");
	}

}
