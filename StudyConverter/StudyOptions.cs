using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihongoStudyBuilder.StudyConverter
{
    class StudyOptions
    {
        public enum Option
        {
            kInvalid,
            kPlainForm,
            kRandomize,
            kIncludeChapterSourceInResult,
        }

        public struct OptionReq
        {
            public Option mOption;
            public bool mEnabled;

            public OptionReq(Option option, bool enabled)
            {
                mOption = option;
                mEnabled = enabled;
            }
        }

        public static bool TryGetOptionFromText(string optionName, out Option option)
        {
            option = Option.kInvalid;
            if (Enum.TryParse<Option>("k" + optionName.Replace(" ", ""), out option))
            {
                return true;
            }
            return false;
        }
    }
}
