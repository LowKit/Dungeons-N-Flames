using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    [SerializeField] private List<DungeonRoom> rooms = new List<DungeonRoom>();
    [SerializeField] private DungeonRoom defaultRoom;

    List<float> probabilities = new List<float>();


    PlayerController playerController;
    DungeonRoom currentRoom;
    private void Awake()
    {
        instance = this;

        playerController = FindAnyObjectByType<PlayerController>();

        probabilities = rooms.Where(r => r != null).Select(r => (float)r.roomProbability).ToList();

        if (defaultRoom != null) EnterRoom(defaultRoom);
    }

    private void EnterRoom(DungeonRoom room)
    {
        playerController.WarpTo(room.GetPlayerSpawnPosition());

        currentRoom = room;
        currentRoom.InitializeRoom();
    }

    public void EnterRandomRoom()
    {
        DungeonRoom room = GetRandomRoom();
        if (room == null)
        {
            Debug.Log("[DungeonManager] Can't enter room (room is null)");
            return;
        }

        if (currentRoom == null || currentRoom.CanLeave())
        {
            currentRoom.CleanRoom();
            EnterRoom(room);
        }
        else
        {
            Debug.Log("[DungeonManager] Cannot leave the current room yet.");
        }
    }

    private DungeonRoom GetRandomRoom()
    {
        if (rooms.Count < 1)
        {
            Debug.LogError("[DungeonManager] There are no rooms assigned to the list: " + rooms.Count);
            return null;
        }

        int index = ProbabilitiesController.GetItemByProbabilityRarity(probabilities);

        if (index < 0 || index >= rooms.Count)
        {
            Debug.LogError("[DungeonManager] Invalid room index selected.");
            return null;
        }

        return rooms[index];
    }
}
