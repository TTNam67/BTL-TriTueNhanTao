using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _hoverImage;
    void Start()
    {
        _hoverImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hoverImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hoverImage.SetActive(false);
    }
}
