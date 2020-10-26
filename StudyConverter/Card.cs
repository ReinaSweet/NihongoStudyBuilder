using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihongoStudyBuilder.StudyConverter
{
    public enum CardType
    {
        kInvalid,
        kVerb,
        kVerbAru,
        kVerbState,
        kVerbKure,
        kVerbIku,
        kVerbGodan,
        kVerbIchidan,
        kVerbKuru,
        kVerbSuru,
        kAdjective,
        kAdjectiveI,
        kAdjectiveNa,
        kAdjectiveYo,
        kTransliterated,
        kPhrase,
        kNumeral
    }

    public enum CardKey
    {
        kKana,
        kEnglish,
        kKanji
    }

    class Card
    {
// public:
        public Card(DeckSerializer deckSerializer, Deck originalParentDeck, List<string> line)
        {
            mDeckSerializer = deckSerializer;
            mBookAndChapterNum = originalParentDeck.GetBookAndChapterNumber();

            InitializeCardTypes(line[0]);
            InitializeKana(line[1]);
            InitializeKanji(line[2]);
            mEnglish = line[3];
            mJishoText = line[4];
            mExtraInformation = line[5];

            if (mCardTypes.Contains(CardType.kVerb))
            {
                if (HasCardType(CardType.kVerbGodan) || HasCardType(CardType.kVerbIchidan) || HasCardType(CardType.kVerbSuru))
                {
                    InitializeVerbJishoFromType();
                    InitializeVerbForms();
                }
                else if (mJishoText.Length > 0)
                {
                    InitializeVerbTypes();
                    InitializeVerbForms();
                }
            }

            if (mCardTypes.Contains(CardType.kAdjectiveNa))
            {
                InitializeAdjectiveNaForms();
            }

            if (mCardTypes.Contains(CardType.kAdjectiveI))
            {
                InitializeAdjectiveIForms();
            }
        }

        public bool HasCardType(CardType cardType)
        {
            return mCardTypes.Contains(cardType);
        }

        public void GetEnabledWordForms(ref List<WordForm> wordForms)
        {
            foreach (WordForm potentialWordForm in mForms)
            {
                if (potentialWordForm.IsFormEnabled(mDeckSerializer))
                {
                    wordForms.Add(potentialWordForm);
                }
            }
        }

        public bool ShouldWriteForCardKey(CardKey cardKey)
        {
            if (cardKey == CardKey.kEnglish && HasCardType(CardType.kTransliterated))
            {
                return false;
            }
            return true;
        }

        public string GetKeyResult(CardKey key)
        {
            switch (key)
            {
                case CardKey.kKana:
                    return mKana;

                case CardKey.kEnglish:
                    return mEnglish;

                case CardKey.kKanji:
                    return mKanji;
            }
            return "";
        }

        public bool KeyResultsEquals(Card otherCard)
        {
            return (mKana == otherCard.mKana) &&
                (mEnglish == otherCard.mEnglish) &&
                (mKanji == otherCard.mKanji);
        }

        public BookAndChapterNum GetBookAndChapterNum()
        {
            return mBookAndChapterNum;
        }

        public string GetExtraInformation()
        {
            return mExtraInformation;
        }

        public List<string> GetKanjiLinks()
        {
            return mKanjiLinks;
        }

        override public string ToString()
        {
            return mKana + " : " + mEnglish;
        }

// private:
        private string mKana;
        private string mKanji;
        private string mEnglish;
        private string mJishoText;
        private string mExtraInformation;

        private char mLastKanji = '\0';
        private List<string> mKanjiLinks = new List<string>();

        private List<CardType> mCardTypes = new List<CardType>();
        private List<WordForm> mForms = new List<WordForm>();


        private void InitializeCardTypes(string typeText)
        {
            foreach (char c in typeText)
            {
                if (c == 'v')
                {
                    mCardTypes.Add(CardType.kVerb);
                }
                else if (c == '1')
                {
                    mCardTypes.Add(CardType.kVerbGodan);
                }
                else if (c == '2')
                {
                    mCardTypes.Add(CardType.kVerbIchidan);
                }
                else if (c == '3')
                {
                    mCardTypes.Add(CardType.kVerbSuru);
                }
                else if (c == 'a')
                {
                    mCardTypes.Add(CardType.kVerb);
                    mCardTypes.Add(CardType.kVerbAru);
                    mCardTypes.Add(CardType.kVerbState);
                    mCardTypes.Add(CardType.kVerbGodan);
                }
                else if (c == 's')
                {
                    mCardTypes.Add(CardType.kVerb);
                    mCardTypes.Add(CardType.kVerbState);
                }
                else if (c == 'k')
                {
                    mCardTypes.Add(CardType.kVerb);
                    mCardTypes.Add(CardType.kVerbKure);
                }
                else if (c == 'な')
                {
                    mCardTypes.Add(CardType.kAdjective);
                    mCardTypes.Add(CardType.kAdjectiveNa);
                }
                else if (c == 'い')
                {
                    mCardTypes.Add(CardType.kAdjective);
                    mCardTypes.Add(CardType.kAdjectiveI);
                }
                else if (c == 'よ')
                {
                    mCardTypes.Add(CardType.kAdjective);
                    mCardTypes.Add(CardType.kAdjectiveYo);
                }
                else if (c == 'T')
                {
                    mCardTypes.Add(CardType.kTransliterated);
                }
                else if (c == 'p')
                {
                    mCardTypes.Add(CardType.kPhrase);
                }
                else if (c == '#')
                {
                    mCardTypes.Add(CardType.kNumeral);
                }
            }
            mCardTypes = mCardTypes.Distinct().ToList();
        }

        private void InitializeKana(string kanaText)
        {
            mKana = kanaText;

            if (mKana.StartsWith(" "))
            {
                throw new Exception(string.Format("\"{0}\" : Starting whitespace on kana", mKana));
            }

            if (mKana.EndsWith(" "))
            {
                throw new Exception(string.Format("\"{0}\" : Trailing whitespace on kana", mKana));
            }

            foreach (char singleKana in mKana)
            {
                if (CharacterTools.IsKanji(singleKana))
                {
                    throw new Exception(string.Format("\"{0}\" : Has Kanji in the Kana field", mKana));
                }
            }
        }

        private void InitializeKanji(string kanjiText)
        {
            if (kanjiText == "—")
            {
                mKanji = "";
            }
            else
            {
                mKanji = kanjiText;

                if (mKanji.StartsWith(" "))
                {
                    throw new Exception(string.Format("\"{0}\" : Starting whitespace on kanji", mKana));
                }

                if (mKanji.EndsWith(" "))
                {
                    throw new Exception(string.Format("\"{0}\" : Trailing whitespace on kanji", mKana));
                }

                List<string> kanaSequences = new List<string>();
                string currentSequence = "";
                foreach (char c in mKanji)
                {
                    if (CharacterTools.IsHiragana(c) || CharacterTools.IsKatakana(c))
                    {
                        currentSequence += c;
                    }
                    else if (c == ',')
                    {
                        kanaSequences.Add(currentSequence);
                        currentSequence = "";
                    }
                    else if (CharacterTools.IsKanji(c))
                    {
                        string kanjiLink = string.Format("<a href='https://jisho.org/search/{0}%20%23kanji' target='_blank'>{0}</a>", c);
                        mKanjiLinks.Add(kanjiLink);
                        mLastKanji = c;
                    }
                }

                kanaSequences.Add(currentSequence);

                // Verify that we have kana and kanji information that at least roughly align
                foreach (string kanaSequence in kanaSequences)
                {
                    int lastTrackedIndex = 0;
                    foreach (char singleKana in kanaSequence)
                    {
                        int idx = mKana.IndexOf(singleKana, lastTrackedIndex);
                        if (idx == -1)
                        {
                            throw new Exception(string.Format("\"{0}\" : Has non-matching Kanji and Kana", mKana));
                        }
                        lastTrackedIndex = idx;
                    }
                }
            }
        }

        /***************************
         * Verbs
         ***************************/
        
        private void InitializeVerbJishoFromType()
        {
            // Only look at the last word in a line, not the whole line
            string reducedLastWord = mKana.Split(' ').Last();

            if (!reducedLastWord.EndsWith("ます"))
            {
                throw new Exception(string.Format("\"{0}\" : Does not end in ます", mKana));
            }
            if (reducedLastWord.Length == 2)
            {
                throw new Exception(string.Format("\"{0}\" : A verb can't be only ます", mKana));
            }

            reducedLastWord = reducedLastWord.Substring(0, reducedLastWord.Length - 2);

            // Special exception, Suru and Kuru are both "type 3"
            if (HasCardType(CardType.kVerbSuru) && mLastKanji == '来')
            {
                mCardTypes.Remove(CardType.kVerbSuru);
                mCardTypes.Add(CardType.kVerbKuru);
            }

            if (HasCardType(CardType.kVerbGodan))
            {
                mJishoText = reducedLastWord.Substring(0, reducedLastWord.Length - 1);
                mJishoText += CharacterTools.ConvertKanaVowelSound(reducedLastWord.Last(), 'う'); ;
            }
            else
            {
                if (HasCardType(CardType.kVerbIchidan))
                {
                    mJishoText = reducedLastWord;
                    mJishoText += 'る';
                }
                else if (HasCardType(CardType.kVerbSuru))
                {
                    mJishoText = reducedLastWord.Substring(0, reducedLastWord.Length - 1);
                    mJishoText += "する";
                }
                else if (HasCardType(CardType.kVerbKuru))
                {
                    mJishoText = reducedLastWord;
                    mJishoText += "くる";
                }
            }
        }

        private void InitializeVerbTypes()
        {
            CardType verbType = CardType.kVerbGodan; // Until shown otherwise

            if (mJishoText.Count() >= 2 && mJishoText.EndsWith("る"))
            {
                if (mKanji.EndsWith("ます") && mLastKanji != '\0' && (mKanji[mKanji.Length - 3] == mLastKanji))
                {
                    verbType = CardType.kVerbIchidan;
                }
                else
                {
                    char charBeforeRu = mJishoText[mJishoText.Count() - 2];
                    if (CharacterTools.IsHiraganaSoundE(charBeforeRu))
                    {
                        if (!CharacterTools.IsLastKanjiIchidanExceptionE(mLastKanji))
                        {
                            verbType = CardType.kVerbIchidan;
                        }
                    }
                    else if (CharacterTools.IsHiraganaSoundI(charBeforeRu))
                    {
                        if (!CharacterTools.IsLastKanjiIchidanExceptionI(mLastKanji))
                        {
                            verbType = CardType.kVerbIchidan;
                        }
                    }
                    else if (charBeforeRu == 'す')
                    {
                        verbType = CardType.kVerbSuru;
                    }
                    else if (charBeforeRu == 'く' && mLastKanji == '来')
                    {
                        verbType = CardType.kVerbKuru;
                    }
                }
            }

            mCardTypes.Add(verbType);

            if (verbType == CardType.kVerbGodan && mJishoText.EndsWith("いく"))
            {
                if (mJishoText.Length == 2 || char.IsWhiteSpace(mJishoText[mJishoText.Length - 3]))
                {
                    mCardTypes.Add(CardType.kVerbIku);
                }
            }
        }

        private void InitializeVerbForms()
        {
            WordForm jishoForm = new WordForm(mJishoText, WordFormType.kPerfect, WordPoliteness.kPlain, "じしょ");
            WordForm masuForm = new WordForm(mKana, WordFormType.kPerfect, WordPoliteness.kPolite, "ます");

            WordForm naiForm = null;
            WordForm teForm = null;
            WordForm potentialForm = null;
            WordForm volitionalForm = null;
            WordForm imperativeForm = null;
            WordForm conditionalForm = null;

            if (mCardTypes.Contains(CardType.kVerbGodan))
            {
                //
                // Godan
                //
                naiForm = new WordForm(jishoForm.GetWordWithConvertedKana('あ') + "ない", WordFormType.kNegative, WordPoliteness.kPlain, "ない");
                potentialForm = new WordForm(jishoForm.GetWordWithConvertedKana('え') + 'る', WordFormType.kPotential, WordPoliteness.kPlain, "れる");
                volitionalForm = new WordForm(jishoForm.GetWordWithConvertedKana('お') + 'う', WordFormType.kVolitional, WordPoliteness.kPlain, "よう");
                imperativeForm = new WordForm(jishoForm.GetWordWithConvertedKana('え'), WordFormType.kImperative, WordPoliteness.kUndefined, "ろ");
                conditionalForm = new WordForm(jishoForm.GetWordWithConvertedKana('え') + 'ば', WordFormType.kConditional, WordPoliteness.kUndefined, "れば");

                string charsDropped;
                string reducedJisho = jishoForm.GetWordMinusLastNChars(1, out charsDropped);

                if (mCardTypes.Contains(CardType.kVerbIku))
                {
                    teForm = new WordForm(reducedJisho + "って", WordFormType.kTe, WordPoliteness.kUndefined, "て");
                }
                else
                {
                    switch (charsDropped[0])
                    {
                        case 'う':
                        case 'つ':
                        case 'る':
                            teForm = new WordForm(reducedJisho + "って", WordFormType.kTe, WordPoliteness.kUndefined, "て");
                            break;

                        case 'く':
                            teForm = new WordForm(reducedJisho + "いて", WordFormType.kTe, WordPoliteness.kUndefined, "て");
                            break;

                        case 'ぐ':
                            teForm = new WordForm(reducedJisho + "いで", WordFormType.kTe, WordPoliteness.kUndefined, "て");
                            break;

                        case 'ぬ':
                        case 'ぶ':
                        case 'む':
                            teForm = new WordForm(reducedJisho + "んで", WordFormType.kTe, WordPoliteness.kUndefined, "て");
                            break;

                        case 'す':
                            teForm = new WordForm(reducedJisho + "して", WordFormType.kTe, WordPoliteness.kUndefined, "て");
                            break;

                        default:
                            throw new Exception(string.Format("\"{0}\" : Could not identify last hiragana for te form", mKana));
                    }
                }
            }
            else if (mCardTypes.Contains(CardType.kVerbIchidan))
            {
                //
                // Ichidan
                //
                string reducedJisho = jishoForm.GetWordMinusLastNChars(1);

                naiForm = new WordForm(reducedJisho + "ない", WordFormType.kNegative, WordPoliteness.kPlain, "ない");
                teForm = new WordForm(reducedJisho + "て", WordFormType.kTe, WordPoliteness.kUndefined, "て");
                potentialForm = new WordForm(reducedJisho + "られる", WordFormType.kPotential, WordPoliteness.kPlain, "れる");
                volitionalForm = new WordForm(reducedJisho + "よう", WordFormType.kVolitional, WordPoliteness.kPlain, "よう");
                imperativeForm = new WordForm(reducedJisho + "ろ", WordFormType.kImperative, WordPoliteness.kUndefined, "ろ");
                conditionalForm = new WordForm(reducedJisho + "れば", WordFormType.kConditional, WordPoliteness.kUndefined, "れば");
            }
            else if (mCardTypes.Contains(CardType.kVerbSuru))
            {
                //
                // Suru
                //
                string veryReducedJisho = jishoForm.GetWordMinusLastNChars(2);

                naiForm = new WordForm(veryReducedJisho + "しない", WordFormType.kNegative, WordPoliteness.kPlain, "ない");
                teForm = new WordForm(veryReducedJisho + "して", WordFormType.kTe, WordPoliteness.kUndefined, "て");
                potentialForm = new WordForm(veryReducedJisho + "できる", WordFormType.kPotential, WordPoliteness.kPlain, "れる");
                volitionalForm = new WordForm(veryReducedJisho + "しよう", WordFormType.kVolitional, WordPoliteness.kPlain, "よう");
                imperativeForm = new WordForm(veryReducedJisho + "しろ", WordFormType.kImperative, WordPoliteness.kUndefined, "ろ");
                conditionalForm = new WordForm(veryReducedJisho + "すれば", WordFormType.kConditional, WordPoliteness.kUndefined, "れば");
            }
            else if (mCardTypes.Contains(CardType.kVerbKuru))
            {
                //
                // Kuru
                //
                string veryReducedJisho = jishoForm.GetWordMinusLastNChars(2);

                naiForm = new WordForm(veryReducedJisho + "こない", WordFormType.kNegative, WordPoliteness.kPlain, "ない");
                teForm = new WordForm(veryReducedJisho + "きて", WordFormType.kTe, WordPoliteness.kUndefined, "て");
                potentialForm = new WordForm(veryReducedJisho + "こられる", WordFormType.kPotential, WordPoliteness.kPlain, "れる");
                volitionalForm = new WordForm(veryReducedJisho + "こよう", WordFormType.kVolitional, WordPoliteness.kPlain, "よう");
                imperativeForm = new WordForm(veryReducedJisho + "こい", WordFormType.kImperative, WordPoliteness.kUndefined, "ろ");
                conditionalForm = new WordForm(veryReducedJisho + "くれば", WordFormType.kConditional, WordPoliteness.kUndefined, "れば");
            }
            else
            {
                throw new Exception(string.Format("\"{0}\" : Unidentified verb type attempted to be used", mKana));
            }

            //
            // Exceptions
            //
            if (mCardTypes.Contains(CardType.kVerbAru))
            {
                naiForm = new WordForm("ない", WordFormType.kNegative, WordPoliteness.kPlain, "ない");
            }

            if (mCardTypes.Contains(CardType.kVerbKure))
            {
                string reducedJisho = jishoForm.GetWordMinusLastNChars(1);
                imperativeForm = new WordForm(reducedJisho, WordFormType.kImperative, WordPoliteness.kUndefined, "ろ");
            }

            if (mCardTypes.Contains(CardType.kVerbState))
            {
                imperativeForm = null;
            }

            //
            // Form order on the card
            //
            mForms.Add(masuForm);

            mForms.Add(jishoForm);
            mForms.Add(new WordForm(teForm.GetWordMinusLastNChars(1) + "た", WordFormType.kPast, WordPoliteness.kPlain, "た"));
            mForms.Add(new WordForm(masuForm.GetWordMinusLastNChars(1) + "した", WordFormType.kPast, WordPoliteness.kPolite, "ました"));
            mForms.Add(naiForm);
            mForms.Add(new WordForm(masuForm.GetWordMinusLastNChars(1) + "せん", WordFormType.kNegative, WordPoliteness.kPolite, "ません"));
            mForms.Add(new WordForm(naiForm.GetWordMinusLastNChars(1) + "かった", WordFormType.kNegativePast, WordPoliteness.kPlain, "なかった"));
            mForms.Add(new WordForm(masuForm.GetWordMinusLastNChars(1) + "せんでした", WordFormType.kNegativePast, WordPoliteness.kPolite, "ませんでした"));
            mForms.Add(teForm);
            mForms.Add(potentialForm);
            mForms.Add(new WordForm(potentialForm.GetWordMinusLastNChars(1) + "ます", WordFormType.kPotential, WordPoliteness.kPolite, "れます"));
            mForms.Add(volitionalForm);
            mForms.Add(new WordForm(masuForm.GetWordMinusLastNChars(1) + "ましょう", WordFormType.kPotential, WordPoliteness.kPolite, "ましょう"));
            if (imperativeForm != null) { mForms.Add(imperativeForm); }
            mForms.Add(new WordForm(jishoForm.GetWord() + "な", WordFormType.kProhibitive, WordPoliteness.kUndefined, "な"));
            mForms.Add(conditionalForm);
        }

        /***************************
         * Adjectives
         ***************************/

        private void InitializeAdjectiveNaForms()
        {
            mForms.Add(new WordForm(mKana + "だ", WordFormType.kPerfect, WordPoliteness.kPlain));
            mForms.Add(new WordForm(mKana + "だた", WordFormType.kPast, WordPoliteness.kPlain));
            mForms.Add(new WordForm(mKana + "じゃない", WordFormType.kNegative, WordPoliteness.kPlain));
            mForms.Add(new WordForm(mKana + "じゃなかった", WordFormType.kNegativePast, WordPoliteness.kPlain));

            mForms.Add(new WordForm(mKana + "です", WordFormType.kPerfect, WordPoliteness.kPolite));
            mForms.Add(new WordForm(mKana + "でした", WordFormType.kPast, WordPoliteness.kPolite));
            mForms.Add(new WordForm(mKana + "じゃありません", WordFormType.kNegative, WordPoliteness.kPolite));
            mForms.Add(new WordForm(mKana + "じゃありませんでした", WordFormType.kNegativePast, WordPoliteness.kPolite));

            mForms.Add(new WordForm(mKana + "で", WordFormType.kTe, WordPoliteness.kUndefined));

            mForms.Add(new WordForm(mKana + "なら", WordFormType.kConditional, WordPoliteness.kUndefined));
        }

        private void InitializeAdjectiveIForms()
        {
            string droppedI = mKana.Substring(0, mKana.Length - 1);
            if (mCardTypes.Contains(CardType.kAdjectiveYo))
            {
                droppedI = droppedI.Substring(0, droppedI.Length - 1) + "よ";
            }

            mForms.Add(new WordForm(mKana, WordFormType.kPerfect, WordPoliteness.kPlain));
            mForms.Add(new WordForm(droppedI + "かった", WordFormType.kPast, WordPoliteness.kPlain));
            mForms.Add(new WordForm(droppedI + "くない", WordFormType.kNegative, WordPoliteness.kPlain));
            mForms.Add(new WordForm(droppedI + "くなかった", WordFormType.kNegativePast, WordPoliteness.kPlain));

            mForms.Add(new WordForm(mKana + "です", WordFormType.kPerfect, WordPoliteness.kPolite));
            mForms.Add(new WordForm(droppedI + "かったです", WordFormType.kPast, WordPoliteness.kPolite));
            mForms.Add(new WordForm(droppedI + "くないです", WordFormType.kNegative, WordPoliteness.kPolite));
            mForms.Add(new WordForm(droppedI + "くなかったです", WordFormType.kNegativePast, WordPoliteness.kPolite));

            mForms.Add(new WordForm(droppedI + "くて", WordFormType.kTe, WordPoliteness.kUndefined));

            mForms.Add(new WordForm(droppedI + "ければ", WordFormType.kConditional, WordPoliteness.kUndefined));
        }

        /***************************
         * Member Variables
         ***************************/
        DeckSerializer mDeckSerializer;
        BookAndChapterNum mBookAndChapterNum;
    }
}
