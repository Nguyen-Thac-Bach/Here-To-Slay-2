using Components.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.CustomEventArgs
{
    /// <summary>
    /// Event args for when a card is moved from one deck to another
    /// </summary>
    public class CardMovedEventArgs : EventArgs
    {
        /// <summary>
        /// The deck the card is moved from
        /// </summary>
        public Deck Origin { get; set; }
        /// <summary>
        /// The id of the card that was moved
        /// </summary>
        public int CardId { get; set; }
        /// <summary>
        /// The deck the card is moved to
        /// </summary>
        public Deck NewDeck { get; set; }
        /// <summary>
        /// The new position of the card in the new deck
        /// </summary>
        public int NewPosition { get; set; }
        /// <summary>
        /// Whether the origin deck needs adjustment (e.g. if a card was moved from a player's hand, the positions of the remaining cards need to be adjusted)
        /// </summary>
        public bool OriginNeedsAdjustment { get; set; }
        /// <summary>
        /// The ids of the cards that need to be adjusted in the origin deck
        /// </summary>
        public List<int> IdsToAdjust { get; set; }
        /// <summary>
        /// The new positions of the cards that need to be adjusted in the origin deck
        /// </summary>
        public List<int> AdjustedPositions { get; set; }
    }
}
