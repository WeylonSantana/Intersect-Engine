﻿using Intersect.Client.General;

namespace Intersect.Client.Core
{
    public static class Flash
    {
        private static float sDuration = 500f;

        private static float sFadeAmt;

        private static float sIntensity = 255f;

        private static long sLastUpdate;

        private static Color sBaseColor = Color.White;

        private static bool sDone = true;

        public static bool DoneFlashing()
        {
            return sDone;
        }

        public static float GetFlash()
        {
            return sFadeAmt;
        }

        public static Color GetColor()
        {
            return sBaseColor;
        }

        public static void FlashScreen(float duration, Color color, float intensity = 255f)
        {
            sDone = false;
            sDuration = duration;
            sLastUpdate = Globals.System.GetTimeMs();
            sBaseColor = color;
            sIntensity = intensity;
            if (sIntensity <= 0)
            {
                sIntensity = 1f;
            }
            sFadeAmt = sIntensity;
            if (sFadeAmt <= 0)
            {
                sFadeAmt = 1;
            }
        }

        public static void Update()
        {
            if (!sDone)
            {
                sFadeAmt = sIntensity - (((Globals.System.GetTimeMs() - sLastUpdate) / sDuration) * 255f);
                if (sFadeAmt <= 0f)
                {
                    sFadeAmt = 0f;
                }
            }
        }
    }
}
