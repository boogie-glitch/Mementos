using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int _maxHp = 100;
    [SerializeField, Range(0, 100)]
    private int _hp;
    [SerializeField]
    private Animator animator;
    [SerializeField] private PlayerStatus playerStatus; // Kéo asset vào Inspector
    [SerializeField] private Loading loading; // Kéo asset vào Inspector
    
    public int MaxHp
    {
        get => _maxHp;
        set => _maxHp = value; // Thêm setter này để có thể gán MaxHp từ ngoài
    }
    public int Hp
    {
        get => _hp;
        set
        {
            var isDamage = value < _hp;
            _hp = Mathf.Clamp(value, 0, _maxHp);

            if (isDamage)
            {
                Damaged?.Invoke(_hp);
            }
            else
            {
                Healed?.Invoke(_hp);
            }

            if (_hp <= 0)
            {
                animator.SetTrigger("Died");
            }
        }
    }

    public UnityEvent<int> Healed;
    public UnityEvent<int> Damaged;
    public UnityEvent Died;

    private void Awake() 
    {
        _hp = _maxHp;

        if (playerStatus != null && playerStatus.maxHp > 0)
        {
            _maxHp = playerStatus.maxHp;
            _hp = playerStatus.hp;
        }
        else if (playerStatus != null)
        {
            playerStatus.maxHp = _maxHp;
            playerStatus.hp = _hp;
        }        
    }

    private void Start()
    {
        Damaged?.Invoke(_hp);
    }

    public void Damage(int amount)
    {
        Hp -= amount;

        if(playerStatus != null)
        {   
            playerStatus.hp = _hp;
            playerStatus.maxHp = _maxHp;
        }
    }
    public void Heal(int amount)
    {
        Hp += amount;

        if (playerStatus != null)
        {
            playerStatus.hp = _hp;
            playerStatus.maxHp = _maxHp;
        }
    }
    public void HealFull() => Hp = _maxHp;

    public void Kill() => Hp = 0;

    public void Adjust(int value) => Hp = value;

    void OnDead()
    {
        Died?.Invoke();
        playerStatus.hp = playerStatus.maxHp; // Reset HP to max when dead
        loading.LoadLevelBtn(playerStatus.sceneName); // Load the scene
    }
}
