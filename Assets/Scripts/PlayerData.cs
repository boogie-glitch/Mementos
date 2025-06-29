using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int experience;
    public int maxExperience;
    public int health;
    public int maxHealth;
    //public int damage;

    public string sceneName;

    public PlayerData (int level, int experience, int maxExperience, int health, int maxHealth, /*int damage,*/ string sceneName)
    {
        this.level = level;
        this.experience = experience;
        this.maxExperience = maxExperience;
        this.health = health;
        this.maxHealth = maxHealth;
        //this.damage = damage;
        this.sceneName = sceneName;
    }
}
