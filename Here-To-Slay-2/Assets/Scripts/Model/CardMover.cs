using UnityEngine;
namespace Model
{
    public class CardMover : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        public void MoveCard(GameObject card, GameObject newParent)
        {
            card.transform.SetParent(newParent.transform);
        }
    }
}

