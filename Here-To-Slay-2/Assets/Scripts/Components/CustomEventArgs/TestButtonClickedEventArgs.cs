using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Components.Enums;

namespace Components.CustomEventArgs
{
    public class TestButtonClickedEventArgs : EventArgs
    {
        public int CardId { get; set; }
        public Deck Deck { get; set; }
    }
}
