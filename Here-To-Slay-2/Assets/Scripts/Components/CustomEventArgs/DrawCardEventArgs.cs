using System;
using Components.Enums;

namespace Components.CustomEventArgs
{

    public class DrawCardEventArgs : EventArgs
    {
        public bool activePlayerDraws { get; set; }
    }
}
