using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private int playersPerTeam = 6;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] team1Spawns;
    [SerializeField] private Transform[] team2Spawns;
    
    private Team team1;
    private Team team2;
    private List<PlayerController> allPlayers = new List<PlayerController>();
    private GameState gameState = GameState.Setup;
    private float matchTimer = 0f;
    private const float MATCH_DURATION = 300f; // 5 minutes
    
    public enum GameState { Setup, Playing, Ended }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        InitializeGame();
    }
    
    private void Update()
    {
        if (gameState == GameState.Playing)
        {
            matchTimer += Time.deltaTime;
            if (matchTimer >= MATCH_DURATION)
            {
                EndMatch();
            }
            
            CheckVictoryCondition();
        }
    }
    
    public void InitializeGame()
    {
        CreateTeams();
        SpawnPlayers();
        gameState = GameState.Playing;
    }
    
    private void CreateTeams()
    {
        GameObject team1Obj = new GameObject("Team1_Red");
        team1 = team1Obj.AddComponent<Team>();
        team1.Initialize("Red Team", Color.red);
        
        GameObject team2Obj = new GameObject("Team2_Blue");
        team2 = team2Obj.AddComponent<Team>();
        team2.Initialize("Blue Team", Color.blue);
    }
    
    private void SpawnPlayers()
    {
        // Spawn Team 1
        for (int i = 0; i < playersPerTeam && i < team1Spawns.Length; i++)
        {
            SpawnPlayerAtTeam(team1, team1Spawns[i], i);
        }
        
        // Spawn Team 2
        for (int i = 0; i < playersPerTeam && i < team2Spawns.Length; i++)
        {
            SpawnPlayerAtTeam(team2, team2Spawns[i], i);
        }
    }
    
    private void SpawnPlayerAtTeam(Team team, Transform spawnPoint, int playerIndex)
    {
        GameObject playerObj = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        PlayerController player = playerObj.GetComponent<PlayerController>();
        
        string playerName = $"Player_{playerIndex + 1}";
        player.SetupPlayer(playerName, team);
        team.AddPlayer(player);
        allPlayers.Add(player);
        
        // Equip weapon
        GivePlayerWeapon(player);
    }
    
    private void GivePlayerWeapon(PlayerController player)
    {
        // Create weapon prefab or use existing
        GameObject weaponObj = new GameObject("Weapon_AR15");
        Weapon weapon = weaponObj.AddComponent<Weapon>();
        player.EquipWeapon(weapon);
    }
    
    private void CheckVictoryCondition()
    {
        if (team1.GetAlivePlayerCount() == 0)
        {
            team2.WinMatch();
            EndMatch();
        }
        else if (team2.GetAlivePlayerCount() == 0)
        {
            team1.WinMatch();
            EndMatch();
        }
    }
    
    public void EndMatch()
    {
        gameState = GameState.Ended;
        Debug.Log("Match Ended!");
        // Show results UI
    }
    
    public Team GetTeam1() => team1;
    public Team GetTeam2() => team2;
    public float GetMatchTimer() => matchTimer;
    public GameState GetGameState() => gameState;
}
