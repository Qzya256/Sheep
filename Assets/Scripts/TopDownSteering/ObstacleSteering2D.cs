using UnityEngine;

public static class ObstacleSteering2D
{
    public static Vector2 AdjustDirection(
        Vector2 origin,
        Vector2 desiredDirection,
        float obstacleCastRadius,
        float obstacleCastDistance,
        LayerMask obstacleLayers)
    {
        if (desiredDirection.sqrMagnitude < 1e-6f)
        {
            return desiredDirection;
        }

        Vector2 normalized = desiredDirection.normalized;
        RaycastHit2D hit = Physics2D.CircleCast(
            origin,
            obstacleCastRadius,
            normalized,
            obstacleCastDistance,
            obstacleLayers);

        if (!hit)
        {
            return desiredDirection;
        }

        float magnitude = desiredDirection.magnitude;
        Vector2 normal = hit.normal;
        Vector2 projected = desiredDirection - Vector2.Dot(desiredDirection, normal) * normal;
        if (projected.sqrMagnitude < 1e-5f)
        {
            Vector2 tangent = new Vector2(-normal.y, normal.x);
            projected = tangent * magnitude;
        }

        return projected.normalized * magnitude;
    }
}
