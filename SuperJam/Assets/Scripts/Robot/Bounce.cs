using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
        public static float In(float k)
        {
            return 1f - Out(1f - k);
        }

        public static float Out(float k)
        {
            if (k < (1f / 2.75f))
            {
                return 7.5625f * k * k;
            }
            else if (k < (2f / 2.75f))
            {
                return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
            }
            else if (k < (2.5f / 2.75f))
            {
                return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
            }
            else
            {
                return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
            }
        }

        public static float InOut(float k)
        {
            if (k < 0.5f) return In(k * 2f) * 0.5f;
            return Out(k * 2f - 1f) * 0.5f + 0.5f;
        }
    
};

public class Back
{
    static float s = 1.70158f;
    static float s2 = 2.5949095f;

    public static float In(float k)
    {
        return k * k * ((s + 1f) * k - s);
    }

    public static float Out(float k)
    {
        return (k -= 1f) * k * ((s + 1f) * k + s) + 1f;
    }

    public static float InOut(float k)
    {
        if ((k *= 2f) < 1f) return 0.5f * (k * k * ((s2 + 1f) * k - s2));
        return 0.5f * ((k -= 2f) * k * ((s2 + 1f) * k + s2) + 2f);
    }
};