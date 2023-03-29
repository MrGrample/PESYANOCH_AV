using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Camera[] cameras;

    private int lastCamera = 0;

    void Start()
    {
        StartCoroutine(ChangeCamera());
    }

    void Update()
    {
        
    }

    IEnumerator ChangeCamera()
    {
        while (true)
        {

            float randomDelay = Random.Range(5f, 10f);

            yield return new WaitForSeconds(randomDelay);

            int randomCameraIndex = Random.Range(0, cameras.Length);

            Debug.Log(randomCameraIndex);

            if (randomCameraIndex != lastCamera)
            {

                if (cameras[lastCamera].gameObject.GetComponent<CPC_CameraPath>() != null)
                {
                    cameras[lastCamera].gameObject.GetComponent<CPC_CameraPath>().enabled = false;
                }

                cameras[lastCamera].gameObject.GetComponent<Camera>().enabled = false;
                cameras[randomCameraIndex].gameObject.GetComponent<Camera>().enabled = true;

                if (cameras[randomCameraIndex].gameObject.GetComponent<CPC_CameraPath>() != null)
                {
                    cameras[randomCameraIndex].gameObject.GetComponent<CPC_CameraPath>().enabled = true;
                }

                Camera.SetupCurrent(cameras[randomCameraIndex]);

                lastCamera = randomCameraIndex;
            }
        }
    }
}
