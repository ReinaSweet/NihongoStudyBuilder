using NihongoStudyBuilder.StudyConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace NihongoStudyBuilder
{
    public partial class App : Form
    {
        private const string kLKGBookFileName = "Book.tsv";
        private const string kTempBookFileName = "TempBook.tsv";
        private const string kGoogleSheetSourceURL = 
            "https://docs.google.com/spreadsheets/d/1-ump-ACjs2j4WGTKWT2qJkoyH_FA4cbrRf_N_g-Bytk/export?format=tsv&gid=1777059317";

// public:
        public App()
        {
            InitializeComponent();
        }

// private:
        private DeckSerializer mDeckSerializer;
        private List<BookAndChapterNum> mBookAndChapterNumbers;
        private string mAggregateFilterDownTo = "";

        private void Initialize()
        {
            string[] defaultAggregateCheckedOptions =
            {
                "Plain Form",
                "Randomize",
            };
            CheckEachOptionInUI(defaultAggregateCheckedOptions, mAggregateEnabledOptions);

            string[] defaultChapterCheckedOptions =
            {
                "Plain Form",
            };
            CheckEachOptionInUI(defaultChapterCheckedOptions, mChapterEnabledOptions);

            LoadDeckSerializer();
        }

        private void LoadDeckSerializer()
        {
            // Set messages
            mLoadResults.Text = "Loading...";
            mInfoGrid.Rows.Clear();

            // Attempt to download sheet into our temp location
            // If download fails, default to LKG
            string fileToUse;
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(kGoogleSheetSourceURL, kTempBookFileName);
                }
                fileToUse = kTempBookFileName;
            }
            catch (Exception)
            {
                fileToUse = kLKGBookFileName;
            }

            // We can't do anything if we both failed to download and had no available Last Known Good
            if (!File.Exists(fileToUse))
            {
                mLoadResults.Text = "Failed download with no available LKG";
                return;
            }

            // Load serializer, this will throw on fail
            mDeckSerializer = new DeckSerializer(fileToUse, "みんなの日本語");
            mDeckSerializer.LoadDecks();

            List<Deck> invalidDecks = mDeckSerializer.GetInvalidDecks();
            bool hasErrors = false;
            bool hasEmptyDecks = false;

            if (invalidDecks.Count > 0)
            {
                foreach (Deck deck in invalidDecks)
                {
                    List<string> errors = deck.GetErrors();
                    if (errors.Count > 0)
                    {
                        hasErrors = true;
                        foreach (string error in errors)
                        {
                            AddToInfoGrid(deck, error, true);
                        }
                    }
                    else if (deck.GetCardCount() == 0)
                    {
                        hasEmptyDecks = true;
                        AddToInfoGrid(deck, "Empty deck");
                    }
                    else
                    {
                        hasErrors = true;
                        AddToInfoGrid(deck, "Marked as invalid, but unable to identify why", true);
                    }
                }
            }

            if (hasErrors)
            {
                mLoadResults.Text = "Errors present; fully valid decks still loaded.";
            }
            else
            {
                mLoadResults.Text = (hasEmptyDecks ? "Success, but with empty decks" : "Success!");

                // Serializer loaded successfully (we didn't throw an exception during it)
                // If we used a file other than LKG, swap
                if (fileToUse != kLKGBookFileName)
                {
                    if (File.Exists(kLKGBookFileName))
                    {
                        File.Delete(kLKGBookFileName);
                    }
                    File.Move(fileToUse, kLKGBookFileName);
                }
            }

            // Update our interface with the new values
            mBookAndChapterNumbers = mDeckSerializer.GetBookAndChapterNumbers();
            mTrackBarMin.Maximum = mBookAndChapterNumbers.Count - 1;
            mTrackBarMax.Maximum = mTrackBarMin.Maximum;
            mTrackBarMax.Value = mTrackBarMin.Maximum;
            UpdateAggregateDisplay();
        }

        private void WriteChapterDecks()
        {
            EnablePermanentExtraOptionsFromUI(mChapterEnabledOptions);

            List<Deck> chapterDecks = mDeckSerializer.GetChapterDecks();
            ResetProgressBar(chapterDecks.Count);

            foreach (Deck deck in chapterDecks)
            {
                mDeckSerializer.WriteSingleDeck(deck);
                IncrementBar();
            }

            mDeckSerializer.ClearPermanentExtraOptions();
        }

        private void WriteAggregateDecks()
        {
            string aggregateDeckName;
            if (TryBuildAggregateDeckName(out aggregateDeckName))
            {
                EnablePermanentExtraOptionsFromUI(mAggregateEnabledOptions);

                const int totalSteps = 5;
                ResetProgressBar(totalSteps);

                // 1 : Aggregate all specified decks into a list
                List<Deck> sourceDecks = new List<Deck>();
                for (int i = mTrackBarMin.Value; i <= mTrackBarMax.Value; ++i)
                {
                    BookAndChapterNum selectedChapter = mBookAndChapterNumbers[i];
                    mDeckSerializer.AddAllDecksOfQualifyingBookAndChapter(selectedChapter, ref sourceDecks);
                }
                IncrementBar();
                
                // 2 : Combine the decks into a mega-deck
                Deck combinedDeck = new Deck(mDeckSerializer);
                combinedDeck.InitializeFromDeckList(sourceDecks, aggregateDeckName);
                IncrementBar();

                // 3 : Filter mega-deck down to selected card type
                CardType filterDownTo = CardType.kInvalid;
                switch (mAggregateFilterDownTo)
                {
                    case "Verbs":
                        filterDownTo = CardType.kVerb;
                        break;

                    case "Adjectives":
                        filterDownTo = CardType.kAdjective;
                        break;

                    case "Phrases":
                        filterDownTo = CardType.kPhrase;
                        break;
                }

                if (filterDownTo != CardType.kInvalid)
                {
                    combinedDeck.FilterCardsDownTo(filterDownTo);
                }
                IncrementBar();

                // 4 : Randomize deck
                if (mDeckSerializer.HasExtraOptionEnabled(StudyOptions.Option.kRandomize))
                {
                    combinedDeck.RandomizeDeck();
                }
                IncrementBar();

                // 5 : Write Deck
                mDeckSerializer.SetPermanentExtraOption(StudyOptions.Option.kIncludeChapterSourceInResult);
                mDeckSerializer.WriteSingleDeck(combinedDeck);
                IncrementBar();

                mDeckSerializer.ClearPermanentExtraOptions();
            }
        }

        private void UpdateAggregateDisplay()
        {
            string aggregateDeckName;
            TryBuildAggregateDeckName(out aggregateDeckName);
            mAggregateRangeDisplay.Text = aggregateDeckName;
        }

        private bool TryBuildAggregateDeckName(out string aggregateDeckName)
        {
            if (mTrackBarMin.Value > mTrackBarMax.Value)
            {
                aggregateDeckName = "Invalid: Min greater than Max";
                return false;
            }

            aggregateDeckName = "";
            if (!string.IsNullOrEmpty(mAggregateFilterDownTo) && !mAggregateFilterDownTo.Equals("Everything"))
            {
                aggregateDeckName = mAggregateFilterDownTo + " ";
            }

            BookAndChapterNum minValue = mBookAndChapterNumbers[mTrackBarMin.Value];
            if (mTrackBarMin.Value == mTrackBarMax.Value)
            {
                aggregateDeckName += minValue.ToString();
                return true;
            }

            BookAndChapterNum maxValue = mBookAndChapterNumbers[mTrackBarMax.Value];
            aggregateDeckName += string.Format("{0} → {1}", minValue.ToString(), maxValue.ToString());
            return true;
        }

        private void EnablePermanentExtraOptionsFromUI(CheckedListBox sourcedBox)
        {
            foreach (var item in sourcedBox.CheckedItems)
            {
                StudyOptions.Option option;
                if (StudyOptions.TryGetOptionFromText(item.ToString(), out option))
                {
                    mDeckSerializer.SetPermanentExtraOption(option);
                }
            }
        }

        private void CheckEachOptionInUI(string[] optionsToCheck, CheckedListBox checkedListBox)
        {
            foreach (string option in optionsToCheck)
            {
                int idx = checkedListBox.Items.IndexOf(option);
                if (idx != -1)
                {
                    checkedListBox.SetItemChecked(idx, true);
                }
            }
        }

        // Progress Bar
        private void ResetProgressBar(int stagesNeeded)
        {
            mProgressBar.Value = 0;
            mProgressBar.Maximum = stagesNeeded;
            Application.DoEvents();
        }

        private void IncrementBar()
        {
            mProgressBar.Value += 1;
            Application.DoEvents();
        }

        // Info Grid
        private void AddToInfoGrid(Deck deck, string message, bool isError = false)
        {
            int idx = mInfoGrid.Rows.Add();
            mInfoGrid.Rows[idx].Cells["mInfoGridChapter"].Value = deck.GetBookAndChapterNumber().ToString();
            mInfoGrid.Rows[idx].Cells["mInfoGridMessage"].Value = message;
            if (isError)
            {
                mInfoGrid.Rows[idx].DefaultCellStyle.BackColor = Color.IndianRed;
            }
        }

// events:
        private void mApp_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void mBtnWriteChapterDecks_Click(object sender, EventArgs e)
        {
            WriteChapterDecks();
        }

        private void mBtnWriteAggregateDecks_Click(object sender, EventArgs e)
        {
            WriteAggregateDecks();
        }

        private void mTrackBar_ValueChanged(object sender, EventArgs e)
        {
            UpdateAggregateDisplay();
        }

        private void mAggRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;
            mAggregateFilterDownTo = radio.Text;
            UpdateAggregateDisplay();
        }

        private void mReloadDeckSerializer_Click(object sender, EventArgs e)
        {
            LoadDeckSerializer();
        }
    }
}
