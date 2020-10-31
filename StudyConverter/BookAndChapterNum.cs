using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihongoStudyBuilder.StudyConverter
{
    class BookAndChapterNum
    {
        public int mBookNum;
        public int mChapterNum;

        public BookAndChapterNum()
        {
            mBookNum = -1;
            mChapterNum = -1;
        }

        public BookAndChapterNum(int bookNum, int chapterNum)
        {
            mBookNum = bookNum;
            mChapterNum = chapterNum;
        }

        public bool IsValid()
        {
            return mBookNum != -1 && mChapterNum != -1;
        }

        public bool IsGreaterThan(BookAndChapterNum other)
        {
            if (mBookNum > other.mBookNum)
            {
                return true;
            }
            if (mBookNum == other.mBookNum)
            {
                return mChapterNum > other.mChapterNum;
            }
            return false;
        }

        public bool IsEqualTo(BookAndChapterNum other)
        {
            return mBookNum == other.mBookNum && mChapterNum == other.mChapterNum;
        }

        public int CompareTo(BookAndChapterNum other)
        {
            if (other == null)
            {
                return 1;
            }
            if (mBookNum == other.mBookNum)
            {
                return mChapterNum.CompareTo(other.mChapterNum);
            }
            return mBookNum.CompareTo(other.mBookNum);
        }

        override public string ToString()
        {
            if (IsValid())
            {
                return string.Format("{0}-{1}", mBookNum, mChapterNum);
            }
            return "Invalid";
        }
    }
}
