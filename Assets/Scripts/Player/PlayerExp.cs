using UnityEngine;
using UnityEngine.Events;

public class PlayerExp : MonoBehaviour
{
    [SerializeField]
    private int _maxExp = 100;
    [SerializeField, Min(0)]
    private int _exp;
    [SerializeField]
    private int _level = 1;
    [SerializeField]
    private int _expIncreasePerLevel = 50;
    [SerializeField]
    private PlayerStatus playerStatus;

    public int MaxExp => _maxExp;
    public int Exp
    {
        get => _exp;
        private set
        {
            var isGain = value > _exp;
            _exp = Mathf.Clamp(value, 0, _maxExp);

            if (isGain)
            {
                Gained?.Invoke(_exp);
            }
            else
            {
                Lost?.Invoke(_exp);
            }

            if (_exp >= _maxExp)
            {
                LevelUp();
            }
        }
    }

    public int Level => _level;

    public UnityEvent<int> Gained;
    public UnityEvent<int> Lost;
    public UnityEvent<int> LeveledUp;

    private void Awake()
    {
        if(playerStatus != null && playerStatus.maxExp > 0)
        {
            _maxExp = playerStatus.maxExp;
            _exp = playerStatus.exp;
            _level = playerStatus.level;
        }
        else if(playerStatus != null)
        {
            playerStatus.maxExp = _maxExp;
            playerStatus.exp = _exp;
            playerStatus.level = _level;
        }
    }

    private void Start()
    {
        Lost?.Invoke(_exp);
    }

    public void Gain(int amount)
    {
        Exp += amount;
        
        if (playerStatus != null)
        {
            playerStatus.exp = _exp;
            playerStatus.maxExp = _maxExp;
            playerStatus.level = _level;
        }
    }

    public void Lose(int amount)
    {
        Exp -= amount;

        if (playerStatus != null)
        {
            playerStatus.exp = _exp;
            playerStatus.maxExp = _maxExp;
            playerStatus.level = _level;
        }
    }

    public void SetExp(int value)
    {
        Exp = value;

        if (playerStatus != null)
        {
            playerStatus.exp = _exp;
            playerStatus.maxExp = _maxExp;
            playerStatus.level = _level;
        }
    }

    public void SetLevel(int level)
    {
        if (level < 1) return;
        _level = level;
        _exp = 0;
        _maxExp = _expIncreasePerLevel * (_level - 1) + 100; // Assuming the first level starts with 100 exp
    
        if (playerStatus != null)
        {
            playerStatus.exp = _exp;
            playerStatus.maxExp = _maxExp;
            playerStatus.level = _level;
        }
    }

    private void LevelUp()
    {
        _level++;
        SetExp(_exp - _maxExp);
        _maxExp += _expIncreasePerLevel;
        LeveledUp?.Invoke(_level);

        if (playerStatus != null)
        {
            playerStatus.exp = _exp;
            playerStatus.maxExp = _maxExp;
            playerStatus.level = _level;
        }
    }
}