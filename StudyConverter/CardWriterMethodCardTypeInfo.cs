using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihongoStudyBuilder.StudyConverter
{
    class CardWriterMethodCardTypeInfo : CardWriterMethod
    {
//public:
        public CardWriterMethodCardTypeInfo(DeckSerializer deckSerializer, Card baseCard, CardKey key)
            : base(deckSerializer, baseCard, key)
        {}

        public override string GetLine()
        {
            return "";
        }
    }
}
