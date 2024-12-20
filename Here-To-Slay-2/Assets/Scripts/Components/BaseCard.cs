
using Components.Enums;
using Unity.Collections;

namespace Components {
    public class BaseCard
    {
        #region Fields
        private int _cardId;
        private string _name;
        private string _description;
        private bool _isViewable;
        private Deck _deck;

        #endregion

        #region Properties
        public int CardId { get => _cardId; }
        public string Name { get => _name; }
        //TODO: add image field
        public string Description { get => _description; }
        public bool IsViewable { get => _isViewable; }
        public Deck Deck { get => _deck; }

        #endregion

        #region Constructors
        public BaseCard(int cardId, string name, string description, bool isViewable)
        {
            _cardId = cardId;
            _name = name;
            _description = description;
            _isViewable = isViewable;

        }
        #endregion

        #region Public methods

        /// <summary>
        /// Make the card face up
        /// </summary>
        public void Show()
        {
            _isViewable = true;
        }
        /// <summary>
        /// Make the card face down
        /// </summary>
        public void Hide()
        {
            _isViewable = false;
        }
        /// <summary>
        /// Move the card to the specified deck
        /// </summary>
        /// <param name="deck"></param>
        public void SetDeck(Deck deck)
        {
            _deck = deck;
        }
    }
}

