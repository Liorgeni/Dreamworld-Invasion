using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    [SerializeField] private GameObject coinsGroup;
    [SerializeField] private Vector3[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int coinNo;
    public TextMeshProUGUI coinsText; // Reference to the UI Text element for displaying coins

    private int coinsCount = 0;
    private int coinsToReward = 0; // Total coins to reward for this instance

    // Set the required position for the top-right
    [SerializeField] private Vector3 targetWorldPosition = new Vector3(3623f, 1997f, -514.6f); // Your desired coordinates

    public void Start()
    {
        initialPos = new Vector3[coinNo];
        initialRotation = new Quaternion[coinNo];

        for (int i = 0; i < coinsGroup.transform.childCount; i++)
        {
            initialPos[i] = coinsGroup.transform.GetChild(i).position;
            initialRotation[i] = coinsGroup.transform.GetChild(i).rotation;
        }
    }

    public void ResetCoins()
    {
        Debug.Log("Resetting Coins");

        for (int i = 0; i < coinsGroup.transform.childCount; i++)
        {
            Transform coin = coinsGroup.transform.GetChild(i);
            coin.position = initialPos[i];

            // Apply a random Z-axis rotation
            Quaternion randomRotation = Quaternion.Euler(initialRotation[i].eulerAngles.x, initialRotation[i].eulerAngles.y, Random.Range(0f, 360f));
            coin.rotation = randomRotation;

            coin.gameObject.SetActive(true);  // Reactivate the coin for the animation
        }
    }

    public void RewardCoin(int coinNo)
    {
        coinsToReward = coinNo; // Track how many coins to reward
        ResetCoins(); // Reset coin positions
        coinsGroup.SetActive(true); // Make sure coins are visible

        float delay = 0.1f; // Delay between each coin's animation

        for (int i = 0; i < coinsGroup.transform.childCount; i++)
        {
            Transform coin = coinsGroup.transform.GetChild(i);

            // Move each coin to the manually set top-right position
            coin.DOMove(targetWorldPosition, 1f)
                .SetEase(Ease.OutBack)
                .SetDelay(delay)
                .OnComplete(() => {
                    // Disable the coin after the movement is done
                    coin.gameObject.SetActive(false);
                    IncrementCoins(); // Increment coins and update UI gradually
                });

            delay += 0.1f; // Increase the delay for each subsequent coin
        }
    }

    private void IncrementCoins()
    {
        // Increment the total coin count with animation
        int targetCount = coinsCount + 1;
        DOTween.To(() => coinsCount, x => coinsCount = x, targetCount, 0.2f).OnUpdate(UpdateCoinsUI);

        coinsCount = targetCount;
    }

    private void UpdateCoinsUI()
    {
        // Gradually update the displayed coins text
        coinsText.text = coinsCount.ToString();
    }
}
