using Components.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.CustomEventArgs
{
    public class TopCardInDrawDeckChangedEventArgs : EventArgs
    {
        public int NewTopCardInDrawDeckId { get; set; }
    }
}
