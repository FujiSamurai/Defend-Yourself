using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldText = null;

    private void Start()
    {
        Player.OnGoldCollected += SetGoldText;
    }

    public void SetGoldText(int goldAmount)
    {
        _goldText.text = goldAmount.ToString();
    }
}
