using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cybershoes
{
    public class CybershoesInput
    {
        private static Vector2 cybershoesInputAxisPrevious;
        private static float [] hmdForward_frames = new float[8];    
        private static float [] timeStamp_frames = new float[8];
        private static float hmdForward_relevantFrame;
        private static int framesIndex;
        private static int minIndex;

        private static float lastBTUpdateTime = 0;
        public static float lastBTupdateTook;
        public static float assumedBTdelay = 0.050f;

        /// <summary>
        /// Returns the Cybershoes Input rotated relative to the HMD.
        /// Need to call this each rendered frame
        /// </summary>
        /// <param name="hmdForward">The rotation of the HMD in world space.</param>
        /// <returns></returns>
        public static Vector2 GetRotatedShoeVector(Quaternion hmdForward, Vector2 cybershoesInputAxis) //call this each frame
        {
            
            hmdForward_frames[framesIndex] = hmdForward.eulerAngles.y;
            timeStamp_frames[framesIndex] = Time.time;
            framesIndex++; if (framesIndex > 7) framesIndex = 0;

            if (!Mathf.Approximately(cybershoesInputAxis.magnitude, cybershoesInputAxisPrevious.magnitude)) //BT update came
            {
                cybershoesInputAxisPrevious = cybershoesInputAxis;
                float timeNow = Time.time;
                float timeMin = 1000;
                lastBTupdateTook = timeNow - lastBTUpdateTime;
                lastBTUpdateTime = timeNow; //reset

                for (int i = 0; i < 8; i++)
                {
                    float challengeTimeMin = Mathf.Abs(timeNow - timeStamp_frames[i] - assumedBTdelay);
                    if (challengeTimeMin < timeMin)
                    {
                        timeMin = challengeTimeMin;
                        minIndex = i;
                    }
                }
                hmdForward_relevantFrame = hmdForward_frames[minIndex];              
            }
            if (!Mathf.Approximately(cybershoesInputAxis.x, 0))
            {
                float diffSinceBTUpdate = hmdForward.eulerAngles.y - hmdForward_relevantFrame;
                cybershoesInputAxis = RotateVector(cybershoesInputAxis, diffSinceBTUpdate);
            }
            return cybershoesInputAxis;
        }

        private static Vector2 RotateVector(Vector2 vector, float _direction)
        { // direction in degree

            float theta = (_direction * Mathf.PI / 180);

            Vector2 oldVector = vector;   //we rotate an existing set of x,y and to not want to mess up input with result during caluclation step
                                          //float oldy = _primaryInputAxis.y;   //we rotate an existing set of x,y

            float cs = Mathf.Cos(theta);
            float sn = Mathf.Sin(theta);

            vector.x = oldVector.x * cs - oldVector.y * sn;
            vector.y = oldVector.x * sn + oldVector.y * cs;
            return vector;
        }

        /// <summary>
        /// Gets the time since the last bluetooth update
        /// </summary>
        /// <returns></returns>
        public static float GetTimeSinceLastBTUpdate()
        {
            return Time.time - lastBTUpdateTime;
        }
    }
}