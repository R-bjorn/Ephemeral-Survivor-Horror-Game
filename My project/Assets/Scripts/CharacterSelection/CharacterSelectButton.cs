using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject disabledOverlay;
    [SerializeField] private Button button;

    private CharacterSelectDisplay characterSelect;

    public Character Character { get; private set; }
    public bool IsDisabled { get; private set; }

    public void SetCharacter(CharacterSelectDisplay characterSelect, Character character)
    {
        iconImage.sprite = character.Icon;
        Debug.Log("Test");

        this.characterSelect = characterSelect;

        Character = character;
    }

    public void SelectCharacter()
    {
        Debug.Log("Tes2t");
        characterSelect.Select(Character);
    }

    public void SetDisabled()
    {
        IsDisabled = true;
        disabledOverlay.SetActive(true);
        button.interactable = false;
    }
}
