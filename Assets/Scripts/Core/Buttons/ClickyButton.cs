using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _default, _pressed;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private ButtonResourcesList _buttonResourcesList;
    [SerializeField] private int _defaultSpriteID, _pressedSpriteID;
    
    RectTransform _rectTransform;
    float _changeY = 5.6f;

    private void Awake() 
    {
        _image = GetComponent<Image>();
        if (_image == null)
            Debug.LogWarning("ClickyButton.cs: _image is null");

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogWarning("ClickyButton.cs: _audioSource is null");

        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null)
        {
            Debug.LogWarning("ClickyButton.cs: _rectTransform is null");
        }
        
        _default = _buttonResourcesList._defaultButtonSprites[_defaultSpriteID];
        _pressed = _buttonResourcesList._pressedButtonSprites[_pressedSpriteID];

        _image.sprite = _default;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _image.sprite = _default;
        AudioManager.Instance.Play(AudioEnum.UncompressedButton);
        // _audioSource.PlayOneShot(_uncompressedClip);

        Vector2 anchoredPosition = _rectTransform.anchoredPosition;

        // Modify the Y component to the new value
        anchoredPosition.y += _changeY;

        // Assign the modified anchored position back to the RectTransform
        _rectTransform.anchoredPosition = anchoredPosition;
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _image.sprite = _pressed;
        
        AudioManager.Instance.Play(AudioEnum.CompressedButton);
        // _audioSource.PlayOneShot(_compressedClip);
        Vector2 anchoredPosition = _rectTransform.anchoredPosition;

        // Modify the Y component to the new value
        anchoredPosition.y -= _changeY;

        // Assign the modified anchored position back to the RectTransform
        _rectTransform.anchoredPosition = anchoredPosition;
        
        
    }
}

