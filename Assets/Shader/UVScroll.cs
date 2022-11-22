using UnityEngine;

public class UVScroll : MonoBehaviour
{
    [SerializeField]
    private Material _targetMaterial;

    [SerializeField]
    private float _scrollX;
    [SerializeField]
    private float _scrollY;

    private Vector2 offset;
    private void Awake()
    {
        offset = _targetMaterial.mainTextureOffset;
    }

    private void Update()
    {
        offset.x += _scrollX * Time.unscaledDeltaTime;
        offset.y += _scrollY * Time.unscaledDeltaTime;
        _targetMaterial.mainTextureOffset = offset;
    }
}