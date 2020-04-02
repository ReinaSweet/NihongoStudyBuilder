using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihongoStudyBuilder.StudyConverter
{
    enum CardWriterFormat
    {
        kBrainscape
    }

    abstract class CardWriterMethod
    {
//public:
        public static CardWriterMethod CreateWriterMethod(CardWriterFormat cardWriterFormat, DeckSerializer deckSerializer, Card baseCard, CardKey key)
        {
            switch (cardWriterFormat)
            {
                case CardWriterFormat.kBrainscape:
                    return new CardWriterMethodBrainscape(deckSerializer, baseCard, key);
            }
            return null;
        }

        public abstract void AttachCard(Card additionalCard);

        public abstract string GetLine();
    }
}
