using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihongoStudyBuilder.StudyConverter
{
    class Deck
    {
// public:
        public Deck(DeckSerializer deckSerializer)
        {
            mDeckSerializer = deckSerializer;
        }

        public void InitializeFromLine(List<string> line)
        {
            List<string> titleBuilder = new List<string>();
            titleBuilder.Add(mDeckSerializer.GetBookName());

            string overallSection = line[1];
            if (overallSection.StartsWith("Appendix "))
            {
                mBookNumber = int.Parse(overallSection.Replace("Appendix ", ""));
                string appendixSection = line[2];
                titleBuilder.Add(appendixSection);

                mWeight = mDeckSerializer.GetNewAppendixSectionWeight(mBookNumber);
            }
            else
            {
                string[] bookChapterPair = line[1].Split('-');
                titleBuilder.Add(line[1]);

                mBookNumber = int.Parse(bookChapterPair[0]);
                mChapterNumber = int.Parse(bookChapterPair[1]);
                mWeight = mDeckSerializer.GetNewBookChapterSectionWeight(mBookNumber, mChapterNumber);
                
                if (!string.IsNullOrEmpty(line[2]))
                {
                    mSubsection = line[2];
                    titleBuilder.Add(mSubsection);
                }
            }

            mDeckSerializer.TrackWeightToNewOptions(mWeight, line[0]);

            mTitle = string.Join(" ", titleBuilder.ToArray());
        }

        public void InitializeFromDeckList(List<Deck> sourceDecks, string title)
        {
            mTitle = title;

            foreach (Deck deck in sourceDecks)
            {
                mCards.AddRange(deck.mCards);
                mWeight = Math.Max(mWeight, deck.mWeight);
            }
        }

        public void InitializeToJustTitle(string title)
        {
            mTitle = title;
        }

        public void AddCardFromLine(List<string> line)
        {
            if (line.Count < 2 || string.IsNullOrEmpty(line[1]))
            {
                return;
            }

            if (line.Contains("FIXME"))
            {
                mErrors.Add("FIXME line present");
                return;
            }

            Card card = null;
            try
            {
                card = new Card(mDeckSerializer, this, line);
                mCards.Add(card);
            }
            catch (Exception e)
            {
                mErrors.Add(e.Message);
            }
        }

        public void AddCardFromAnotherDeck(Deck deck, int cardIndex)
        {
            if (deck.mCards.Count > cardIndex)
            {
                mCards.Add(deck.mCards[cardIndex]);
            }
        }

        public string GetTitle()
        {
            return mTitle;
        }

        public bool IsValidDeck()
        {
            return (mCards.Count > 0) && (mErrors.Count == 0);
        }

        public int GetCardCount()
        {
            return mCards.Count;
        }

        public List<string> GetErrors()
        {
            return mErrors;
        }
        
        public void FilterCardsDownTo(CardType cardType)
        {
            mCards.RemoveAll((card) =>
            {
                if (card.HasCardType(cardType))
                {
                    return false;
                }
                return true;
            });
        }

        public void RandomizeDeck()
        {
            int cardsInDeck = mCards.Count;
            if (cardsInDeck < 8)
            {
                // We don't have enough cards to both distribute and get meaningful randomization, just do pure randomization
                ShuffleListOfCards(ref mCards);
            }
            else
            {
                // Distribute cards so that when we shuffle
                // We ensure that cards avoid their old neighbors
                int size = 2;
                int interval = 2;
                int shuffledSubsections = 2;

                int iteration = 0;
                int currentMaxCount = 8;

                int nextSize = 2;
                int nextInterval = 2;
                while (cardsInDeck > currentMaxCount)
                {
                    size = nextSize;
                    interval = nextInterval;

                    if ((iteration % 3) == 0)
                    {
                        ++nextSize;
                    }
                    else
                    {
                        ++nextInterval;
                    }
                    ++iteration;

                    currentMaxCount = nextSize * nextInterval * shuffledSubsections;
                }

                currentMaxCount = size * interval * shuffledSubsections;

                List<Card> shuffledDeck = new List<Card>();
                List<Card> shuffledSection = new List<Card>();
                int sourceCardIndex = 0;
                int baseSectionOfCardsAccounted = 0;
                int indexOfExtraCards = currentMaxCount;
                while (baseSectionOfCardsAccounted < currentMaxCount)
                {
                    shuffledSection.Add(mCards[sourceCardIndex]);

                    if (shuffledSection.Count == size)
                    {
                        baseSectionOfCardsAccounted += shuffledSection.Count;

                        if (indexOfExtraCards < mCards.Count)
                        {
                            shuffledSection.Add(mCards[indexOfExtraCards]);
                            ++indexOfExtraCards;
                        }

                        ShuffleListOfCards(ref shuffledSection);
                        shuffledDeck.AddRange(shuffledSection);
                        shuffledSection = new List<Card>();
                    }

                    sourceCardIndex += interval;
                    if (sourceCardIndex >= currentMaxCount)
                    {
                        sourceCardIndex -= (currentMaxCount - 1);
                    }
                }

                mCards = shuffledDeck;
            }
        }

        public BookAndChapterNum GetBookAndChapterNumber()
        {
            return new BookAndChapterNum(mBookNumber, mChapterNumber);
        }

        public void WriteDeck(string description, CardKey cardKey)
        {
            if (!Directory.Exists(mDeckSerializer.GetBookName()))
            {
                Directory.CreateDirectory(mDeckSerializer.GetBookName());
            }

            string filePath = mDeckSerializer.GetBookName() + '/' + mTitle + ' ' + description + ".csv";
            using (StreamWriter file = new StreamWriter(filePath))
            {
                Dictionary<string, CardWriterMethod> cardWriters = new Dictionary<string, CardWriterMethod>();
                int uniqueID = 1;

                foreach (Card card in mCards)
                {
                    if (!card.ShouldWriteForCardKey(cardKey))
                    {
                        continue;
                    }

                    string key = card.GetKeyResult(cardKey);
                    if (key.Length == 0)
                    {
                        key = "_UniqueID_" + uniqueID;
                        ++uniqueID;
                    }

                    if (cardWriters.ContainsKey(key))
                    {
                        cardWriters[key].AttachCard(card);
                    }
                    else
                    {
                        cardWriters[key] = new CardWriterMethod(mDeckSerializer, card, cardKey);
                    }
                }

                mDeckSerializer.SetCurrentWeight(mWeight);

                foreach (KeyValuePair<string, CardWriterMethod> cardWriterPair in cardWriters)
                {
                    CardWriterMethod cardWriter = cardWriterPair.Value;
                    file.WriteLine(cardWriter.GetLine());
                }
            }
        }

// private:
        private DeckSerializer mDeckSerializer;
        private string mSubsection = "";
        private int mBookNumber = -1;
        private int mChapterNumber = -1;

        private string mTitle;
        private List<Card> mCards = new List<Card>();

        private int mWeight = -1;

        private List<string> mErrors = new List<string>();
        
        private void ShuffleListOfCards(ref List<Card> cards)
        {
            // Fisher-Yates shuffle
            Random random = new Random();
            for (int i = cards.Count - 1; i > 0; --i)
            {
                int index = random.Next(i + 1);
                Card cardA = cards[index];
                cards[index] = cards[i];
                cards[i] = cardA;
            }
        }
    }
}
