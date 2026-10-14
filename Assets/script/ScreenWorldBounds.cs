using UnityEngine;

public static class ScreenWorldBounds
{
    private static Camera targetCamera;
    private static bool initialized;
    private static float minX;
    private static float maxX;
    private static float minY;
    private static float maxY;
    private static float worldZ;

    public static float MinX => minX;
    public static float MaxX => maxX;
    public static float MinY => minY;
    public static float MaxY => maxY;
    public static Vector2 Center => new Vector2((minX + maxX) * 0.5f, (minY + maxY) * 0.5f);

    public static void Initialize(Camera camera, float targetWorldZ = 0f)
    {
        if (camera == null)
        {
            Debug.LogError("ScreenWorldBounds Initialize Failed: camera is null");
            return;
        }

        targetCamera = camera;
        worldZ = targetWorldZ;
        Refresh();
        initialized = true;
    }

    public static void Refresh()
    {
        if (targetCamera == null) return;

        float distance = worldZ - targetCamera.transform.position.z;
        Vector3 bottomLeft = targetCamera.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
        Vector3 topRight = targetCamera.ViewportToWorldPoint(new Vector3(1f, 1f, distance));

        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }

    public static bool IsOutside(Vector3 worldPosition, float padding = 0f)
    {
        EnsureInitialized();

        return worldPosition.x < minX - padding ||
               worldPosition.x > maxX + padding ||
               worldPosition.y < minY - padding ||
               worldPosition.y > maxY + padding;
    }

    public static Vector3 ClampPosition(Vector3 worldPosition, float padding = 0f)
    {
        EnsureInitialized();

        worldPosition.x = Mathf.Clamp(worldPosition.x, minX + padding, maxX - padding);
        worldPosition.y = Mathf.Clamp(worldPosition.y, minY + padding, maxY - padding);
        return worldPosition;
    }

    private static void EnsureInitialized()
    {
        if (initialized) return;
        Initialize(Camera.main, worldZ);
    }
}
