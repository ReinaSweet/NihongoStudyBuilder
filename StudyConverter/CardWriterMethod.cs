using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihongoStudyBuilder.StudyConverter
{
    enum CardWriterFormat
    {
        kBrainscape,
        kCardTypeInfo
    }

    abstract class CardWriterMethod
    {
//public://static
        public static CardWriterMethod CreateWriterMethod(CardWriterFormat cardWriterFormat, DeckSerializer deckSerializer, Card baseCard, CardKey key)
        {
            switch (cardWriterFormat)
            {
                case CardWriterFormat.kBrainscape:
                    return new CardWriterMethodBrainscape(deckSerializer, baseCard, key);

                case CardWriterFormat.kCardTypeInfo:
                    return new CardWriterMethodCardTypeInfo(deckSerializer, baseCard, key);
            }
            return null;
        }

//public://abstract
        public abstract string GetLine();

//public:
        public void AttachCard(Card additionalCard)
        {
            mCards.Add(additionalCard);
        }

//protected:
        protected CardWriterMethod(DeckSerializer deckSerializer, Card baseCard, CardKey key)
        {
            mDeckSerializer = deckSerializer;
            mCards.Add(baseCard);
            mCardKey = key;
        }

        protected DeckSerializer mDeckSerializer;
        protected List<Card> mCards = new List<Card>();
        protected CardKey mCardKey;
    }
}
