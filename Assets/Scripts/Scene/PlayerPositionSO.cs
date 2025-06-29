using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPositionSO", menuName = "Scriptable Objects/PlayerPositionSO")]
public class PlayerPositionSO : ScriptableObject
{
    public Vector3 lastPlayerPosition = Vector3.zero;
    public bool isGoBack = false;
}
