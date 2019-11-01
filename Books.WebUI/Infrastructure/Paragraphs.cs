using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Books.WebUI.Infrastructure
{
    public class Paragraphs : Text
    {
        public Paragraphs()
            : base()
        {
        }

        public int NoOfParagraphs { get; set; }
        public string TheAlteredText { get; set; }
        public int AlteredNoOfParagraphs { get; set; }

        int txtPtr, linePtr, paragraphStart, paragraphEnd, paragraphLength;
        string txtLetter, paragraph, debugText;
        Text Txt;

        public void Words(int WordsNoOf, List<string> Words)
        {
            // Subs called:- None
            // Properties Altered:- None
            int wordPtr;
            string word;
            SortedList<string, int> wordsDistinct;
            wordsDistinct = new SortedList<string, int>();
            this.TheAlteredText = "";
            WordsNoOf = 0;
            this.DivideIntoWords(this.TheText, out WordsNoOf, ref Words);
            for (wordPtr = 0; wordPtr <= Words.Count - 1; wordPtr++)
            {
                word = Words[wordPtr].ToString();
                if (wordsDistinct.ContainsKey(word))
                {
                    wordsDistinct[word] += 1;
                }
                else
                {
                    wordsDistinct[word] = 1;
                }
            }
            this.TheAlteredText = "There were " + Words.Count + " words in text with the following frequencies:-" + (Char)13 + (Char)10 + (Char)13 + (Char)10;
            foreach (KeyValuePair<string, int> element in wordsDistinct)
            {
                string wrd = element.Key;
                int cnt = element.Value;
                this.TheAlteredText += wrd + ": " + cnt + (Char)13 + (Char)10;
            }
        }

        public void Sentences(out int SentencesNoOf, ref List<string> Sentences, ref List<int> SentenceInParagraph, int LineWidth, bool Debug, bool EliminateWhiteSpace, bool tabs, bool SplitOnLF)
        {
            // Subs called:- None
            // Properties Altered:- None
            int ParagraphsNoOf, LinesNoOf;
            string debugText, SentenceNo, Sentence, Paragraph;
            List<string> Paragraphs, Lines;
            Paragraphs = new List<string>();
            Lines = new List<string>();
            SentenceInParagraph = new List<int>();
            debugText = "";
            ParagraphsNoOf = 0;
            LinesNoOf = 0;
            this.DivideText(out ParagraphsNoOf, ref Paragraphs, out SentencesNoOf, ref Sentences, ref SentenceInParagraph, out LinesNoOf, ref Lines, out debugText, LineWidth, Debug, true, true, true, false, SplitOnLF, false);
            this.TheAlteredText = "There were " + SentencesNoOf.ToString() + " sentences in text these are:-" + (Char)13 + (Char)10;
            this.TheAlteredText = "There were " + Sentences.Count + " Sentences in text" + (Char)13 + (Char)10 + (Char)13 + (Char)10;
            for (int i = 0; i <= SentencesNoOf - 1; i++)
            {
                SentenceNo = i.ToString();
                Sentence = Sentences[i];
                Paragraph = SentenceInParagraph[i].ToString();
                this.TheAlteredText += SentenceNo + " - " + Sentence + (Char)13 + (Char)10 + "This Sentence is in paragraph # " + Paragraph + (Char)13 + (Char)10 + (Char)13 + (Char)10;
            }
            if (Debug)
            {
                this.TheAlteredText += "Debug Text : -" + (Char)13 + (Char)10 + (Char)13 + (Char)10 + debugText + (Char)13 + (Char)10 + (Char)13 + (Char)10;
            }
        }

        public void Paragrphs(out int ParagraphsNoOf, ref List<string> Paragraphs, out int SentencesNoOf, ref List<string> Sentences, ref List<int> SentenceInParagraph, out int LinesNoOf, ref List<string> Lines, int LineWidth, bool Debug, bool EliminateWhiteSpace, bool tabs, bool SplitHeaders, bool SplitOnColon, bool SplitOnLF, bool InsertIndicators)
        {
            // Subs called:- DivideText
            // Properties Altered:- TheAlteredText
            int paragraphPtr, sentencePtr, linePtr;
            string debugText;
            linePtr = 0;
            sentencePtr = 0;
            paragraphPtr = 0;
            this.DivideText(out ParagraphsNoOf, ref Paragraphs, out SentencesNoOf, ref Sentences, ref SentenceInParagraph, out LinesNoOf, ref Lines, out debugText, LineWidth, Debug, EliminateWhiteSpace, tabs, SplitHeaders, SplitOnColon, SplitOnLF, InsertIndicators);
            if (Debug)
            {
                this.TheAlteredText += "There were " + Paragraphs.Count + " Paragraphs in text" + (Char)13 + (Char)10 + (Char)13 + (Char)10;
            }
            foreach (string paragraph in Paragraphs)
            {
                paragraphPtr += 1;
                if (Debug)
                {
                    this.TheAlteredText += paragraphPtr.ToString() + " - " + paragraph;
                }
                else
                {
                    this.TheAlteredText += paragraph;
                }
                if (this.TheAlteredText.Length > 3)
                {
                    // Havent got any CR LF's add two lots to separate paragraphs with a blank line
                    if (this.TheAlteredText.Substring(this.TheAlteredText.Length - 3, 1).ToCharArray()[0] != (Char)10 && this.TheAlteredText.Substring(this.TheAlteredText.Length - 1, 1).ToCharArray()[0] != (Char)10)
                    {
                        this.TheAlteredText = this.TheAlteredText + (Char)13 + (Char)10 + (Char)13 + (Char)10;
                    }
                    // Only got one CR LF so add one so we have a blank line between paragraphs
                    if (this.TheAlteredText.Substring(this.TheAlteredText.Length - 3, 1).ToCharArray()[0] != (Char)10 && this.TheAlteredText.Substring(this.TheAlteredText.Length - 1, 1).ToCharArray()[0] == (Char)10)
                    {
                        this.TheAlteredText = this.TheAlteredText + (Char)13 + (Char)10;
                    }
                }
                if (this.TheAlteredText.Length > 5)
                {
                    // Eliminate double blank lines between paragraphs
                    if (this.TheAlteredText.Substring(this.TheAlteredText.Length - 5, 1).ToCharArray()[0] == (Char)10 && this.TheAlteredText.Substring(this.TheAlteredText.Length - 3, 1).ToCharArray()[0] == (Char)10 && this.TheAlteredText.Substring(this.TheAlteredText.Length - 1, 1).ToCharArray()[0] == (Char)10)
                    {
                        this.TheAlteredText = this.TheAlteredText.Substring(0, this.TheAlteredText.Length - 2);
                    }
                }
            }
            if (Debug)
            {
                this.TheAlteredText += "Debug Text : -" + (Char)13 + (Char)10 + (Char)13 + (Char)10 + debugText + (Char)13 + (Char)10 + (Char)13 + (Char)10;
                this.TheAlteredText += "There were " + Sentences.Count + " Sentences in text" + (Char)13 + (Char)10 + (Char)13 + (Char)10;
                foreach (string sentence in Sentences)
                {
                    sentencePtr += 1;
                    this.TheAlteredText += sentencePtr.ToString() + " - " + sentence.ToString() + (Char)13 + (Char)10 + (Char)13 + (Char)10;
                }
                this.TheAlteredText += "There were " + Lines.Count + " Lines in text" + (Char)13 + (Char)10 + (Char)13 + (Char)10;
                foreach (string line in Lines)
                {
                    linePtr += 1;
                    this.TheAlteredText += linePtr.ToString() + " - " + line.ToString() + (Char)13 + (Char)10 + (Char)13 + (Char)10;
                }
            }
        }
    }
}