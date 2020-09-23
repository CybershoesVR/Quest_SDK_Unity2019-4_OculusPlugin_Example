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
    private OVRCameraRig oculusCameraRig;
    private CybershoesHeightScaler cybershoesScaler;
    
    // Start is called before the first frame update
    void Start()
    {
        oculusCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
        cybershoesScaler = GetComponent<CybershoesHeightScaler>();
		Invoke("StartCybershoesHeightScaler", 1);
        Invoke("DeactivateCybershoesHeightScaler", 30);
        Invoke("StartCybershoesHeightScaler", 60);
    }

    void StartCybershoesHeightScaler()
    {
        cybershoesScaler.InitHeightScaler(oculusCameraRig.centerEyeAnchor, 1.75f);
    }

    void DeactivateCybershoesHeightScaler()
    {
        cybershoesScaler.InitHeightScaler(null, 0);
    }

    // Update is called once per frame
    void Update()
    {
        oculusCameraRig.transform.Translate(GetCybershoesInput());
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
        Vector3 characterMovement = new Vector3(adjustedShoeMovement.x * 0.05f, 0, adjustedShoeMovement.y * 0.05f);
        return characterMovement;
    }   
}