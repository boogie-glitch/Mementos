using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private EnemyHealth _health;
    [SerializeField] private RectTransform _barRect;
    [SerializeField] private RectMask2D _mask;

    private float _maxLeftMask;
    private float _initialLeftMask;

    private void Start()
    {
        //x = left, y = bottom, z = right, w = top
        _maxLeftMask = _barRect.rect.width - _mask.padding.z - _mask.padding.x;
        _initialLeftMask = _mask.padding.x;
    }

    public void SetValue(int newValue)
    {
        var targetWidth = newValue * _maxLeftMask / _health.MaxHp;
        var newLeftMask = _maxLeftMask + _initialLeftMask - targetWidth;
        var padding = _mask.padding;
        padding.x = newLeftMask;
        _mask.padding = padding;
    }
}