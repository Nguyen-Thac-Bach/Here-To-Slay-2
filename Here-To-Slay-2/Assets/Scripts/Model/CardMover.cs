using UnityEngine;
namespace Model
{
    public class CardMover : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }
        //TODO: have a separate card mover for model and view
        //model should first have its own tracker of cards and which deck they are in, SEPARATE FROM THAT OF THE VIEW
        //model should have a method to move a card from one deck to another
        //after movement in model, view should be updated
        public void MoveCard(GameObject card, GameObject newParent)
        {
            card.transform.SetParent(newParent.transform);
        }
    }
}

