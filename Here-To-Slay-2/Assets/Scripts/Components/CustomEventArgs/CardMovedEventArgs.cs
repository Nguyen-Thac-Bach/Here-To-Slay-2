using Components.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.CustomEventArgs
{
    public class CardMovedEventArgs : EventArgs
    {
        public Deck Origin { get; set; }
        public int CardId { get; set; }
        public Deck NewDeck { get; set; }
        public int NewPosition { get; set; }

        public bool OriginNeedsAdjustment { get; set; }
        public List<int> IdsToAdjust { get; set; }
        public List<int> AdjustedPositions { get; set; }
    }
}
