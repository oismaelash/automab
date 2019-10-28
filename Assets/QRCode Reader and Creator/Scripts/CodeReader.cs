using UnityEngine;
using System.Collections;
using System;  
using ZXing;

#if !UNITY_WEBGL
using System.Threading;
#endif

public class CodeReader : MonoBehaviour {
	
	float intervalTime = 1f;	//set the interval time for wait next scan
	float tempTime = 1;

	bool isWorking = false;

	BarcodeReader barReader;	

	public delegate void QRScanFinished(string str);  			//declare a delegate to deal with the QRcode decode complete
	public static event QRScanFinished OnCodeFinished;  		//declare a event with the delegate to trigger the complete event

	public PreviewController previewctr;// the preview controller
	int frameCount = 0;
	Result data = null;
	bool isReadyForRead = true;
	Color32[] dataColor ;

	public bool isUseAsynScanThread = true;


	Thread mAsynScanthread;
	int scanWidth = 0;
	int scanHeight = 0;
	// Use this for initialization
	void Start () {
		barReader = new BarcodeReader ();
		barReader.TryInverted = true;
		barReader.AutoRotate = true;
		if (previewctr == null) {
			previewctr = GameObject.FindObjectOfType<PreviewController> ();
		}
		tempTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (isWorking) {
			//if (frameCount++ % 20 == 0) 
			{
				if (!previewctr.devicecamera.isPlaying ()) {
					return;
				}

				if (data != null) {
					OnCodeFinished(data.Text);// get data send the message to other module
					tempTime = Time.time;
					data = null;
					frameCount = 0;
					isReadyForRead = false;
					return;
				}

				isReadyForRead = true;

				if (isUseAsynScanThread) {
					
					#if !UNITY_WEBGL
					scanWidth = previewctr.devicecamera.Width() ;
					scanHeight = previewctr.devicecamera.Height() ;

					dataColor = previewctr.devicecamera.GetPixels32 ();// get the camera pixels
					#endif

					#if UNITY_WEBGL
						if (frameCount++ % 20 == 0) {
						DecodeQR ();
						}
					#endif

				}
				else {
					if (frameCount++ % 20 == 0) {
						DecodeQR ();
					}
				}

			}
		}

	}

	public void DecodeQR()
	{

		if (!isWorking|| !isReadyForRead ||  previewctr.devicecamera.Width() <100)
		{
			return;
		}

		try
		{
			dataColor = previewctr.devicecamera.GetCenterPixels32 ();// get the camera pixels

			scanWidth = previewctr.devicecamera.centerBlockWidth ;
			scanHeight = previewctr.devicecamera.centerBlockWidth ;

			CodeDecodeThread.RunAsync(() =>
				{
					try
					{
						data = barReader.Decode(dataColor,
							scanWidth,
							scanHeight);//start decode
					}
					catch(Exception e)
					{

					}
				});

			isReadyForRead = false;
		}

		catch (Exception e)
		{
			//Log.Error(e);
		}

	}




	/// <summary>
	/// Threads the decode Q.
	/// </summary>

	public void asynScanThread()
	{
		while (data == null)
		{
			// Wait
			if (!isWorking || !isReadyForRead || scanWidth<100)
			{
				Thread.Sleep(Mathf.FloorToInt(intervalTime * 500));
				continue;
			}
			try
			{
				data = barReader.Decode(dataColor,
					scanWidth,
					scanHeight);//start decode
				
				isReadyForRead = false;

				// Sleep a little bit and set the signal to get the next frame
				Thread.Sleep(Mathf.FloorToInt(intervalTime * 800));
			}
			catch (Exception e)
			{
				
				//Log.Error(e);
			}
		}
	}

	/// <summary>
	/// Starts the work.
	/// </summary>
	public void StartWork()
	{
		if (previewctr != null) {
			isWorking = true;
			#if !UNITY_WEBGL
			if(isUseAsynScanThread)
			{
				mAsynScanthread = new Thread(asynScanThread);
				mAsynScanthread.Start();	
			}

			#endif
			previewctr.StartWork ();
		}
	}

	/// <summary>
	/// Stops the work.
	/// </summary>
	public void StopWork()
	{
		isWorking = false;
		data = null;
		#if !UNITY_WEBGL
		if (mAsynScanthread != null) {
			mAsynScanthread.Abort ();
		}
		#endif
		previewctr.StopWork ();
	}

	/// <summary>
	/// Reads the code by static texture
	/// </summary>
	/// <returns>The code.</returns>
	/// <param name="targetTex">Target tex.</param>
	public string ReadCode(Texture2D targetTex)
	{
		try
		{
			if(barReader == null)
			{
				barReader = new BarcodeReader ();
				barReader.TryInverted = true;
				barReader.AutoRotate = true;
			}
			Result data;
			data = barReader.Decode(targetTex.GetPixels32(),
				targetTex.width,
				targetTex.height);//start decode

			if (data != null) // if get the result success
			{
				return data.Text;
				//dataText = data.Text;	// use the variable to save the code result
			}
		}
		catch (Exception e){

		}
		return "";
	}
}
