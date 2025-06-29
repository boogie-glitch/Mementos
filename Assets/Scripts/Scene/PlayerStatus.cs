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

    public void ResetStatus()
    {
        maxHp = 100; // Set your desired max HP
        hp = maxHp;
        exp = 0;
        maxExp = 100; // Set your desired max EXP
        level = 1;
        sceneName = "Level1"; // Replace with your default scene name
    }
}
