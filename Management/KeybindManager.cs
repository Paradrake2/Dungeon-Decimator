using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    public Combat combat;
    public KeyCode[] keybinds = new KeyCode[5]
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5
    };
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keybinds[0]))
        {
            Hotbar.Instance.selectedIndex = 0;
            Debug.LogWarning("PRESSED 1");
            combat.ChangeSelectedSlotIndex(0);
        }
        if (Input.GetKeyDown(keybinds[1]))
        {
            Hotbar.Instance.selectedIndex = 1;
            combat.ChangeSelectedSlotIndex(1);
        }
        if (Input.GetKeyDown(keybinds[2]))
        {
            Hotbar.Instance.selectedIndex = 2;
            combat.ChangeSelectedSlotIndex(2);
        }
        if (Input.GetKeyDown(keybinds[3]))
        {
            Hotbar.Instance.selectedIndex = 3;
            combat.ChangeSelectedSlotIndex(3);
        }
        if (Input.GetKeyDown(keybinds[4]))
        {
            Hotbar.Instance.selectedIndex = 4;
            combat.ChangeSelectedSlotIndex(4);
        }
    }
}
