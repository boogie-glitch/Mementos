using TMPro;
using UnityEngine;

public class Levelbar : MonoBehaviour
{
    [SerializeField] private PlayerExp _playerExp;
    [SerializeField] private TMP_Text _levelText;

    private void Start()
    {
        _levelText.SetText($"{_playerExp.Level}");
    }

    private void Update()
    {
        if (_playerExp.Level != int.Parse(_levelText.text))
        {
            _levelText.SetText($"{_playerExp.Level}");
        }
    }
}
