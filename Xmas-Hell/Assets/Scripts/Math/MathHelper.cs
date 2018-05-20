using UnityEngine;

public static class MathHelper
{
    /// <summary>
    /// Get an angle in degrees from a normalized direction
    /// </summary>
    /// <param name="direction">Normalized direction</param>
    /// <returns></returns>
    public static float DirectionToAngle(Vector2 direction)
    {
        return (-Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg);
    }

    /// <summary>
    /// Get a normalized direction from an angle in degrees
    /// </summary>
    /// <param name="angle">Angle in degrees</param>
    /// <returns></returns>
    public static Vector2 AngleToDirection(float angle)
    {
        // Convert the angle in radians
        angle = (angle + 90f) * Mathf.Deg2Rad;

        var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        direction.Normalize(); // make sure the direction is normalized

        return direction;
    }
}
