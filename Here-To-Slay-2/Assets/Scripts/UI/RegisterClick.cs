using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RegisterClick : MonoBehaviour
{
    [SerializeField]
    Button button;

    public static RegisterClick _instance;

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        button.onClick.AddListener(() =>
        {
            Debug.Log("Button clicked");
        });
        button.onClick.RemoveAllListeners();
    }

    public void OnClick()
    {
        Debug.Log("Button clicked function");
    }
}
