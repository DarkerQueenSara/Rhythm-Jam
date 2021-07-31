using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrowOnHoverUI : MonoBehaviour
{     
    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(GetComponent<RectTransform>(), new Vector3(1.5f, 1.5f, 1f), 0.2f);
    }
}
