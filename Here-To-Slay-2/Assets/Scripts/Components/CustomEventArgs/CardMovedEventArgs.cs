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
        public int CardId { get; set; }
        public Deck NewPosition { get; set; }
    }
}
