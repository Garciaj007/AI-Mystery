using UnityEngine;

public static class Utils
{
    public static class TimeFormat
    {
        public static string FormatTime(float time)
        {
            var minutes = (int)time / 60;
            var seconds = (int)time - 60 * minutes;
            var millis = (int)(1000 * (time - minutes * 60 - seconds));
            return $"{minutes:00}:{seconds:00}:{millis:00}";
        }
    }

    public static class Rigidbody2D
    {
        public static float Heading(Vector3 velocity)
        {
            return Vector3.Dot(Vector3.forward, velocity);
        }
    }

    public static class Mathf
    {
        public static float Map(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return (value - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
        }
    }
}
public static class VectorExtensions
{
    public static Vector2 XY(this Vector3 v)
    { return new Vector2(v.x, v.y); }
}
