using UnityEngine;

public class SaveLoadUI : MonoBehaviour
{
    public Transform content;
    public GameObject saveSlotPrefab;

    public int slotCount = 5;

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(saveSlotPrefab, content);

            SaveSlotUI ui = slot.GetComponent<SaveSlotUI>();

            if (ui != null)
                ui.Setup(i);
        }
    }
}