using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputEditor : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private GameObject[] inputLabels;
    private List<TextMeshProUGUI> textLabels = new ();
    private List<string> bindingValues = new();
    private string lastKey;
    private bool canEditKey;

    private Dictionary<string, string> rawNameToDisplay = new Dictionary<string, string>
    {
        {"upArrow", "Up arrow"},
        {"leftArrow", "Left arrow"},
        {"rightArrow", "Right arrow"},
        {"downArrow", "Down arrow"}
    };
    
    void Start()
    {
        foreach (var inputLabel in inputLabels)
        {
            textLabels.Add(inputLabel.GetComponent<TextMeshProUGUI>());
        }
        input.SwitchCurrentControlScheme("keyboard", InputSystem.GetDevice<Keyboard>());
        UpdateBindings();
    }
    private void UpdateBindings()
    {
        bindingValues.Clear();
        for (var i = 0; i < input.actions["Movement"].controls.Count; i++)
        {
            bindingValues.Add(input.actions["Movement"].controls[i].name);
        }
        bindingValues.Add(input.actions["Bonus"].controls[0].name);
        bindingValues.Add(input.actions["Drift"].controls[0].name);
        input.SwitchCurrentActionMap("Character");
        bindingValues.Add(input.actions["Jump"].controls[0].name);
        for (var i = 0; i < bindingValues.Count; i++)
        {
            textLabels[i].text = bindingValues[i];
        }
        input.SwitchCurrentActionMap("Vehicle");
    }

    public void EditBindingTrigger(string name)
    {
        canEditKey = true;
        InputSystem.onAnyButtonPress.CallOnce(control =>
        {
            Debug.Log("Path : " + control.path);
            if (!control.path.Contains("/Mouse"))
            {
                lastKey = (canEditKey ? control.path : lastKey);
                Debug.Log(lastKey);
                EditBinding(lastKey, name);
                UpdateBindings();
            }
        });
    }

    private void EditBinding(string controlName, string actionName)
    {
        switch (actionName)
        {
            case "accelerate":
                input.actions["Movement"].ChangeBinding("keyboard2DVector").NextBinding().WithPath(controlName);
                input.SwitchCurrentActionMap("Character");
                input.actions["Move"].ChangeBinding("keyboard2DVector").NextBinding().WithPath(controlName);
                break;
            case "brake":
                input.actions["Movement"].ChangeBinding("keyboard2DVector").NextBinding().NextBinding().WithPath(controlName);
                input.SwitchCurrentActionMap("Character");
                input.actions["Move"].ChangeBinding("keyboard2DVector").NextBinding().WithPath(controlName);
                break;
            case "left":
                input.actions["Movement"].ChangeBinding("keyboard2DVector").NextBinding().NextBinding().NextBinding().WithPath(controlName);
                input.SwitchCurrentActionMap("Character");
                input.actions["Move"].ChangeBinding("keyboard2DVector").NextBinding().WithPath(controlName);
                break;
            case "right":
                input.actions["Movement"].ChangeBinding("keyboard2DVector").NextBinding().NextBinding().NextBinding().NextBinding().WithPath(controlName);
                input.SwitchCurrentActionMap("Character");
                input.actions["Move"].ChangeBinding("keyboard2DVector").NextBinding().WithPath(controlName);
                break;
            case "Bonus":
                input.actions["Bonus"].ChangeBinding(0).WithPath(controlName);
                input.SwitchCurrentActionMap("Character");
                input.actions["Bonus"].ChangeBinding(0).WithPath(controlName);
                break;
            case "SpecialAction":
                input.actions["Drift"].ChangeBinding(0).WithPath(controlName);
                input.SwitchCurrentActionMap("Character");
                input.actions["Dash"].ChangeBinding(0).WithPath(controlName);
                break;
            case "Jump":
                input.SwitchCurrentActionMap("Character");
                input.actions["Jump"].ChangeBinding(0).WithPath(controlName);
                break;
        }
        input.SwitchCurrentActionMap("Vehicle");
        canEditKey = false;
    }
}
