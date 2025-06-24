using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int _maxHp = 100;
    [SerializeField, Range(0, 100)]
    private int _hp;
    [SerializeField]
    private Animator animator;

    public int MaxHp => _maxHp;

    public int Hp
    {
        get => _hp;
        private set
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
                Debug.Log("Destroy called");
                animator.SetTrigger("Died");
                Died?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    public UnityEvent<int> Healed;
    public UnityEvent<int> Damaged;
    public UnityEvent Died;

    private void Awake() => _hp = _maxHp;

    public void Damage(int amount) => Hp -= amount;

    public void Heal(int amount) => Hp += amount;

    public void HealFull() => Hp = _maxHp;

    public void Kill()
    {
 Debug.Log("Kill() called");
        {
            if (_hp > 0)
                Hp = 0;
            else
                Hp = -1; // ép setter chạy lại
        }
    }
    //   => Hp = 0;

    public void Adjust(int value) => Hp = value;

    public void OnDead() => Died?.Invoke();
}
