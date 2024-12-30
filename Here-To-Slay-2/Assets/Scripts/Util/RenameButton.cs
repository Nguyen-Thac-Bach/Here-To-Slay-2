using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Util
{
    public class RenameButton: MonoBehaviour
    {
        public string newName;
        public Button button;
        void Start()
        {

        }
        void Update()
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = newName;
            Debug.Log("Button name changed to: " + newName);
            this.enabled = false;
        }
    }

}
