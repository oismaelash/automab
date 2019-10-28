using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PreviewController : MonoBehaviour {

    public static PreviewController Instance;

	public DeviceCamera devicecamera
	{
		get
		{
			return mCamera;
		}
	}

    private void Awake()
    {
        Instance = this;
    }

    public RawImage rawImg;
	DeviceCamera mCamera;
	// Use this for initialization
	IEnumerator Start()
	{
		// When the app start, ask for the authorization to use the webcam
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

		if (Application.HasUserAuthorization(UserAuthorization.WebCam))
		{
			mCamera = new DeviceCamera ();
		}
	}

	public void StartWork()
	{
		if (mCamera != null) {
			mCamera.Play ();
			rawImg.texture = mCamera.preview;
			InvokeRepeating("checkRunningCamera", 0.1f, 0.05f);
		}
	}

	/// <summary>
	/// Checks the running camera.
	/// </summary>
	void checkRunningCamera()
	{
		if (mCamera != null && mCamera.isPlaying() && mCamera.Width () > 100) {
			
			CancelInvoke ("checkRunningCamera");
			this.rawImg.transform.localEulerAngles = mCamera.GetRotation();
			this.rawImg.transform.localScale = mCamera.getVideoScale ();
			RectTransform component = this.rawImg.GetComponent<RectTransform>();
			float y = component.sizeDelta.x * (float)this.mCamera.Height() / (float)this.mCamera.Width();
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
		}
	}

	/// <summary>
	/// Stops the work.
	/// </summary>
	public void StopWork()
	{
		if (mCamera != null) {
			mCamera.Stop ();
			CancelInvoke ("checkRunningCamera");
			rawImg.texture = null;
		}

	}
	/// <summary>
	/// open the rear camera.
	/// </summary>
	/// <param name="val">If set to <c>true</c> value.</param>
	public void rearCamera(bool val = false)
	{
		if (val && mCamera != null) {
			mCamera.ActiveRearCamera ();
			rawImg.texture = mCamera.preview;
			InvokeRepeating("checkRunningCamera", 0.1f, 0.05f);
		}
	}
	/// <summary>
	/// open the front camera.
	/// </summary>
	/// <param name="val">If set to <c>true</c> value.</param>
	public void frontCamera(bool val = false)
	{
		if (val && mCamera != null) {
			mCamera.ActiveFrontCamera ();
			rawImg.texture = mCamera.preview;
			InvokeRepeating("checkRunningCamera", 0.1f, 0.05f);
		}
	}

}
