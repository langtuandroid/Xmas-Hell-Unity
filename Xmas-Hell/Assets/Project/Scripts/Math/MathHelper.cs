using UnityEngine;

public static class MathHelper
{
    /// <summary>
    /// Get an angle in degrees from a normalized direction clamped between 0° and 360°
    /// </summary>
    /// <param name="direction">Normalized direction</param>
    /// <returns>Angle in degrees</returns>
    public static float DirectionToAngle(Vector2 direction)
    {
        // +180° to shift the values from -180 -> 180 to 0 -> 360
        return (-Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg) + 180f;
    }

    /// <summary>
    /// Get a normalized direction from an angle in degrees
    /// </summary>
    /// <param name="angle">Angle in degrees</param>
    /// <returns>Normalized direction</returns>
    public static Vector2 AngleToDirection(float angle)
    {
        // Convert the angle in radians
        angle = (angle + 90f) * Mathf.Deg2Rad;

        var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        direction.Normalize(); // make sure the direction is normalized

        return direction;
    }

    /// <summary>
    /// Wrap the given angle between 0° and 360°
    /// </summary>
    /// <param name="angle">Angle to wrap in degrees</param>
    /// <returns>Wrapped angle in degrees</returns>
    public static float WrapAngle(float angle)
    {
        return (angle + 360) % 360;
    }
}
