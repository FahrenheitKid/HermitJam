using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelpExtensions
{
    public static List<string> IntStringLookup = new List<string>(290);

    public static void Initiate()
    {
        for (int __i = 0; __i < 290; __i++)
        {
            IntStringLookup.Add(__i.ToString());
        }
    }

    public static int ClampCircle(int p_value, int p_min, int p_max)
    {
        if (p_value > p_max)
        {
            return p_min;
        }
        else if (p_value < p_min)
        {
            return p_max;
        }
        else
        {
            return p_value;
        }
    }

    public static int ClampMin0(int p_value)
    {
        return p_value <= 0 ? 0 : p_value;
    }

    public static float ClampMin0(float p_value)
    {
        return p_value <= 0 ? 0 : p_value;
    }

    public static float ClampMin(float p_value, float p_min)
    {
        return p_value < p_min ? p_min : p_value;
    }

    public static int ClampMax(int p_value, int p_max)
    {
        return p_value > p_max ? p_max : p_value;
    }

    public static float ClampMax(float p_value, float p_max)
    {
        return p_value > p_max ? p_max : p_value;
    }

    public static void SetX(this ref Vector2 p_vector, int p_x)
    {
        p_vector.Set(p_x, p_vector.y);
    }

    public static void SetX(this ref Vector2 p_vector, float p_x)
    {
        p_vector.Set(p_x, p_vector.y);
    }

    public static void SetY(this ref Vector2 p_vector, int p_y)
    {
        p_vector.Set(p_vector.x, p_y);
    }

    public static void SetY(this ref Vector2 p_vector, float p_y)
    {
        p_vector.Set(p_vector.x, p_y);
    }

    public static void SetX(this ref Vector3 p_vector, float p_x)
    {
        p_vector.Set(p_x, p_vector.y, p_vector.z);
    }

    public static void SetY(this ref Vector3 p_vector, float p_y)
    {
        p_vector.Set(p_vector.x, p_y, p_vector.z);
    }

    public static void AddX(this ref Vector2 p_vector, int p_amount)
    {
        p_vector.Set(p_vector.x + p_amount, p_vector.y);
    }

    public static void AddX(this ref Vector2 p_vector, float p_amount)
    {
        p_vector.Set(p_vector.x + p_amount, p_vector.y);
    }

    public static void AddY(this ref Vector2 p_vector, int p_amount)
    {
        p_vector.Set(p_vector.x, p_vector.y + p_amount);
    }

    public static void AddY(this ref Vector2 p_vector, float p_amount)
    {
        p_vector.Set(p_vector.x, p_vector.y + p_amount);
    }

    public static void MultiplyX(this ref Vector2 p_vector, int p_amount)
    {
        p_vector.Set(p_vector.x * p_amount, p_vector.y);
    }

    public static void MultiplyY(this ref Vector2 p_vector, int p_amount)
    {
        p_vector.Set(p_vector.x, p_vector.y * p_amount);
    }

    public static Vector2 Clamp(Vector2 p_vector, Vector2 p_min, Vector2 p_max)
    {
        return new Vector2(
            Mathf.Clamp(p_vector.x, p_min.x, p_max.x),
            Mathf.Clamp(p_vector.y, p_min.y, p_max.y)
            );
    }

    public static Vector2 ClampMin(Vector2 p_vector, Vector2 p_min)
    {
        return new Vector2(
            Mathf.Clamp(p_vector.x, p_min.x, Mathf.Infinity),
            Mathf.Clamp(p_vector.y, p_min.y, Mathf.Infinity)
            );
    }

    public static void SetAlpha(this SpriteRenderer p_renderer, float p_alpha)
    {
        Color __color = p_renderer.color;

        p_renderer.color = new Color(__color.r, __color.g, __color.b, p_alpha);
    }

    public static void SetAlpha<T>(this T p_renderer, float p_alpha) where T : UnityEngine.UI.Graphic
    {
        Color __color = p_renderer.color;

        p_renderer.color = new Color(__color.r, __color.g, __color.b, p_alpha);
    }

    public static int RandomSelect(int p_value1, int p_value2)
    {
        return Random.Range(0, 2) == 1 ? p_value1 : p_value2;
    }

    public static T RandomSelect<T>(T p_value1, T p_value2)
    {
        return Random.Range(0, 2) == 1 ? p_value1 : p_value2;
    }

    public static T GetRandom<T>(this List<T> p_list)
    {
        return p_list[Random.Range(0, p_list.Count - 1)];
    }

    /// <summary>
    /// Returns true if you pass a lucky draw based on the percentage you seted.
    /// </summary>
    /// <param name="p_percentage"> From 0 to 100% </param>
    public static bool LuckyDraw(int p_percentage)
    {
        return Random.Range(1, 101) <= p_percentage;
    }

    public static bool Contains(this LayerMask p_mask, int p_layer)
    {
        return (p_mask & (1 << p_layer)) != 0;
    }

    public static float GetStereoPan(float p_x)
    {
        return (p_x / 12.5f) * 0.5f;
    }
}
