using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "Scriptable Objects/PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    public int hp;
    public int maxHp;
    public int exp;
    public int maxExp;
    public int level;
    public string sceneName;
}
