using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Books.Domain.Abstract;
using Books.Domain.Concrete;
using Books.Domain.Entities;
using Books.WebUI.Models;
using Books.WebUI.Infrastructure;

namespace Books.WebUI.Controllers
{
    public class SummaryController : Controller
    {

        private INodeRepository repository;

        public SummaryController(INodeRepository repo)
        {
            repository = repo;
        }

        public ViewResult Edit(int NodeID)
        {
            int linesNoOf, sentencesNoOf, paragraphsNoOf, newNoOfParagraphs;
            string p;
            List<string> lines, sentences, paragrphs, para, newParagraphs;
            Paragraphs paragraphs;
            List<int> sentenceInParagraph;
            List<bool> selectedSentences;
            IEnumerable<Node> nodes;
            IEnumerable<Picture> pictures;

            bool haspict = false;
            bool hasnofigpara = true;
            bool hasnotabpara = true;
            bool itHasATableHeading = false;
            bool tableStarted = false;
            bool NodeIDOK = false;
            int listStartedAt = -1;
            int tableStartedAt = -1;

            sentenceInParagraph = new List<int>();
            selectedSentences = new List<bool>();
            paragrphs = new List<string>();
            para = new List<string>();
            newParagraphs = new List<string>();
            sentences = new List<string>();
            lines = new List<string>();
            paragraphs = new Paragraphs();

            Node node = repository.Nodes.Single(n => n.NodeID == NodeID);
            paragraphs.TheText = node.NodeText;
            paragraphs.NoOfChars = paragraphs.TheText.Length;
            paragraphs.Paragrphs(out paragraphsNoOf, ref paragrphs, out sentencesNoOf, ref sentences, ref sentenceInParagraph, out linesNoOf, ref lines, 0, false, true, true, false, true, false, true);

            for (int i = 0; i < sentencesNoOf; i++)
            {
                selectedSentences.Add(false);
            }

            para.Add(" ");

            Summary summary = new Summary();
            summary.NodeID = NodeID;
            summary.Summary1 = node.Heading + ":- \r\n";

            nodes = repository.Nodes.Where(n => n.NodeID == NodeID);
            pictures = repository.Pictures.Where(pic => pic.NodeID == NodeID);
            if (pictures.Count() > 0)
            {
                haspict = true;
            }

            newNoOfParagraphs = 0;
            for (int i = 0; i < paragraphsNoOf; i++)
            {
                p = paragrphs.ToArray()[i];
                if (p.Length > 2)
                {
                    if (p.Substring(0, 3).ToLower() == "fig") { hasnofigpara = false; }
                    if (p.Substring(0, 3).ToLower() == "tab")
                    {
                        hasnotabpara = false;
                        itHasATableHeading = true;
                        tableStarted = false;
                    }
                }
                if (p.Substring(0, 1) == "¤" || p.Substring(0, 1) == "¥" || p.IndexOf((Char)9) > 0)
                {
                    if (p.Substring(0, 1) == "¤" || p.Substring(0, 1) == "¥")
                    {
                        tableStartedAt = -1;
                        if (listStartedAt == -1)
                        {
                            listStartedAt = newNoOfParagraphs;
                            newParagraphs.Add("");
                            newNoOfParagraphs++;
                        }
                        if (listStartedAt > -1)
                        {
                            newParagraphs[listStartedAt] += p;
                        }
                    }
                    if (p.IndexOf((Char)9) > 0)
                    {
                        if (itHasATableHeading)
                        {
                            listStartedAt = -1;
                            if (tableStartedAt == -1)
                            {
                                tableStarted = true;
                                tableStartedAt = newNoOfParagraphs;
                                newParagraphs.Add("");
                                newNoOfParagraphs++;
                            }
                            if (tableStartedAt > -1)
                            {
                                if (p.Substring(0, 1) == "§")
                                {
                                    newParagraphs[tableStartedAt] += "£" + p.Substring(1);
                                }
                                else
                                {
                                    newParagraphs[tableStartedAt] += "£" + p;
                                }
                            }
                        }
                        else
                        {
                            listStartedAt = -1;
                            tableStartedAt = -1;
                            if (p.Length > 2)
                            {
                                if (p != "§\r\n")
                                {
                                    newNoOfParagraphs++;
                                    newParagraphs.Add(p);
                                }
                            }
                            if (tableStarted)
                            {
                                itHasATableHeading = false;
                                tableStarted = false;
                            }
                        }
                    }
                }
                else
                {
                    listStartedAt = -1;
                    tableStartedAt = -1;
                    if (p.Length > 2)
                    {
                        if (p != "§\r\n")
                        {
                            newNoOfParagraphs++;
                            newParagraphs.Add(p);
                        }
                    }
                    if (tableStarted)
                    {
                        itHasATableHeading = false;
                        tableStarted = false;
                    }
                }
            }
            BookIndexViewModel model = new BookIndexViewModel
            {
                Node = nodes.FirstOrDefault(),
                Summary = summary,
                SentencesNoOf = sentencesNoOf, 
                Sentences = sentences,
                SentenceInParagraph = sentenceInParagraph,
                SelectedSentences = selectedSentences,
                Paragraphs = para,
                NoOfParagraphs = 1,
                Paragraph = "",
                HasPicture = haspict,
                NoOfPictures = pictures.Count(),
                PicturePointer = 0,
                Pictures = pictures,
                PictureTitle = "",
                PictureFile = null,
                PictureFixed = false,
                HasSummary = false,
                HasChildren = false,
                NoOfChildren = 0,
                HasParent = false,
                HasNoFigPara = false,
                HasNoTabPara = false,
                ShowingDetails = true,
                ShowingSummary = false,
                SearchKey = ""
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SummaryEditViewModel summ, List<string> Sent, List<bool> Selected, bool undo = false)
        {

            int linesNoOf, sentencesNoOf, paragraphsNoOf, paragraphPtr, newNoOfParagraphs;
            string p;
            List<string> lines, sentences, paragrphs, newParagraphs;
            Paragraphs paragraphs;
            List<int> sentenceInParagraph;
            List<bool> selectedSentences;
            IEnumerable<Node> nodes;
            IEnumerable<Picture> pictures;

            bool haspict = false;
            bool hasnofigpara = true;
            bool hasnotabpara = true;
            bool itHasATableHeading = false;
            bool tableStarted = false;
            bool NodeIDOK = false;
            int listStartedAt = -1;
            int tableStartedAt = -1;

            sentenceInParagraph = new List<int>();
            selectedSentences = new List<bool>();
            paragrphs = new List<string>();
            sentences = new List<string>();
            lines = new List<string>();
            newParagraphs = new List<string>();
            paragraphs = new Paragraphs();
            paragraphPtr = 0;
            paragraphs.TheText = summ.Summary.Summary1;

            for (int i = 0; i < summ.SentencesNoOf; i++)
            {
                if (summ.SentenceInParagraph[i] != paragraphPtr)
                {
                    paragraphs.TheText += "\r\n";
                    paragraphPtr = summ.SentenceInParagraph[i];
                }
                if (summ.SelectedSentences[i])
                {
                    paragraphs.TheText += summ.Sentences[i] + " ";
                }
            }

            paragraphs.NoOfChars = paragraphs.TheText.Length;
            paragraphs.Paragrphs(out paragraphsNoOf, ref paragrphs, out sentencesNoOf, ref sentences, ref sentenceInParagraph, out linesNoOf, ref lines, 0, false, true, true, false, true, false, true);

            summ.Summary.Summary1 = paragraphs.TheAlteredText;

            nodes = repository.Nodes.Where(n => n.NodeID == summ.Summary.NodeID);
            pictures = repository.Pictures.Where(pic => pic.NodeID == summ.Summary.NodeID);
            if (pictures.Count() > 0)
            {
                haspict = true;
            }

            newNoOfParagraphs = 0;
            for (int i = 0; i < paragraphsNoOf; i++)
            {
                p = paragrphs.ToArray()[i];
                if (p.Length > 2)
                {
                    if (p.Substring(0, 3).ToLower() == "fig") { hasnofigpara = false; }
                    if (p.Substring(0, 3).ToLower() == "tab")
                    {
                        hasnotabpara = false;
                        itHasATableHeading = true;
                        tableStarted = false;
                    }
                }
                if (p.Substring(0, 1) == "¤" || p.Substring(0, 1) == "¥" || p.IndexOf((Char)9) > 0)
                {
                    if (p.Substring(0, 1) == "¤" || p.Substring(0, 1) == "¥")
                    {
                        tableStartedAt = -1;
                        if (listStartedAt == -1)
                        {
                            listStartedAt = newNoOfParagraphs;
                            newParagraphs.Add("");
                            newNoOfParagraphs++;
                        }
                        if (listStartedAt > -1)
                        {
                            newParagraphs[listStartedAt] += p;
                        }
                    }
                    if (p.IndexOf((Char)9) > 0)
                    {
                        if (itHasATableHeading)
                        {
                            listStartedAt = -1;
                            if (tableStartedAt == -1)
                            {
                                tableStarted = true;
                                tableStartedAt = newNoOfParagraphs;
                                newParagraphs.Add("");
                                newNoOfParagraphs++;
                            }
                            if (tableStartedAt > -1)
                            {
                                if (p.Substring(0, 1) == "§")
                                {
                                    newParagraphs[tableStartedAt] += "£" + p.Substring(1);
                                }
                                else
                                {
                                    newParagraphs[tableStartedAt] += "£" + p;
                                }
                            }
                        }
                        else
                        {
                            listStartedAt = -1;
                            tableStartedAt = -1;
                            if (p.Length > 2)
                            {
                                if (p != "§\r\n")
                                {
                                    newNoOfParagraphs++;
                                    newParagraphs.Add(p);
                                }
                            }
                            if (tableStarted)
                            {
                                itHasATableHeading = false;
                                tableStarted = false;
                            }
                        }
                    }
                }
                else
                {
                    listStartedAt = -1;
                    tableStartedAt = -1;
                    if (p.Length > 2)
                    {
                        if (p != "§\r\n")
                        {
                            newNoOfParagraphs++;
                            newParagraphs.Add(p);
                        }
                    }
                    if (tableStarted)
                    {
                        itHasATableHeading = false;
                        tableStarted = false;
                    }
                }
            }


            BookIndexViewModel model = new BookIndexViewModel
            {
                Node = nodes.FirstOrDefault(),
                Summary = summ.Summary,
                SentencesNoOf = summ.SentencesNoOf,
                Sentences = summ.Sentences,
                SentenceInParagraph = summ.SentenceInParagraph,
                SelectedSentences = summ.SelectedSentences,
                Paragraphs = paragrphs,
                NoOfParagraphs = paragraphsNoOf,
                Paragraph = "",
                HasPicture = haspict,
                NoOfPictures = pictures.Count(),
                PicturePointer = 0,
                Pictures = pictures,
                PictureTitle = "",
                PictureFile = null,
                PictureFixed = false,
                HasSummary = false,
                HasChildren = false,
                NoOfChildren = 0,
                HasParent = false,
                HasNoFigPara = false,
                HasNoTabPara = false,
                ShowingDetails = true,
                ShowingSummary = false,
                SearchKey = ""
            };
            if (ModelState.IsValid)
            {
                if (summ.Summary.SummaryID == 0)
                {
                    //repository.Edit(summ.Summary);
                    TempData["message"] = string.Format("Summary: {0} ... has been created for Node: {1}", summ.Summary.SummaryID, summ.Summary.NodeID);
                }
                else
                {
                    //repository.Edit(summ.Summary);
                    TempData["message"] = string.Format("Summary: {0} ... has been editted for Node: {1}", summ.Summary.SummaryID, summ.Summary.NodeID);
                }
                //return RedirectToAction("Index", "Book", new { NodeID = summ.Summary.NodeID });
                return View(model);
            }
            else
            {
                // there is something wrong with the data values
                return View(model);
            }
        }
	}
}