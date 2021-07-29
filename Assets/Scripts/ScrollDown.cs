using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollDown : MonoBehaviour
{
    public float speed;
    public float timeToDestroy;

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        Invoke(nameof(RemoveFromGame), timeToDestroy);
    }

    private void Update()
    {
        _rectTransform.position += speed * Time.deltaTime * Vector3.down;
    }

    private void RemoveFromGame()
    {
        Destroy(gameObject);
    }
}
