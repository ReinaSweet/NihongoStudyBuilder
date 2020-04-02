using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihongoStudyBuilder.StudyConverter
{
    class CardWriterMethodBrainscape : CardWriterMethod
    {
//public:
        public CardWriterMethodBrainscape(DeckSerializer deckSerializer, Card baseCard, CardKey key)
            : base(deckSerializer, baseCard, key)
        {
            if (baseCard.HasCardType(CardType.kPhrase))
            {
                mSharedPreNote += "[Phrase]";
            }
        }

        public override string GetLine()
        {
            bool placeExtraInfoWithCardValue = (mCards.Count > 1);

            // Build key
            string key = Format_FinalStep(BuildKey(placeExtraInfoWithCardValue), 100);

            // Build values
            int percentPerValueCard = (int)Math.Floor(100.0f / (float)mCards.Count);
            string value = "";
            foreach (Card card in mCards)
            {
                value += Format_FinalStep(BuildValueForCard(card, placeExtraInfoWithCardValue), percentPerValueCard);
            }
            
            // Piece key and values together
            return string.Format("\"{0}\",\"{1}\"", key, value);
        }

//private: // static
        private const string kLineBreak = "•<span style='text-decoration: line-through'>————————————</span>";

//private:
        private string mSharedPreNote = "";

        private string Format_MainSubject(string text)
        {
            if (text.Length == 0) { return ""; }
            return string.Format("<span class='large'>{0}</span>", text);
        }

        private string Format_PreNote(string text)
        {
            if (text.Length == 0) { return ""; }
            return string.Format("<span class='small'>{0}•</span>", text);
        }

        private string Format_PlainText(string text)
        {
            if (text.Length == 0) { return ""; }
            return string.Format("•{0}", text);
        }

        private string Format_WordSpecificity(string text)
        {
            if (text.Length == 0) { return ""; }
            return string.Format("<span class='small'>•[{0}]</span>", text);
        }

        private string Format_InfoLine(string subject, string text)
        {
            if (text.Length == 0) { return ""; }
            return string.Format("•{0}: {1}", subject, text);
        }

        private string Format_FinalStep(string source, int percentPer)
        {
            if (source.Contains('•'))
            {
                source = source.Replace("•", "<br>");
            }
            if (source.Contains('○'))
            {
                source = source.Replace("○", "<br>••• ");
            }
            if (source.Contains("..."))
            {
                source = source.Replace("...", "•••");
            }
            if (source.Contains('"'))
            {
                source = source.Replace("\"", "&quot;");
            }
            return string.Format("<p class='medium' style='text-align:left; float:left; width:{1}%;'>{0}</p>", source, percentPer);
        }

        private string BuildKey(bool placeExtraInfoWithCardValue)
        {
            // Main Subject
            string keyText = mCards[0].GetKeyResult(mCardKey);
            if (keyText.Length == 0)
            {
                if (mCardKey == CardKey.kKanji)
                {
                    keyText = mCards[0].GetKeyResult(CardKey.kKana);
                }
                else
                {
                    throw new Exception(string.Format("Card \"{0}\" is missing entry for key \"{1}\"", mCards[0].ToString(), mCardKey.ToString()));
                }
            }

            // Extra info for key, but only if we are NOT a multi answer card
            string keyExtraInfoNote = "";
            if (!placeExtraInfoWithCardValue && mCards[0].GetExtraInformation().Length > 0)
            {
                keyExtraInfoNote = mCards[0].GetExtraInformation();
            }

            // Key notes describing what extra forms we expect
            string keyNote = "";
            List<WordForm> enabledWordForms = new List<WordForm>();
            mCards[0].GetEnabledWordForms(ref enabledWordForms);

            List<string> formDescriptions = new List<string>();

            WordFormCategory lastSeenCategory = WordFormCategory.kBeginner;
            foreach (WordForm form in enabledWordForms)
            {
                if (form.TryGetDescription(out string description) && !formDescriptions.Contains(description))
                {
                    if ((formDescriptions.Count > 0) && form.GetWordFormCategory() != lastSeenCategory)
                    {
                        keyNote += string.Join(", ", formDescriptions) + "•";
                        formDescriptions.Clear();
                        lastSeenCategory = form.GetWordFormCategory();
                    }

                    formDescriptions.Add(description);
                }
            }

            if (formDescriptions.Count > 0)
            {
                keyNote += string.Join(", ", formDescriptions);
            }

            // Finish
            return 
                Format_PreNote(mSharedPreNote) +
                Format_MainSubject(keyText) +
                Format_WordSpecificity(keyExtraInfoNote) +
                Format_PlainText(keyNote);
        }

        private string BuildValueForCard(Card card, bool placeExtraInfoWithCardValue)
        {
            // Main subject
            string mainSubject = (mCardKey == CardKey.kEnglish ? card.GetKeyResult(CardKey.kKana) : card.GetKeyResult(CardKey.kEnglish));

            // Extra info for key, but only if we ARE a multi answer card
            string valueExtraInfoNote = "";
            if (placeExtraInfoWithCardValue && card.GetExtraInformation().Length > 0)
            {
                valueExtraInfoNote = card.GetExtraInformation();
            }

            string chapterNum = "";
            if (mDeckSerializer.HasExtraOptionEnabled(StudyOptions.Option.kIncludeChapterSourceInResult) && card.GetBookAndChapterNum().IsValid())
            {
                chapterNum = card.GetBookAndChapterNum().ToString();
            }

            // Secondary subject
            string secondarySubject = (mCardKey == CardKey.kKanji ? card.GetKeyResult(CardKey.kKana) : card.GetKeyResult(CardKey.kKanji));
            if (secondarySubject.Equals(mainSubject))
            {
                secondarySubject = "";
            }

            // Kanji links
            List<string> kanjiLinks = card.GetKanjiLinks();

            // Word forms
            // Special case, calls to format are handled here for convenience
            List<WordForm> enabledWordForms = new List<WordForm>();
            card.GetEnabledWordForms(ref enabledWordForms);

            string wordFormText = "";
            if (enabledWordForms.Count > 0)
            {
                wordFormText = "";
                WordFormCategory lastCategorySeen = WordFormCategory.kBeginner;

                foreach (WordForm wordForm in enabledWordForms)
                {
                    if (wordForm.IsTypeAndPoliteness(WordFormType.kPerfect, WordPoliteness.kPolite))
                    {
                        continue; // The mainSubject is expected to be perfect polite
                    }

                    if (string.IsNullOrEmpty(wordFormText) || wordForm.GetWordFormCategory() != lastCategorySeen)
                    {
                        wordFormText += kLineBreak;
                        lastCategorySeen = wordForm.GetWordFormCategory();
                    }

                    if (wordForm.TryGetDescription(out string wordFormDescription))
                    {
                        wordFormText += Format_InfoLine(wordFormDescription, wordForm.GetWord());
                    }
                    else
                    {
                        wordFormText += Format_PlainText(wordForm.GetWord());
                    }
                }
            }

            // Finish
            return 
                Format_PreNote(mSharedPreNote) +
                Format_MainSubject(mainSubject) +
                Format_WordSpecificity(valueExtraInfoNote) +
                Format_WordSpecificity(chapterNum) +
                Format_PlainText(secondarySubject) +
                Format_InfoLine("漢字", string.Join(" , ", kanjiLinks)) +
                wordFormText;
        }
    }
}
