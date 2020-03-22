using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihongoStudyBuilder.StudyConverter
{
    enum WordFormType
    {
        kPerfect,
        kTe,
        kPast,
        kNegative,
        kNegativePast,
        kPotential,
        kVolitional,
        kImperative,
        kProhibitive,
        kConditional
    }

    enum WordPoliteness
    {
        kUndefined,
        kPlain,
        kPolite
    }

    enum WordFormCategory
    {
        kBeginner,
        kIntermediate1,
        kIntermediate2
    }

    enum WordFormConfig
    {
        kNone = 0,
        kRequiresOption  = 1 << 0
    }

    class WordForm
    {
        class FormTypeInfo
        {
            public FormTypeInfo(WordFormCategory category, WordFormConfig config = WordFormConfig.kNone)
            {
                mCategory = category;
                mConfig = config;
            }

            public bool HasConfig(WordFormConfig config)
            {
                return (mConfig & config) != 0;
            }

            public WordFormCategory GetCategory()
            {
                return mCategory;
            }
            
            private WordFormCategory mCategory;
            private WordFormConfig mConfig;
        }


// private: // static
        static readonly Dictionary<WordFormType, FormTypeInfo> sTypeInfo = new Dictionary<WordFormType, FormTypeInfo>()
        {
            { WordFormType.kPerfect, new FormTypeInfo(WordFormCategory.kBeginner) },
            { WordFormType.kTe, new FormTypeInfo(WordFormCategory.kBeginner) },
            { WordFormType.kPast, new FormTypeInfo(WordFormCategory.kBeginner) },
            { WordFormType.kNegative, new FormTypeInfo(WordFormCategory.kBeginner) },
            { WordFormType.kNegativePast, new FormTypeInfo(WordFormCategory.kBeginner) },
            { WordFormType.kPotential, new FormTypeInfo(WordFormCategory.kIntermediate1, WordFormConfig.kRequiresOption) },
            { WordFormType.kVolitional, new FormTypeInfo(WordFormCategory.kIntermediate1, WordFormConfig.kRequiresOption) },
            { WordFormType.kImperative, new FormTypeInfo(WordFormCategory.kIntermediate1, WordFormConfig.kRequiresOption) },
            { WordFormType.kProhibitive, new FormTypeInfo(WordFormCategory.kIntermediate1, WordFormConfig.kRequiresOption) },
            { WordFormType.kConditional, new FormTypeInfo(WordFormCategory.kIntermediate2, WordFormConfig.kRequiresOption) }
        };
        
// public:
        public WordForm(string word, WordFormType type, WordPoliteness politeness, string optionalHint = null)
        {
            mWord = word;
            mType = type;
            mPoliteness = politeness;
            mOptionalHint = optionalHint;

            // Build descriptions
            bool previousCharIsLowercase = false;
            string typeBasedDescription = type.ToString().Substring(1);
            foreach (char c in typeBasedDescription)
            {
                if (char.IsUpper(c) && previousCharIsLowercase)
                {
                    mDescription += ' ';
                }
                mDescription += c;
                previousCharIsLowercase = char.IsLower(c);
            }

            // Find TypeInfo
            foreach (var typeInfoPair in sTypeInfo)
            {
                if (type == typeInfoPair.Key)
                {
                    mFormTypeInfo = typeInfoPair.Value;
                    break;
                }
            }
            if (mFormTypeInfo == null) { throw new Exception("Missing FormTypeInfo for: " + type.ToString()); }
            
            // Always enable masu
            if (type == WordFormType.kPerfect && politeness == WordPoliteness.kPolite)
            {
                mEnabledWithPlainSet = true;
                mEnabledWithPoliteSet = true;
            }
            else
            {
                mEnabledWithPlainSet = (politeness != WordPoliteness.kPolite);
                mEnabledWithPoliteSet = (politeness != WordPoliteness.kPlain);
            }
        }

        public string GetWord() { return mWord; }
        public WordFormCategory GetWordFormCategory() { return mFormTypeInfo.GetCategory(); }
        public WordFormType GetWordFormType() { return mType; }

        public bool IsTypeAndPoliteness(WordFormType type, WordPoliteness politeness)
        {
            return (mType == type) && (mPoliteness == politeness);
        }
        
        public string GetWordWithConvertedKana(char desiredKana)
        {
            bool forVerbUsage = true;
            return mWord.Substring(0, mWord.Count() - 1) + CharacterTools.ConvertKanaVowelSound(mWord.Last(), desiredKana, forVerbUsage);
        }

        public string GetWordMinusLastNChars(int charsToDrop)
        {
            return mWord.Substring(0, mWord.Count() - charsToDrop);
        }

        public string GetWordMinusLastNChars(int charsToDrop, out string charsDropped)
        {
            charsDropped = mWord.Substring(mWord.Count() - charsToDrop);
            return mWord.Substring(0, mWord.Count() - charsToDrop);
        }

        public bool TryGetDescription(out string description)
        {
            if (mDescription == null)
            {
                description = "";
                return false;
            }
            description = mDescription;
            return true;
        }

        public bool IsFormEnabled(DeckSerializer deckSerializer)
        {
            if (deckSerializer.HasExtraOptionEnabled(StudyOptions.Option.kPlainForm))
            {
                if (!mEnabledWithPlainSet)
                {
                    return false;
                }
            }
            else
            {
                if (!mEnabledWithPoliteSet)
                {
                    return false;
                }
            }

            if (mFormTypeInfo.HasConfig(WordFormConfig.kRequiresOption))
            {
                if (!deckSerializer.HasOptionalWordFormTypeEnabled(mType))
                {
                    return false;
                }
            }
            
            return true;
        }

        override public string ToString()
        {
            return string.Format("{0} ({1}, {2})", mWord, mType.ToString(), mPoliteness.ToString());
        }

// private:
        private string mWord;
        private WordFormType mType;
        private WordPoliteness mPoliteness;
        private FormTypeInfo mFormTypeInfo = null;
        private string mOptionalHint = null;
        private string mDescription = "";
        private bool mEnabledWithPlainSet = false;
        private bool mEnabledWithPoliteSet = false;

        private bool TypeIsAnyOf(WordFormType[] types)
        {
            foreach (WordFormType type in types)
            {
                if (type == mType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
