using UnityEngine;

public class RestingRoom : MonoBehaviour
{
    [SerializeField] private DungeonRoom dungeonRoom;
    [SerializeField] private RecoverHealth recoverHealth;
    [SerializeField] private GameObject healthStatue;
    [SerializeField] private GameObject anvil;

    [SerializeField] private int maxStatueChance = 20;
    [SerializeField] private int maxAnvilChance = 20;
    private void OnEnable()
    {
        dungeonRoom.OnRoomInitialized += OnRoomInitialized;
    }

    private void OnDisable()
    {
        dungeonRoom.OnRoomInitialized -= OnRoomInitialized;
    }

    private void OnRoomInitialized()
    {
        EnableBed();

        bool statueEnabled = Random.Range(0, maxStatueChance) <= 1;
        bool anvilEnabled = Random.Range(0, maxAnvilChance) <= 1;

        healthStatue.SetActive(statueEnabled);
        anvil.SetActive(anvilEnabled);
    }

    private void EnableBed()
    {
        recoverHealth.ToggleInteractable(true);
    }
}
