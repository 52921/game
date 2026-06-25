using UnityEngine;
using System.Collections.Generic;

public class MissionSystem : MonoBehaviour
{
    [System.Serializable]
    public class Mission
    {
        public string missionName;
        public string description;
        public Vector3 missionLocation;
        public float completionReward = 100f;
        public bool isCompleted = false;
    }
    
    [SerializeField] private List<Mission> missions = new List<Mission>();
    private PlayerController assignedPlayer;
    private Mission currentMission;
    
    public void AssignMissionToPlayer(PlayerController player)
    {
        assignedPlayer = player;
        if (missions.Count > 0)
        {
            currentMission = missions[Random.Range(0, missions.Count)];
            Debug.Log($"Mission assigned to {player.GetPlayerName()}: {currentMission.missionName}");
        }
    }
    
    public void CompleteMission()
    {
        if (currentMission != null)
        {
            currentMission.isCompleted = true;
            Debug.Log($"Mission completed: {currentMission.missionName}");
            // Award player
        }
    }
    
    public Mission GetCurrentMission() => currentMission;
    public Vector3 GetMissionLocation() => currentMission != null ? currentMission.missionLocation : Vector3.zero;
}
