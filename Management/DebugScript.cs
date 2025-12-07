using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public GameObject hotbarObject;
    public void SetSelectedSlot(int index)
    {
        Hotbar.Instance.selectedIndex = index;
        Combat playerCombat = FindFirstObjectByType<Combat>();
        playerCombat.ChangeSelectedSlotIndex(index);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
