using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihongoStudyBuilder.StudyConverter
{
    public static class CharacterTools
    {
// public: // static
        static public bool IsKanji(char c)
        {
            return (c >= 0x4E00) && (c <= 0x9FBF);
        }

        static public bool IsHiraganaSoundE(char c)
        {
            return
                (c == 'ぇ') ||
                (c == 'え') ||
                (c == 'け') ||
                (c == 'げ') ||
                (c == 'せ') ||
                (c == 'ぜ') ||
                (c == 'て') ||
                (c == 'で') ||
                (c == 'ね') ||
                (c == 'へ') ||
                (c == 'べ') ||
                (c == 'ぺ') ||
                (c == 'め') ||
                (c == 'れ');
        }

        static public bool IsHiraganaSoundI(char c)
        {
            return
                (c == 'ぃ') ||
                (c == 'い') ||
                (c == 'き') ||
                (c == 'ぎ') ||
                (c == 'し') ||
                (c == 'じ') ||
                (c == 'ち') ||
                (c == 'ぢ') ||
                (c == 'に') ||
                (c == 'ひ') ||
                (c == 'び') ||
                (c == 'ぴ') ||
                (c == 'み') ||
                (c == 'り');
        }
        
        static public bool IsLastKanjiIchidanExceptionE(char c)
        {
            return
                (c == '帰');
        }

        static public bool IsLastKanjiIchidanExceptionI(char c)
        {
            return
                (c == '切') ||
                (c == '入') ||
                (c == '知') ||
                (c == '要');
        }

        static public char ConvertKanaVowelSound(char c, char desiredVowel, bool forVerbUsage = false)
        {
            int codePoint = c;
            if (codePoint < 0x3040 || codePoint > 0x30F2)
            {
                return c;
            }

            bool isKatakana = false;
            if (codePoint >= 0x30A0)
            {
                isKatakana = true;
                codePoint = codePoint - 0x30A0;
            }
            else
            {
                codePoint = codePoint - 0x3040;
            }

            // あ = 0, い = 1, う = 2, え = 3, お = 4
            int desiredCodePointV = desiredVowel;
            desiredCodePointV = ((desiredCodePointV - 0x3040) >> 1) - 1;

            int newCodePoint = 0x00;
            if (codePoint >= 0x01 && codePoint <= 0x0A) // あ
            {
                if (forVerbUsage && desiredCodePointV == 0)
                {
                    return 'わ';
                }
                newCodePoint = 0x01 + (2 * desiredCodePointV) + ((codePoint + 1) % 2);
            }
            else if (codePoint >= 0x0B && codePoint <= 0x14) // か
            {
                newCodePoint = 0x0B + (2 * desiredCodePointV) + ((codePoint + 1) % 2);
            }
            else if (codePoint >= 0x15 && codePoint <= 0x1E) // さ
            {
                newCodePoint = 0x15 + (2 * desiredCodePointV) + ((codePoint + 1) % 2);
            }
            else if (codePoint >= 0x1F && codePoint <= 0x22) // た -> ぢ
            {
                // っ (small つ) requires an extra skip
                newCodePoint = 0x1F + (2 * desiredCodePointV) + ((codePoint + 1) % 2) + (desiredCodePointV >= 2 ? 1 : 0);
            }
            else if (codePoint >= 0x24 && codePoint <= 0x29) // つ -> ど
            {
                // っ (small つ) requires an extra skip
                newCodePoint = 0x1F + (2 * desiredCodePointV) + (codePoint % 2) + (desiredCodePointV >= 2 ? 1 : 0);
            }
            else if (codePoint >= 0x2A && codePoint <= 0x2E) // な
            {
                newCodePoint = 0x2A + desiredCodePointV;
            }
            else if (codePoint >= 0x2F && codePoint <= 0x3D) // は
            {
                newCodePoint = 0x2F + (3 * desiredCodePointV) + ((codePoint + 1) % 3);
            }
            else if (codePoint >= 0x3E && codePoint <= 0x42) // ま
            {
                newCodePoint = 0x3E + desiredCodePointV;
            }
            else if (codePoint >= 0x43 && codePoint <= 0x48) // ゃ
            {
                if (desiredCodePointV == 1) { return c; }
                if (desiredCodePointV == 2) { desiredCodePointV -= 1; }
                if (desiredCodePointV == 3) { return c; }
                if (desiredCodePointV == 4) { desiredCodePointV -= 2; }

                newCodePoint = 0x43 + (2 * desiredCodePointV) + ((codePoint + 1) % 2);
            }
            else if (codePoint >= 0x49 && codePoint <= 0x4D) // ら
            {
                newCodePoint = 0x49 + desiredCodePointV;
            }
            else if (codePoint >= 0x4F && codePoint <= 0x52) // わ
            {
                if (desiredCodePointV == 0) { return 'わ'; }
                if (desiredCodePointV == 4) { return 'を'; }
                return c;
            }
            else
            {
                return c;
            }

            newCodePoint += (isKatakana ? 0x30A0 : 0x3040);
            return (char)newCodePoint;
        }
    }
}
