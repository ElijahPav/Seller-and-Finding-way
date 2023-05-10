using TMPro;
using UnityEngine;

public class WalletController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;


    private int amount;

    private void Start()
    {
        amount = 20;
        amountText.text = amount.ToString();
    }

    public void AddCurrency(int value)
    {
        if (value > 0)
        {
            amount += value;
            amountText.text = amount.ToString();
        }
    }
    public bool TrySubtract(int value)
    {
        if (value > 0 && amount >= value)
        {
            amount -= value;
            amountText.text = amount.ToString();
            return true;
        }
        return false;
    }
}

