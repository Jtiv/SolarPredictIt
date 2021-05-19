using UnityEngine;
using TMPro;


public class UICurrentlyTracking : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Text;

    void Update()
    {
        int[] array = MarketHandler.instance._inputFieldList.ToArray();
        string newText = string.Join("\n", array);
        Text.text = newText;
    }
}
