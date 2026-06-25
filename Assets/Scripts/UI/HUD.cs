using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI teamText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killsText;
    
    private PlayerController currentPlayer;
    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    
    private void Update()
    {
        if (currentPlayer != null)
        {
            UpdateHUD();
        }
    }
    
    private void UpdateHUD()
    {
        // Update player info
        if (playerNameText != null)
            playerNameText.text = currentPlayer.GetPlayerName();
        
        if (healthText != null)
            healthText.text = $"Health: {currentPlayer.GetHealth()}"; // Needs GetHealth method
        
        if (teamText != null)
            teamText.text = $"Team: {currentPlayer.GetTeam().GetTeamName()}";
        
        // Update timer
        if (timerText != null)
        {
            float remainingTime = 300f - gameManager.GetMatchTimer();
            int minutes = (int)(remainingTime / 60);
            int seconds = (int)(remainingTime % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }
    
    public void SetCurrentPlayer(PlayerController player)
    {
        currentPlayer = player;
    }
}
