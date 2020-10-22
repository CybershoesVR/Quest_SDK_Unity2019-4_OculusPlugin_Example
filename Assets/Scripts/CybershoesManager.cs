using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cybershoes;

/// <summary>
/// Example Implementation of player movement class using Cybershoes
/// Use as reference for own implementation
/// </summary>
public class CybershoesManager : MonoBehaviour
{
    private OVRPlayerController oculusPlayerController;
    private OVRCameraRig oculusCameraRig;
    private CybershoesHeightScaler cybershoesScaler;

    private float lastOffset;
    private bool scaling;
    
    // Start is called before the first frame update
    void Start()
    {
        oculusPlayerController = GameObject.Find("OVRPlayerController").GetComponent<OVRPlayerController>();
        oculusPlayerController.useProfileData = false; // Neccessary to be able to move the camera rig in y-direction.
        oculusPlayerController.BackAndSideDampen = 1; // Moving to the back or sides should have the same speed when using shoes.
        oculusCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
        oculusCameraRig.transform.localPosition = new Vector3(0, -1, 0); // Counteract player controller being at y = 1.
        cybershoesScaler = GetComponent<CybershoesHeightScaler>();

        // Showcasing height scaler functionality.
		Invoke("ActivateCybershoesHeightScaler", 1);
        Invoke("DeactivateCybershoesHeightScaler", 30);
        Invoke("ActivateCybershoesHeightScaler", 40);
    }

    /// <summary>
    /// Example implementation of starting height scaler functionality.
    /// Call using eyetracking input device at the time of activating cybershoes input mode.
    /// Call when user is at a neutral seated position.
    /// </summary>
    private void ActivateCybershoesHeightScaler()
    {
        cybershoesScaler.InitHeightScaler(oculusCameraRig.centerEyeAnchor, 1.75f);
        scaling = true;
        oculusPlayerController.useProfileData = false;
    }

    /// <summary>
    /// Example implementation of ending height scaler functionality.
    /// Call when deactivating cybershoes input mode, to revert player height to physical user height.
    /// </summary>
    private void DeactivateCybershoesHeightScaler()
    {
        scaling = false;
        oculusPlayerController.useProfileData = true;
        
    }

    private void ReactivateCybershoesHeightScaler()
    {
        scaling = true;
        oculusPlayerController.useProfileData = false;
    }

    // Update is called once per frame
    void Update()
    {
        oculusPlayerController.transform.Translate(GetCybershoesInput());

        if (scaling)
        {
            float currentOffset = cybershoesScaler.CalculateOffset();
            oculusCameraRig.transform.localPosition -= Vector3.up * lastOffset; // Subtract offset calculated in last frame.
            oculusCameraRig.transform.localPosition += Vector3.up * currentOffset; // Add offset calculated in current frame.
            lastOffset = currentOffset;
        }
    }

    /// <summary>
    /// Returns a vector detailing the x and z axes player movement according to cybershoes movement.
    /// Call per frame to constanly update player movement.
    /// </summary>
    /// <returns>characterMovement usable to update position of player camera or player controller</returns>
    private Vector3 GetCybershoesInput()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            return new Vector3(0,0,0);
        }
        Vector2 shoeMovement = new Vector2(gamepad.leftStick.x.ReadValue(), gamepad.leftStick.y.ReadValue());
        Vector2 adjustedShoeMovement = CybershoesInput.GetRotatedShoeVector(oculusCameraRig.centerEyeAnchor.rotation, shoeMovement);
        Vector3 characterMovement = new Vector3(-adjustedShoeMovement.x * Time.deltaTime * 0.5f, 0, -adjustedShoeMovement.y * Time.deltaTime * 0.5f);
        return characterMovement;
    }   
}