using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.LD50.DialogueSystem.Structs
{
    [Serializable]
    public class DialogueQuote
    {
        public static readonly DialogueQuote Empty = new DialogueQuote(string.Empty);

        public string data;
        public float quoteTime;
        public override string ToString()
        {
            return data;
        }

        public DialogueQuote(string data = "", float quoteTime = 0.0f)
        {
            this.data = data;
            this.quoteTime = quoteTime;
        }
    }
}
