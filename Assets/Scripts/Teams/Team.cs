using UnityEngine;
using System.Collections.Generic;

public class Team : MonoBehaviour
{
    [SerializeField] private string teamName;
    [SerializeField] private Color teamColor;
    [SerializeField] private Material teamMaterial;
    
    private List<PlayerController> activePlayers = new List<PlayerController>();
    private int targetKills = 0;
    private bool hasWon = false;
    
    public void Initialize(string name, Color color)
    {
        teamName = name;
        teamColor = color;
    }
    
    public void AddPlayer(PlayerController player)
    {
        if (!activePlayers.Contains(player))
        {
            activePlayers.Add(player);
        }
    }
    
    public void RemovePlayer(PlayerController player)
    {
        activePlayers.Remove(player);
        
        // Check if team has lost
        if (activePlayers.Count == 0)
        {
            OnTeamEliminated();
        }
    }
    
    public int GetAlivePlayerCount()
    {
        return activePlayers.FindAll(p => p != null && p.gameObject.activeInHierarchy).Count;
    }
    
    public List<PlayerController> GetActivePlayers() => activePlayers;
    public string GetTeamName() => teamName;
    public Color GetTeamColor() => teamColor;
    public Material GetTeamMaterial() => teamMaterial;
    
    private void OnTeamEliminated()
    {
        Debug.Log($"Team {teamName} has been eliminated!");
    }
    
    public void WinMatch()
    {
        hasWon = true;
        Debug.Log($"Team {teamName} wins the match!");
    }
}
