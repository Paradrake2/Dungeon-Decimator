using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RecipeMaterialSlot : MonoBehaviour
{
    public Image slotImage;
    public Image materialImage;
    public Button slotButton;
    public Image overlayImage;
    
    public List<CraftingMaterialTag> acceptedTags = new List<CraftingMaterialTag>();
    public bool isTagBased;


    public Sprite defaultIcon;
    [SerializeField] private CraftingMaterial currentMaterial;
    public bool isOccupied = false;
    public CraftingMaterial specificMaterial;


    public void SetupTagBasedSlot(CraftingMaterialTag tags)
    {
        acceptedTags.Add(tags);
        isTagBased = true;
        slotImage.sprite = defaultIcon;
        UpdateSlotUI(acceptedTags, null);
    }

    public void SetupSpecificMaterialSlot(CraftingMaterial material)
    {
        specificMaterial = material;
        isTagBased = false;
        overlayImage.gameObject.SetActive(true);
        UpdateSlotUI(null, material);
    }

    public bool CanAcceptMaterial(CraftingMaterial material)
    {
        if (isOccupied)
        {
            Debug.Log("Slot is occupied");
            return false;
        }
        if (isTagBased)
        {
            foreach (var tag in acceptedTags)
            {
                if (material.tags.Contains(tag))
                {
                    Debug.Log("true");
                    return true;
                }
            }
        }
        else
        {
            return material == specificMaterial;
        }
        Debug.Log("cannot accept material");
        return false;
    }
    public CraftingMaterial GetPlacedMaterial()
    {
        return currentMaterial;
    }
    public void RemoveMaterial()
    {
        Debug.Log("Attempting to remove material");
        currentMaterial = null;
        isOccupied = false;
        UpdateSlotUI();
    }
    public void PlaceMaterialInSlot(CraftingMaterial material)
    {
        StopCoroutine(LoadTagIcons(acceptedTags));
        if (CanAcceptMaterial(material))
        {
            currentMaterial = material;
            isOccupied = true;
            materialImage.sprite = material.icon;
        }
    }
    public void CheckIfOccupied() 
    {
        if (isOccupied)
        {
            overlayImage.gameObject.SetActive(false);
        } else
        {
            overlayImage.gameObject.SetActive(true);
        }
    }
    public void UpdateSlotUI(List<CraftingMaterialTag> tags = null, CraftingMaterial material = null)
    {
        if (tags != null)
        {
            //StartCoroutine(LoadTagIcons(tags));
        }
        else if (material != null)
        {
            materialImage.sprite = material.icon;
            CheckIfOccupied();
        }
    }

    IEnumerator LoadTagIcons(List<CraftingMaterialTag> tags)
    {
        while (!isOccupied)
        {
            if (tags.Count == 0)
            {
                slotImage.sprite = defaultIcon;
                yield break;
            }
            foreach (var tag in tags)
            {
                yield return new WaitForSeconds(0.5f);
                UpdateTagIconUI(tag.icon);
            }
        }
    }
    void UpdateTagIconUI(Sprite icon)
    {
        slotImage.sprite = icon;
    }
    void Start()
    {
        if (slotButton != null)
        {
            slotButton.onClick.AddListener(OnSlotClicked);
        }
    }
    void OnSlotClicked()
    {
        Debug.Log("RecipeMaterialSlot: Slot clicked");
        // Implement logic for when the slot is clicked (e.g., remove material)
        if (isOccupied)
        {
            Debug.Log($"RecipeMaterialSlot: Removing material {currentMaterial.materialName} from slot");
            int slotIndex = transform.GetSiblingIndex();
            CraftingManager.Instance.RemoveMaterialFromRecipe(slotIndex);
            currentMaterial = null;
            isOccupied = false;
            materialImage.sprite = null;
            if (isTagBased)
            {
                //StartCoroutine(LoadTagIcons(acceptedTags));
            }
            else if (specificMaterial != null)
            {
                materialImage.sprite = specificMaterial.icon;
            }
        }
    }
}
