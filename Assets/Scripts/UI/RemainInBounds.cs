using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainInBounds : MonoBehaviour
{
    [SerializeField] private Vector3 boundsSize;

    private RectTransform rectTransform;
    private Bounds bounds;

    private void Awake()
    {
        bounds = new Bounds(new Vector3(boundsSize.x, -boundsSize.y, 0f) / 2f, boundsSize);
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!bounds.Contains(rectTransform.anchoredPosition))
        {
            rectTransform.anchoredPosition = bounds.ClosestPoint(rectTransform.anchoredPosition);
        }
    }
}
