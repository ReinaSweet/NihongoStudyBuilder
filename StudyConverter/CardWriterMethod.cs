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
        public void AttachCard(Card card)
        {
            if (card.GetBookAndChapterNum().IsValid())
            {
                mCardChapterNums.Add(card.GetBookAndChapterNum());
            }

            if ((mCards.Count == 0) ||
                (!mCards.First().KeyResultsEquals(card)))
            {
                mCards.Add(card);
            }
        }

//protected:
        protected CardWriterMethod(DeckSerializer deckSerializer, Card baseCard, CardKey key)
        {
            mDeckSerializer = deckSerializer;
            AttachCard(baseCard);
            mCardKey = key;
        }

        protected DeckSerializer mDeckSerializer;
        protected List<Card> mCards = new List<Card>();
        protected List<BookAndChapterNum> mCardChapterNums = new List<BookAndChapterNum>();
        protected CardKey mCardKey;
    }
}
