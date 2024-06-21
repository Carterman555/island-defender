using IslandDefender.Units;
using IslandDefender;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers
{
    /// <code>
    /// transform.DestroyChildren();
    /// </code>
    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) UnityEngine.Object.Destroy(child.gameObject);
    }

    public static void SetActiveChildren(this Transform t, bool active)
    {
        foreach (Transform child in t) child.gameObject.SetActive(active);
    }

    public static void Fade(this SpriteRenderer spriteRenderer, float value)
    {
        Color color = spriteRenderer.color;
        color.a = value;
        spriteRenderer.color = color;
    }
    public static void Fade(this Image image, float value)
    {
        Color color = image.color;
        color.a = value;
        image.color = color;
    }
    public static void Fade(this TextMeshPro text, float value) {
        Color color = text.color;
        color.a = value;
        text.color = color;
    }


    public static void ChangeHue(this SpriteRenderer spriteRenderer, Color targetColor, float amount) {

        float alpha = spriteRenderer.color.a;

        // Convert the original color and target color to HSV
        Color.RGBToHSV(spriteRenderer.color, out float h1, out float s1, out float v1);
        Color.RGBToHSV(targetColor, out float h2, out float s2, out float v2);

        // Calculate the shortest direction towards the target hue
        float diff = Mathf.DeltaAngle(h1 * 360f, h2 * 360f) / 360f;

        // Adjust the hue by the specified amount towards the target hue
        float newHue = h1 + diff * amount;
        if (newHue < 0) newHue += 1;
        if (newHue > 1) newHue -= 1;

        // Convert the new HSV color back to RGB
        Color newColor = Color.HSVToRGB(newHue, s1, v1);
        newColor.a = alpha;
        spriteRenderer.color = newColor;
    }

    public static void RemoveWithCheck<T>(this List<T> list, T item) {
        if (list.Contains(item)) list.Remove(item);
    }

    public static T RandomItem<T>(this T[] list) {
        int randomIndex = UnityEngine.Random.Range(0, list.Length);
        return list[randomIndex];
    }
    public static T RandomItem<T>(this List<T> list) {
        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static bool IsMouseOverLayer(int layerMask)
    {
        // Cast a ray from the camera to the mouse position and check if null
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << layerMask).collider != null;
    }

    public static Quaternion GetRotation(Vector3 current, Vector3 target, float offset)
    {
        float _xDiff = current.x - target.x;
        float _yDiff = current.y - target.y;

        float _radians = Mathf.Atan2(_yDiff, _xDiff);
        float _degrees = _radians * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, _degrees + offset);
    }

    public static Vector3 RotateDirection(Vector3 direction, float degrees)
    {
        return Quaternion.Euler(0, 0, degrees) * direction;
    }

    public static float GetClosestDirection(Vector3 dir1, Vector3 dir2)
    {
        float _angle = Vector2.SignedAngle(dir1, dir2);
        return -Mathf.Sign(_angle);
    }

    public static float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    public static Transform GetClosest(this Transform self, Transform[] objects)
    {
        float closestDistance = float.PositiveInfinity;
        Transform closestOb = null;
        foreach (Transform ob in objects)
        {
            float distance = Vector3.Distance(self.position, ob.position);

            if (ob != self && distance < closestDistance)
            {
                closestOb = ob;
                closestDistance = distance;
            }
        }

        return closestOb;
    }

    public static int FacingInt(Vector3 current, Vector3 target) => (int)Mathf.Sign(target.x - current.x);

    public static Vector3 WorldToCanvasPosition(Canvas canvas, Vector3 worldPos)
    {
        RectTransform _canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 _viewportPosition = Camera.main.WorldToViewportPoint(worldPos);
        return new Vector2(
        (_viewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f),
        (_viewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f));
    }

    public static bool IsMouseOverUI()
    {
        if (EventSystem.current == null) EventSystem.current = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule)).GetComponent<EventSystem>();

        PointerEventData pointerEventData = new(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

        for (int i = 0; i < raycastResultList.Count; i++)
        {
            int UILayer = 5;
            if (raycastResultList[i].gameObject.layer == UILayer) return true;
        }
        return false;
    }

    public static string ToPrettyString(this Enum value)
    {
        string original = value.ToString();
        return Regex.Replace(original, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1");
    }

    public static bool ContainsLayer(this LayerMask mask, int layer) {
        return (mask.value & (1 << layer)) != 0;
    }

    // Game specific
    //public static bool IsCollisionWithFriendly(out IDamagable damagable, Faction faction, GameObject other) {

    //    if (!other.TryGetComponent(out UnitBase otherUnit)) {
    //        damagable = null;
    //        return false;
    //    }

    //    if (faction == otherUnit.Faction) {
    //        damagable = otherUnit.GetComponent<IDamagable>();
    //        return true;
    //    }
    //    else {
    //        damagable = null;
    //        return false;
    //    }
    //}

    //public static bool IsCollisionWithOpposite(out IDamagable damagable, Faction faction, GameObject other) {

    //    if (!other.TryGetComponent(out UnitBase otherUnit)) {
    //        damagable = null;
    //        return false;
    //    }

    //    bool collisionWithOpposite = !IsCollisionWithFriendly(out damagable, faction, other);
    //    return collisionWithOpposite;
    //}
}
