# Cybershoes Quest SDK Developer Handbook for Oculus Utiliy Plugin 1.40

Cybershoes Functionality Scripts:
* CybershoesInput.cs
* CybershoesHeightScaler.cs

Cybershoes Example Scripts:
* CybershoesOVRPlayerController.cs
* CybershoesManager.cs

Cybershoes Example Scene:
* CybershoesSampleScene.unity

## Cybershoes SDK Scripts Details
### CybershoesInput.cs
**Public function GetRotatedShoeVector():**  
Takes the forward rotation of your hmd, e.g., yourCameraRig.centerEyeAnchor.rotation and input from a gamepad.  
The shoes are treated as a Bluetooth gamepad by their driver software.  
Bluetooth package loss is mitigated by GetRotatedShoeVector to return the most accurate current shoe rotation.  
  
### CybershoesHeightScaler.cs
**Public function InitHeightScaler():**  
Takes the transform component of your hmd and a float that signifies how tall your player should be in a standing position in meters, e.g., yourCameraRig.centerEyeAnchor and 1.75f.  
Both parameters are then assigned to member variables of the HeightScaler script. Initialize the height scaler at the moment of calibrating the player height.  
You should adjust the size of your player character model to the target standing height of the player.  

**Public function CalculateOffset():**  
Returns the difference of your target player standing height (simulated) and the current seated user height (real).  
The goal of CalculateOffset is that the returned value may then be used to position the player camera as if the user were standing instead of seated.  
Moving the camera leads to the chaperone being moved in the y-direction as well which leads to users not being able to pick up items off the ground.  
CalculateOffset incorporates a mitigation tactic that looks whether a user is bending down towards the ground and moves  the player position in the y-direction accordingly.  
This is incorporated into the return value of CalculateOffset.  

## How to use the Example Project
The Cybershoes example project uses a modified version of the OVRPlayerController script that comes with the OVRPlayerController prefab included in the Oculus Utilities Plugin for Unity.  
The example project serves only as a basic implementation of Cybershoes support.  
You are free to include the Cybershoes specific lines of code detailed further on in your own script handling player and camera movement.  
If you want to edit the OVRPlayerController in a similar fashion as shown in this example, it might be necessary to save the edited script under a different name,   
attach it to the OVRPlayerController game object and remove the original OVRPlayerController script from the game object.  
  
Open the project using UnityHub and then open the CybershoesSampleScene in Unity.  
Make sure to deny upgrading APIs if Unity shows you the prompt.  
Open the Build Settings, set the platform to Android and select ASTC in the Texture Compression menu point. Make sure that the CybershoesSampleScene is listed under Scenes In Build.  
Connect your Quest with activated developer mode to your PC and click Build And Run.  

### CybershoesOVRPlayerController.cs
The CybershoesOVRPlayerController script is a modified version of the OVRPlayerController script included with the Oculus Utilities Plugin.  
You can easily find the added Cybershoes functionality by searching the script for “added by Cybershoes”.  
Line 17 uses the namespace Cybershoes to be able to access the Cybershoes scripts.  
In line 164 the HeightScaler script is assigned to a member variable. It is located on a Game Object called CybershoesManager.  
Line 255 uses CalculateOffset to get the offset between the target player standing height (simulated) and the current user height (real).  
Line 256 adds the offset to the variable that is used by the Oculus player controller to move the player position.  
Line 392 was changed to only get the primary thumbstick of the touch controllers, as to not interfere with the input taken from the shoes.
  
### CybershoesManager.cs
This script initializes the height scaler and handles input taken from the shoes.  
The IntitHeightScaler function is invoked using a helper function in Start. This function is called with a delay of one second. The delay is necessary to initialize the height scaler successfully.   
InitHeightScaler is called using the center eye anchor of the OVRCameraRig with a target player standing height of 1.75m.  
To showcase the functionality of the height scaler, it is activated after 1 second, then deactivated after 30 seconds and then activated again after 60 seconds.  
DeactivateCybershoesHeightScaler shows an example of turning height scaling off.  
This is done by calling InitHeightScaler with null and 0 to set the member variables in the CybershoesHeightScaler script to values that are not being used.

**Private function GetCybershoesInput():**  
Example implementation of constructing a three-dimensional vector by calling the GetRotatedShoeVector function.  
GetRotatedShoeVector takes a 2D vector detailing the current state of the primary thumbstick of a gamepad and returns an adjusted two-dimensional vector of the x and y movement of the shoes.  
The 2D x value is used for the 3D x value, while the 2D y value is used for the 3D z value.  
This three-dimensional vector is then used to move the player in Update.  
The example implementation uses OVRInput.Get to query the current state of the gamepad thumbstick.  
You are, however, free to use your own solution of getting the thumbstick state and passing it to the GetRotatedShoeVector function.

CybershoesManager.cs and CybershoesHeightScaler.cs are located on the empty Game Object called CybershoesManager,  
while the CybershoesPlayerController script is located on the OVRPlayerController Game Object.



