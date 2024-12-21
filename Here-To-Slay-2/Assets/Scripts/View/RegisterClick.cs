using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Detects when card is clicked
/// </summary>
public class RegisterClick : MonoBehaviour
{
    [SerializeField]
    Button button;

    private void Start()
    {
        //probably subscribes EventSystem to this button's clicks
        button.onClick.AddListener(() =>
        {
            Debug.Log("Button clicked");
        });
        //removes "subscribers" to the event
        //button.onClick.RemoveAllListeners();
    }
}
