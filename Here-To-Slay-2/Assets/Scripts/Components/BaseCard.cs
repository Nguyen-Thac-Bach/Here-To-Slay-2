
using Components.Enums;
using Unity.Collections;

namespace Components {
    public class BaseCard
    {
        #region Fields
        private int _cardId;
        private string _name;
        private string _description;
        private bool _isFaceUp;
        private Deck _deck;
        private int _cardPosition;

        #endregion

        #region Properties
        public int CardId { get => _cardId; }
        public string Name { get => _name; }
        //TODO: add image field
        public string Description { get => _description; }
        public bool IsFaceUP { get => _isFaceUp; }
        public Deck Deck { get => _deck; }
        /// <summary>
        /// Relevant for cards in the player's hand, field; attackable monsters
        /// </summary>
        public int CardPosition { get => _cardPosition; }

        #endregion

        #region Constructors
        public BaseCard(int cardId, string name, string description, bool isFaceUp, Deck deck)
        {
            _cardId = cardId;
            _name = name;
            _description = description;
            _isFaceUp = isFaceUp;
            _deck = deck;
            _cardPosition = -1;
        }
        #endregion

        #region Public methods

        public override string ToString()
        {
            string result = 
                $"Card ID: {_cardId},\n"+
                $"Name: {_name},\n"+
                $"Description: {_description},\n"+
                $"IsFaceUp: {_isFaceUp},\n"+
                $"Deck: {_deck},\n"+
                $"CardPosition: {_cardPosition}";
            return result;
        }

        /// <summary>
        /// Make the card face up
        /// </summary>
        public void Show()
        {
            _isFaceUp = true;
        }
        /// <summary>
        /// Make the card face down
        /// </summary>
        public void Hide()
        {
            _isFaceUp = false;
        }
        /// <summary>
        /// Move the card to the specified deck
        /// </summary>
        /// <param name="deck"></param>
        public void SetDeck(Deck deck)
        {
            _deck = deck;
        }
        /// <summary>
        /// Moves the card to the specified position within the deck
        /// </summary>
        /// <param name="cardPosition">-1 if the deck it is in has no limits to respect</param>
        public void SetCardPosition(int cardPosition)
        {
            _cardPosition = cardPosition;
        }
        #endregion
    }
}

