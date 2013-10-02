using UnityEngine;
using System.Collections;

public class WebCameraScript : MonoBehaviour {
    //public GameObject CameraPlane;
    private WebCamTexture webCameraTexture;
 
 
    void Start () {
        // Checks how many and which cameras are available on the device
        for (int cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++)
        {
            // We want the back camera
            if (WebCamTexture.devices[cameraIndex].isFrontFacing)
            {
                webCameraTexture = new WebCamTexture(WebCamTexture.devices[cameraIndex].name, Screen.width, Screen.height);
                // Here we flip the GuiTexture by applying a localScale transformation
                // works only in Landscape mode
               // myCameraTexture.transform.localScale = new Vector3(-1,-1,1);
            }
        }    
        // Here we tell that the texture of coming from the camera should be applied
        // to our GUITexture. As we have flipped it before the camera preview will have the
        // correct orientation
        //myCameraTexture.texture = webCameraTexture;
        // Starts the camera
       ShowCamera();
    }
    public void ShowCamera()
    {
        renderer.material.mainTexture = webCameraTexture;
        webCameraTexture.Play();
    }
 
    public void HideCamera()
    {
      //  myCameraTexture.guiTexture.enabled = false;
        webCameraTexture.Stop();
    }
 
}