using System;
using UnityEngine;
using MoreMountains.NiceVibrations;

namespace Fishtail.PlayTheBall.Vibration
{
    public class VibrationController : MonoBehaviour
    {
        public static VibrationController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            MMVibrationManager.iOSInitializeHaptics();
        }

        private void OnDestroy()
        {
			MMVibrationManager.iOSReleaseHaptics();
        }

        public void Impact()
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }

        public void Impact(HapticTypes type)
        {
            MMVibrationManager.Haptic(type);
        }
    }
}
