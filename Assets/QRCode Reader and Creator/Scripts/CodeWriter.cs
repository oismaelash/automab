using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; 

using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Common;


public class CodeWriter : MonoBehaviour {

	/// <summary>
	/// Code type.
	/// </summary>
	public enum CodeType
	{
		QRCode=0,
		CODE_39=1,
		CODE_128=2,
		EAN_8=3,
		EAN_13=4
	}

	private Texture2D mCodeTex;
	public delegate void QREncodeFinished(Texture2D tex);  
	public static event QREncodeFinished onCodeEncodeFinished;  

	public delegate void QREncodeError(string ErrorInfo);  
	public static event QREncodeError onCodeEncodeError;  

	BitMatrix byteMatrix;
	private int CodeWidth  =256;

	CodeType codetype = CodeType.QRCode;
	void Start ()
	{
		
	}

	/// <summary>
	/// Creates the code by codetype and code content
	/// </summary>
	/// <returns><c>true</c>, if code was created, <c>false</c> otherwise.</returns>
	/// <param name="type">Type.</param>
	/// <param name="content">Content.</param>
	public bool CreateCode(CodeType type, string content)
	{
		int imgWidth = CodeWidth;
		int imgHeight = CodeWidth;
		codetype = type;
		BarcodeFormat codeFormat = BarcodeFormat.QR_CODE;
		switch (type) {
		case CodeType.QRCode:
			{
				codeFormat = BarcodeFormat.QR_CODE;
			}
			break;
		case CodeType.CODE_39:
			{
				imgWidth = CodeWidth;
				imgHeight = CodeWidth / 2;
				codeFormat = BarcodeFormat.CODE_39;
			}
			break;
		case CodeType.CODE_128:
			{
				imgWidth = CodeWidth;
				imgHeight = CodeWidth / 2;
				codeFormat = BarcodeFormat.CODE_128;
			}
			break;
		case CodeType.EAN_8:
			{
				imgWidth = CodeWidth;
				imgHeight = CodeWidth / 2;
				codeFormat = BarcodeFormat.EAN_8;
			}
			break;
		case CodeType.EAN_13:
			{
				imgWidth = CodeWidth;
				imgHeight = CodeWidth / 2;
				codeFormat = BarcodeFormat.EAN_13;
			}
			break;
		}

		var writer = new MultiFormatWriter();// new the writer controller
		Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>(); 
		//set the code type
		hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
		hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

		try
		{
			byteMatrix = writer.encode( content, codeFormat, imgWidth, imgHeight ,hints); 
		}
		catch(Exception e) {
			onCodeEncodeError (getErrorInfo (type));
			Debug.Log ( "current :" +e);
			return false;
		}

		if (mCodeTex != null) {
			Destroy (mCodeTex);
		}

		mCodeTex = new Texture2D(byteMatrix.Width,  byteMatrix.Height);
		//set the content pixels in target image
		for (int i =0; i!= mCodeTex.width; i++) {
			for(int j = 0;j!= mCodeTex.height;j++)
			{
				if(byteMatrix[i,j])
				{
					mCodeTex.SetPixel(i,j,Color.black);
				}
				else
				{
					mCodeTex.SetPixel(i,j,Color.white);
				}
			}
		}

		Color32[] pixels = mCodeTex.GetPixels32();
		this.mCodeTex.SetPixels32(pixels);
		this.mCodeTex.Apply();
		onCodeEncodeFinished (mCodeTex);// send the message.

		return true;
	}

	/// <summary>
	/// Gets the error info.
	/// </summary>
	/// <returns>The error info.</returns>
	/// <param name="type">Type.</param>
	string getErrorInfo(CodeType type)
	{
		string error = "";
		switch (type) {
		case CodeType.QRCode:
			{
				
			}
			break;
		case CodeType.CODE_39:
			{
				error = "Code_39: Contents only contain digits !";
			}
			break;
		case CodeType.CODE_128:
			{
				error = "CODE_128: Contents length should be between 1 and 80 characters !";
			}
			break;
		case CodeType.EAN_8:
			{
				error ="EAN_8: Must contain 7 digits,the 8th digit is automatically added !";
			}
			break;
		case CodeType.EAN_13:
			{
				error = "EAN_13: Must contain 12 digits,the 13th digit is automatically added !";
			}
			break;
		}

		return error;

	}

	bool isContainDigit(string str)
	{
		for (int i = 0; i != str.Length; i++) {
			if (str [i] >= '0' && str [i] <= '9') {
				return true;
			}
		}
		return false;
	}

	bool isContainChar(string str)
	{
		for (int i = 0; i != str.Length; i++) {
			if (str [i] >= 'a' && str [i] <= 'z') {
				return true;
			}
		}
		return false;
	}

	bool bAllDigit(string str)
	{
		for (int i = 0; i != str.Length; i++) {
			if (str [i] >= '0' && str [i] <= '9') {
			} else {
				return false;
			}
		}
		return true;
	}


}
