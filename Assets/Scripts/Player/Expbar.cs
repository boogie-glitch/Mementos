using UnityEngine;
using UnityEngine.UI;

public class Expbar : MonoBehaviour
{
    [SerializeField] private PlayerExp _playerExp;
    [SerializeField] private RectTransform _barRect;
    [SerializeField] private RectMask2D _mask;

    private float _maxTopMask;
    private float _initialTopMask;

    private void Start()
    {
        // x = left, y = bottom, z = right, w = top
        _maxTopMask = _barRect.rect.height - _mask.padding.y - _mask.padding.w;
        _initialTopMask = _mask.padding.w;
    }
    public void SetValue(int newValue)
    {
        var targetHeight = newValue * _maxTopMask / _playerExp.MaxExp;
        var newTopMask = _maxTopMask + _initialTopMask - targetHeight;
        var padding = _mask.padding;
        padding.w = newTopMask;
        _mask.padding = padding;
    }
}
