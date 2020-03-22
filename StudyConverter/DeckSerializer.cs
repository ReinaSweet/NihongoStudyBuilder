using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihongoStudyBuilder.StudyConverter
{
    class DeckSerializer
    {
// public: // static
        static public int BookChapterPairToWeight(int book, int chapter, int section = 0)
        {
            return (book * kWeightMaxChapterKindsPerBook) +
                (Convert.ToInt32(WeightChapterKind.kChapterPair) * kWeightMaxChaptersPerChapterKind) +
                (chapter * kWeightMaxSectionsPerChapter) +
                section;
        }

        static public int BookAppendixToWeight(int book, int appendixEntryCount)
        {
            return (book * kWeightMaxChapterKindsPerBook) +
                (Convert.ToInt32(WeightChapterKind.kAppendix) * kWeightMaxChaptersPerChapterKind) +
                (appendixEntryCount * kWeightMaxSectionsPerChapter);
        }

        static public int ConvertWeightToChapterMinimumWeight(int weight)
        {
            return kWeightMaxSectionsPerChapter * (int)Math.Floor(((float)weight) / kWeightMaxSectionsPerChapter);
        }
        
// public:
        public DeckSerializer(string sourceFile, string bookName)
        {
            mSourceFileName = sourceFile;
            mBookName = bookName;
        }

        public void LoadDecks()
        {
            StreamReader file = new StreamReader(mSourceFileName, Encoding.UTF8);

            string line;
            Deck currentDeck = new Deck(this);

            while ((line = file.ReadLine()) != null)
            {
                // Ignore blank and effectively blank lines
                string strippedLine = "";
                foreach (char c in line)
                {
                    if (!char.IsWhiteSpace(c) && c != '—')
                    {
                        strippedLine += c;
                    }
                }

                if (strippedLine.Length == 0)
                {
                    continue;
                }

                // Stoppers between decks
                if (line.StartsWith("@"))
                {
                    if (currentDeck.HasCards())
                    {
                        mChapterDecks.Add(currentDeck);
                    }
                    currentDeck = new Deck(this);
                    currentDeck.InitializeFromLine(SplitSourceFileLine(line));
                }
                // Read in each line into its component parts
                else
                {
                    currentDeck.AddCardFromLine(SplitSourceFileLine(line));
                }
            }

            file.Close();

            // Close out last deck
            if (currentDeck.HasCards())
            {
                mChapterDecks.Add(currentDeck);
            }

            // Track which book and chapter numbers are available for aggregating
            BookAndChapterNum lastBookAndChapterNumber = new BookAndChapterNum();
            foreach (Deck deck in mChapterDecks)
            {
                BookAndChapterNum thisBookAndChapterNumber = deck.GetBookAndChapterNumber();
                if (thisBookAndChapterNumber.IsValid() && thisBookAndChapterNumber.IsGreaterThan(lastBookAndChapterNumber))
                {
                    mBookAndChapterNumbers.Add(thisBookAndChapterNumber);
                    lastBookAndChapterNumber = thisBookAndChapterNumber;
                }
            }
        }

        public string GetBookName()
        {
            return mBookName;
        }

        public List<BookAndChapterNum> GetBookAndChapterNumbers()
        {
            return mBookAndChapterNumbers;
        }

        public void AddAllDecksOfQualifyingBookAndChapter(BookAndChapterNum bookAndChapterNum, ref List<Deck> deckListToAppend)
        {
            foreach (Deck deck in mChapterDecks)
            {
                if (bookAndChapterNum.IsEqualTo(deck.GetBookAndChapterNumber()))
                {
                    deckListToAppend.Add(deck);
                }
            }
        }

        public List<Deck> GetChapterDecks() { return mChapterDecks; }

        public void WriteSingleDeck(Deck deck)
        {
            // Brainscape has a card limit per deck of 500
            // Choose something lower so that it's more obvious to us what's happening as we approach that limit
            const int deckLimit = 450;
            const int deckLowerLimit = 400;

            int cardCount = deck.GetCardCount();

            if (cardCount > deckLimit)
            {
                // Method 1: Try to make all decks at the lower limit
                // And then distribute excess cards among them
                int numberOfDecks = cardCount / deckLowerLimit;
                int remainder = cardCount % deckLowerLimit;
                int sizePerDeck = deckLowerLimit + (int)Math.Ceiling((float)(remainder) / (float)(numberOfDecks));

                // If the prior method has decks exceed the deckLimit still, instead
                // Method 2: Increase number of decks and distribute cards
                // until we're safely below the deckLimit, ignoring the lower limit
                while (sizePerDeck > deckLimit)
                {
                    ++numberOfDecks;
                    sizePerDeck = (int)Math.Ceiling((float)cardCount / (float)numberOfDecks);
                }

                List<Deck> listOfSplitDecks = new List<Deck>();
                int currentDeckNum = 0;

                for (int i = 0; i < cardCount; ++i)
                {
                    if (listOfSplitDecks.Count == 0 || listOfSplitDecks.Last().GetCardCount() == sizePerDeck)
                    {
                        ++currentDeckNum;
                        string deckTitle = string.Format("{0} pt {1}", deck.GetTitle(), currentDeckNum.ToString().PadLeft(2, '0'));
                        listOfSplitDecks.Add(new Deck(this));
                        listOfSplitDecks.Last().InitializeToJustTitle(deckTitle);
                    }

                    listOfSplitDecks.Last().AddCardFromAnotherDeck(deck, i);
                }

                foreach (Deck splitDeck in listOfSplitDecks)
                {
                    WriteSingleDeckForAllLanguages(splitDeck);
                }
            }
            else
            {
                WriteSingleDeckForAllLanguages(deck);
            }
        }
        
        public int GetNewAppendixSectionWeight(int bookNumber)
        {
            if (mBookNumberToAppendixCount.ContainsKey(bookNumber))
            {
                mBookNumberToAppendixCount[bookNumber]++;
            }
            else
            {
                mBookNumberToAppendixCount[bookNumber] = 1;
            }

            return BookAppendixToWeight(bookNumber, mBookNumberToAppendixCount[bookNumber]);
        }

        public int GetNewBookChapterSectionWeight(int bookNumber, int bookChapter)
        {
            if (!mBookNumberAndChapterToSectionCount.ContainsKey(bookNumber))
            {
                mBookNumberAndChapterToSectionCount[bookNumber] = new Dictionary<int, int>();
            }
            Dictionary<int, int> chapterToSectionCount = mBookNumberAndChapterToSectionCount[bookNumber];

            if (chapterToSectionCount.ContainsKey(bookChapter))
            {
                chapterToSectionCount[bookChapter]++;
            }
            else
            {
                chapterToSectionCount[bookChapter] = 1;
            }

            return BookChapterPairToWeight(bookNumber, bookChapter, chapterToSectionCount[bookChapter]);
        }

        public void TrackWeightToNewOptions(int weight, string optionString)
        {
            if (optionString.StartsWith("@") && optionString.Count() > 1)
            {
                optionString = optionString.Substring(1);
                string[] optionList = optionString.Split(',');
                foreach (string singleOption in optionList)
                {
                    int chapterMinWeight = ConvertWeightToChapterMinimumWeight(weight);

                    StudyOptions.Option option;
                    if (StudyOptions.TryGetOptionFromText(singleOption, out option))
                    {
                        mEnabledOptionsByChapterWeight.Add(new KeyValuePair<int, StudyOptions.Option>(chapterMinWeight, option));
                    }
                    else
                    {
                        WordFormType type;
                        if (Enum.TryParse<WordFormType>("k" + singleOption.First().ToString().ToUpper() + singleOption.Substring(1), out type))
                        {
                            mEnabledWordFormTypesByChapterWeight.Add(new KeyValuePair<int, WordFormType>(chapterMinWeight, type));
                        }
                    }
                }
            }
        }

        public void SetPermanentExtraOption(StudyOptions.Option option)
        {
            mPermanentExtraOptions.Add(option);
        }
        
        public void ClearPermanentExtraOptions()
        {
            mPermanentExtraOptions.Clear();
        }

        public void SetCurrentWeight(int weight)
        {
            int chapterMinWeight = ConvertWeightToChapterMinimumWeight(weight);

            mByWeightExtraOptions.Clear();
            foreach (KeyValuePair<int, StudyOptions.Option> pair in mEnabledOptionsByChapterWeight)
            {
                if (pair.Key <= chapterMinWeight)
                {
                    mByWeightExtraOptions.Add(pair.Value);
                }
            }

            mByWeightWordFormTypes.Clear();
            foreach (KeyValuePair<int, WordFormType> pair in mEnabledWordFormTypesByChapterWeight)
            {
                if (pair.Key <= chapterMinWeight)
                {
                    mByWeightWordFormTypes.Add(pair.Value);
                }
            }
        }

        public bool HasExtraOptionEnabled(StudyOptions.Option option)
        {
            if (mPermanentExtraOptions.Contains(option) ||
                mByWeightExtraOptions.Contains(option))
            {
                return true;
            }
            return false;
        }

        public bool HasOptionalWordFormTypeEnabled(WordFormType type)
        {
            return (mByWeightWordFormTypes.Contains(type));
        }

        public bool IsWeightMatchingChapter(int weight, int bookNumber, int chapter)
        {
            int matchingWeight = ConvertWeightToChapterMinimumWeight(weight);
            int vsWeight = BookChapterPairToWeight(bookNumber, chapter);
            return (matchingWeight == vsWeight);
        }

// private:
        private string mSourceFileName;
        private string mBookName;
        private List<Deck> mChapterDecks = new List<Deck>();
        private List<BookAndChapterNum> mBookAndChapterNumbers = new List<BookAndChapterNum>();

        private Dictionary<int, int> mBookNumberToAppendixCount = new Dictionary<int, int>();
        private Dictionary<int, Dictionary<int, int>> mBookNumberAndChapterToSectionCount = new Dictionary<int, Dictionary<int, int>>();
        private List<KeyValuePair<int, StudyOptions.Option>> mEnabledOptionsByChapterWeight = new List<KeyValuePair<int, StudyOptions.Option>>();
        private List<KeyValuePair<int, WordFormType>> mEnabledWordFormTypesByChapterWeight = new List<KeyValuePair<int, WordFormType>>();

        private List<StudyOptions.Option> mPermanentExtraOptions = new List<StudyOptions.Option>();
        private List<StudyOptions.Option> mByWeightExtraOptions = new List<StudyOptions.Option>();
        private List<WordFormType> mByWeightWordFormTypes = new List<WordFormType>();

        private const int kWeightMaxSectionsPerChapter = 10;
        private const int kWeightMaxChaptersPerChapterKind = kWeightMaxSectionsPerChapter * 100;
        private const int kWeightMaxChapterKindsPerBook = kWeightMaxChaptersPerChapterKind * 10;
        enum WeightChapterKind
        {
            kChapterPair = 0,
            kAppendix
        }

        private void WriteSingleDeckForAllLanguages(Deck deck)
        {
            deck.WriteDeck("From English", CardKey.kEnglish);
            deck.WriteDeck("From Japanese (Kana)", CardKey.kKana);
            deck.WriteDeck("From Japanese (Kanji)", CardKey.kKanji);
        }

        private List<string> SplitSourceFileLine(string line)
        {
            const int columnsExpected = 6;
            List<string> splitLine = line.Split('\t').ToList();
            for (int i = 0; i < columnsExpected; ++i)
            {
                if (splitLine.Count <= i)
                {
                    splitLine.Add("");
                }
                else if (splitLine[i].Equals("—"))
                {
                    splitLine[i] = "";
                }
            }
            return splitLine;
        }
    }
}
