﻿using Intersect.Client.General;

namespace Intersect.Client.Core
{

    public static class Fade
    {
        private const float STANDARD_FADE_RATE = 1800f;
        private const float FAST_FADE_RATE = 800f;

        public enum FadeType
        {

            None = 0,

            In = 1,

            Out = 2,

        }

        private static FadeType sCurrentAction;

        private static float sFadeAmt;

        private static float sFadeRate = STANDARD_FADE_RATE;

        private static long sLastUpdate;

        private static bool sAlertServerWhenFaded;

        public static void FadeIn(bool fast = false)
        {
            sFadeRate = fast ? FAST_FADE_RATE : STANDARD_FADE_RATE;

            sCurrentAction = FadeType.In;
            sFadeAmt = 255f;
            sLastUpdate = Globals.System.GetTimeMs();
        }

        public static void FadeOut(bool alertServerWhenFaded = false, bool fast = false)
        {
            sFadeRate = fast ? FAST_FADE_RATE : STANDARD_FADE_RATE;
            
            sCurrentAction = FadeType.Out;
            sFadeAmt = 0f;
            sLastUpdate = Globals.System.GetTimeMs();
            sAlertServerWhenFaded = alertServerWhenFaded;
        }

        public static bool DoneFading()
        {
            return sCurrentAction == FadeType.None;
        }

        public static float GetFade()
        {
            return sFadeAmt;
        }

        public static void Update()
        {
            if (sCurrentAction == FadeType.In)
            {
                sFadeAmt -= (Globals.System.GetTimeMs() - sLastUpdate) / sFadeRate * 255f;
                if (sFadeAmt <= 0f)
                {
                    sCurrentAction = FadeType.None;
                    sFadeAmt = 0f;
                }
            }
            else if (sCurrentAction == FadeType.Out)
            {
                sFadeAmt += (Globals.System.GetTimeMs() - sLastUpdate) / sFadeRate * 255f;
                if (sFadeAmt >= 255f)
                {
                    sCurrentAction = FadeType.None;
                    if (sAlertServerWhenFaded)
                    {
                        Networking.PacketSender.SendMapTransitionReady(Globals.futureWarpMapId, Globals.futureWarpX, Globals.futureWarpY, Globals.futureWarpDir, Globals.futureWarpInstanceType);
                    }
                    sAlertServerWhenFaded = false;
                    sFadeAmt = 255f;
                }
            }

            sLastUpdate = Globals.System.GetTimeMs();
        }

    }

}
