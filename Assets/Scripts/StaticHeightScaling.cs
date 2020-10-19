using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cybershoes;

/// <summary>
/// Example Implementation of player movement class using Cybershoes
/// Use as reference for own implementation
/// </summary>
public class StaticHeightScaling : MonoBehaviour
{
    private OVRPlayerController oculusPlayerController;
    private OVRCameraRig oculusCameraRig;
    
    // Start is called before the first frame update
    void Start()
    {
        oculusPlayerController = GameObject.Find("OVRPlayerController").GetComponent<OVRPlayerController>();
        oculusPlayerController.BackAndSideDampen = 1; // Moving to the back or sides should have the same speed when using shoes.
        oculusCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
    }

    // Update is called once per frame
    void Update()
    {
        oculusPlayerController.transform.Translate(GetCybershoesInput());

        // Pressing the A button on the right Touch Controller activates the static camera offset.
        // Pressing the B button entirely deactivates any offset.
        if (OVRInput.Get(OVRInput.Button.One))
        {
            ActivateSimulatedHeight();
        }
        else if (OVRInput.Get(OVRInput.Button.Two))
        {
            DeactivateSimulatedHeight();
        }
    }

    /// <summary>
    /// Example implementation of turning dynamic height scaling off, instead using a fixed camera y-offset.
    /// Camera rig is positioned at a negative value to counteract player controller starting on y = 1 (e.g., positioned at -0.7 equals +0.3 to player height).
    /// </summary>
    private void ActivateSimulatedHeight() 
    {
        oculusPlayerController.useProfileData = false;
        oculusCameraRig.transform.localPosition = new Vector3(0,-0.7f,0);
    }

    /// <summary>
    /// Example implementation of reverting to no camera y-offset.
    /// Camera rig is positioned at the physical height of the user.
    /// </summary>
    private void DeactivateSimulatedHeight() 
    {
        oculusPlayerController.useProfileData = true;
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