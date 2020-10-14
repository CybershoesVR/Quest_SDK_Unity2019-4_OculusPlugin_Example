using UnityEngine;

namespace Cybershoes
{
    public class CybershoesHeightScaler : MonoBehaviour
    {
        private bool initialized = false;

        private Transform hmdTransform;
        private float standingHeight;
        private float maxOffset;
        private float duckMax;
        private float duckMin;
        public float boostHeightScaling = 0.05f; //0.00f is no boost 0.45f is max boost

        /// <summary>
        /// Calculates the scaling data to offset the player to the desired height of the character.
        /// </summary>
        /// <param name="hmd">The Transform of the hmd/camera object.</param>
        /// <param name="targetCharacterHeight">The target height of the character.</param>
        public void InitHeightScaler(Transform hmd, float targetCharacterHeight)
        {
            hmdTransform = hmd;

            standingHeight = targetCharacterHeight;
            if (hmdTransform != null)
            {
                maxOffset = targetCharacterHeight - hmdTransform.localPosition.y;
                duckMax = hmdTransform.localPosition.y - 0.05f;
                duckMin = hmdTransform.localPosition.y - 0.40f;
            }
            initialized = true;
        }

        /// <summary>
        /// Get the appropriate offset for the current height of the player.
        /// </summary>
        /// <returns></returns>
        public float CalculateOffset()
        {
            if (hmdTransform == null)
            {
                Debug.Log("Cannot calculate the Seated Offset because it has not been initialized!");
                return 0;
            }
            else if (hmdTransform.localPosition.y < duckMin)
            {
                //return 0.0f;
                return MapClamped(hmdTransform.localPosition.y, 0, duckMin, 0, duckMin - boostHeightScaling) - hmdTransform.localPosition.y;
            }
            else if (hmdTransform.localPosition.y < duckMax)
            {
                return MapClamped(hmdTransform.localPosition.y, duckMin, duckMax, duckMin - boostHeightScaling, duckMax + maxOffset) - hmdTransform.localPosition.y;
            }
            else
            {
                return maxOffset;
            }
        }

        private static float MapClamped(float input, float input_start, float input_end, float output_start, float output_end)
        {
            float output = Map(input, input_start, input_end, output_start, output_end);
            output = Mathf.Max(output, output_start);
            output = Mathf.Min(output, output_end);
            return output;
        }

        private static float Map(float input, float input_start, float input_end, float output_start, float output_end)
        {
            return output_start + ((output_end - output_start) / (input_end - input_start)) * (input - input_start);
        }
    }
}