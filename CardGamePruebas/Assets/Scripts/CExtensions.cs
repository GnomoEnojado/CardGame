using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CExtensions
{
	public static RectTransform RT(this Transform tf)
	{
		return tf as RectTransform;
	}

	public static Vector3 XY0(this Vector3 v)
	{
		return new Vector3(v.x,v.y,0);
	}

	public static Color WithAlpha(this Color c, float a)
	{
		return new Color (c.r, c.g, c.b, a);
	}
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
	public static Vector3 ScreenToWorld(this Vector2 screen, Camera cam, float distance)
	{
		return cam.ScreenToWorldPoint (new Vector3(screen.x,screen.y,distance));
	}
	public static Vector3 ScreenToWorld(this Vector3 screen, Camera cam, float distance)
	{
		return cam.ScreenToWorldPoint (new Vector3(screen.x,screen.y,distance));
	}

}
