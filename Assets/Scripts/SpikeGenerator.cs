using UnityEngine;

[ExecuteAlways]
public class SpikeGenerator : MonoBehaviour
{
    [Header("Spike Prefabs")]
    public GameObject leftPrefab;
    public GameObject midPrefab;
    public GameObject rightPrefab;

    [Header("Settings")]
    public bool autoGenerate = false;

    private void OnValidate()
    {
        if (autoGenerate)
            Generate();
    }

    public void Generate()
    {
        if (leftPrefab == null || midPrefab == null || rightPrefab == null)
        {
            Debug.LogError("Podłącz wszystkie prefaby!");
            return;
        }

        // usuń poprzednie segmenty
        ClearChildren();

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box == null)
        {
            Debug.LogError("Brak BoxCollider2D!");
            return;
        }

        float totalWidth = box.size.x;

        float widthL = GetPolygonWidth(leftPrefab);
        float widthM = GetPolygonWidth(midPrefab);
        widthM -= 0.03f;               // Twój tweak szerokości środka
        float widthR = GetPolygonWidth(rightPrefab);

        float availableWidth = totalWidth - widthL - widthR;
        if (availableWidth < 0) availableWidth = 0;

        int midCount = Mathf.Max(0, Mathf.FloorToInt(availableWidth / widthM));

        // 🔹 początek na LEWEJ krawędzi BoxCollider2D (z uwzględnieniem offsetu)
        float startX = box.offset.x - box.size.x * 0.5f;

        // ====== LEFT ======
        GameObject left = Instantiate(leftPrefab, transform);
        left.transform.localPosition = new Vector3(startX, 1f, 0f);

        // ====== MID(S) ======
        for (int i = 0; i < midCount; i++)
        {
            GameObject mid = Instantiate(midPrefab, transform);

            // to jest dokładnie to co miałeś, tylko z dodanym startX
            float baseX = widthL + (i * widthM) + 0.71f;
            float x = startX + baseX - 0.05f;        // Twoje -0.05f
            float y = mid.transform.localPosition.y + 0.08f; // Twoje +0.08f

            mid.transform.localPosition = new Vector3(x, y, 0f);
        }

        // ====== RIGHT ======
        GameObject right = Instantiate(rightPrefab, transform);
        float rightBaseX = widthL + midCount * widthM;
        float rightX = startX + rightBaseX - 0.29f; // Twoje -0.29f
        right.transform.localPosition = new Vector3(rightX, 1f, 0f);
    }

    // ========= HELPERY ===========

    void ClearChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    float GetPolygonWidth(GameObject prefab)
    {
        PolygonCollider2D poly = prefab.GetComponent<PolygonCollider2D>();

        if (poly == null)
        {
            Debug.LogError(prefab.name + " nie ma PolygonCollider2D!");
            return 1;
        }

        Vector2[] pts = poly.points;

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        foreach (Vector2 p in pts)
        {
            if (p.x < minX) minX = p.x;
            if (p.x > maxX) maxX = p.x;
        }

        return maxX - minX;
    }
}
