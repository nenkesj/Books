using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Books.WebUI.Models;
using Books.WebUI.Infrastructure;

namespace Books.WebUI.Controllers
{
    public class FormulaController: Controller
    {

        public ViewResult Edit(int NodeID)
        {
            Session["formula"] = "<math xmlns=" + '"' + "http://www.w3.org/1998/Math/MathML" + '"' + " display='inline'> </math>";
            Session["undo"] = Session["formula"];
            Session["ident"] = null;
            Session["oper"] = null;
            Session["num"] = null;
            Session["space"] = null;
            Session["text"] = null;
            FormulaEditViewModel model = new FormulaEditViewModel
            {
                NodeID = NodeID,
                ClearFormula = false,
                BothIdent = false,
                BoldIdent = false,
                Ident1 = "",
                Ident2 = "",
                Oper1 = "",
                Oper2 = "",
                BothOper = false,
                Num1 = "",
                Num2 = "",
                Space = "",
                BoldText = false,
                Text = "",
                Insert = "Identifier",
                Target = "Append",
                Block = "inline",
                Algebraic = "None",
                Calculus = "None",
                Ellipses = "None",
                Logic = "None",
                Vector = "None",
                GreekUpper = "None",
                GreekLower = "None"
            };

            return View(model);
        }

        [HttpPost]
        public ViewResult Edit(FormulaEditViewModel form, bool undo = false)
        {
            string searchfor = " </math>";
            bool insert = true;
            StringBuilder sb;

            if (ModelState.IsValid)
            {
                if (undo)
                {
                    sb = new StringBuilder((string)Session["undo"]);
                }
                else
                {
                    sb = new StringBuilder((string)Session["formula"]);
                }

                if (form.ClearFormula)
                {
                    Session["formula"] = "<math xmlns=" + '"' + "http://www.w3.org/1998/Math/MathML" + '"' + " display='inline'> </math>";
                    Session["undo"] = Session["formula"];
                    Session["ident"] = form.Ident1;
                    Session["oper"] = form.Oper1;
                    Session["num"] = form.Num1;
                    Session["space"] = form.Space;
                    Session["text"] = form.Text;
                    sb = new StringBuilder((string)Session["formula"]);
                    form.BothIdent = false;
                    form.BoldIdent = false;
                    form.Ident1 = "";
                    form.Ident2 = "";
                    form.Oper1 = "";
                    form.Oper2 = "";
                    form.BothOper = false;
                    form.Num1 = "";
                    form.Num2 = "";
                    form.Space = "";
                    form.BoldText = false;
                    form.Text = "";
                    form.Insert = "Identifier";
                    form.Target = "Append";
                    form.Block = "inline";
                    form.Algebraic = "None";
                    form.Calculus = "None";
                    form.Ellipses = "None";
                    form.Logic = "None";
                    form.Vector = "None";
                    form.GreekUpper = "None";
                    form.GreekLower = "None";
                }
                else
                {
                    switch (form.Block)
                    {
                        case "Block":
                            if (sb.ToString().IndexOf("inline") > 0)
                            {
                                sb.Replace("inline", "block");
                                insert = false;
                            }
                            break;
                        case "Inline":
                            if (sb.ToString().IndexOf("block") > 0)
                            {
                                sb.Replace("block", "inline");
                                insert = false;
                            }
                            break;
                        default:
                            break;

                    }

                    switch (form.Target)
                    {
                        case "Append":
                            searchfor = " </math>";
                            insert = true;
                            break;
                        case "Numerator":
                            searchfor = " #numerator";
                            insert = true;
                            break;
                        case "Denominator":
                            searchfor = " #denominator";
                            insert = true;
                            break;
                        case "Subscript Row":
                            searchfor = " #subrow";
                            insert = true;
                            break;
                        case "Subscript Row2":
                            searchfor = " #subrow2";
                            insert = true;
                            break;
                        case "Superscript Row":
                            searchfor = " #suprow";
                            insert = true;
                            break;
                        case "Superscript Row2":
                            searchfor = " #suprow2";
                            insert = true;
                            break;
                        case "Square Root Row":
                            searchfor = " #sqrtrow";
                            insert = true;
                            break;
                        case "Root Row":
                            searchfor = " #rootrow";
                            insert = true;
                            break;
                        case "Over Row":
                            searchfor = " #overrow";
                            insert = true;
                            break;
                        case "Under Row":
                            searchfor = " #underrow";
                            insert = true;
                            break;
                        case "Row":
                            searchfor = " #row";
                            insert = true;
                            break;
                        case "Clear Numerator":
                            if (sb.ToString().Contains("#numerator")) { sb.Remove(sb.ToString().IndexOf("#numerator"), 10); };
                            insert = false;
                            form.Target = "Denominator";
                            form.Insert = "Number";
                            break;
                        case "Clear Denominator":
                            if (sb.ToString().Contains("#denominator")) { sb.Remove(sb.ToString().IndexOf("#denominator"), 12); };
                            insert = false;
                            form.Insert = "Operator";
                            break;
                        case "Clear Subscript Row":
                            if (sb.ToString().Contains("#subrow")) { sb.Remove(sb.ToString().IndexOf("#subrow"), 7); };
                            if (sb.ToString().Contains("#subrow2")) { form.Target = "Subscript Row2"; };
                            insert = false;
                            form.Insert = "Operator";
                            break;
                        case "Clear Subscript Row2":
                            if (sb.ToString().Contains("#subrow2")) { sb.Remove(sb.ToString().IndexOf("#subrow2"), 8); };
                            insert = false;
                            form.Insert = "Operator";
                            break;
                        case "Clear Superscript Row":
                            if (sb.ToString().Contains("#suprow")) { sb.Remove(sb.ToString().IndexOf("#suprow"), 7); };
                            if (sb.ToString().Contains("#suprow2")) { form.Target = "Superscript Row2"; };
                            insert = false;
                            form.Insert = "Operator";
                            break;
                        case "Clear Superscript Row2":
                            if (sb.ToString().Contains("#suprow2")) { sb.Remove(sb.ToString().IndexOf("#suprow2"), 8); };
                            insert = false;
                            form.Insert = "Operator";
                            break;
                        case "Clear Square Root Row":
                            if (sb.ToString().Contains("#sqrtrow")) { sb.Remove(sb.ToString().IndexOf("#sqrtrow"), 8); };
                            insert = false;
                            form.Insert = "Operator";
                            break;
                        case "Clear Root Row":
                            if (sb.ToString().Contains("#rootrow")) { sb.Remove(sb.ToString().IndexOf("#rootrow"), 8); };
                            insert = false;
                            form.Insert = "Operator";
                            break;
                        case "Clear Over Row":
                            if (sb.ToString().Contains("#overrow")) { sb.Remove(sb.ToString().IndexOf("#overrow"), 8); };
                            insert = false;
                            form.Insert = "Operator";
                            break;
                        case "Clear Under Row":
                            if (sb.ToString().Contains("#underrow")) { sb.Remove(sb.ToString().IndexOf("#underrow"), 9); };
                            insert = false;
                            form.Target = "Over Row";
                            form.Insert = "Number";
                            break;
                        case "Clear Row":
                            if (sb.ToString().Contains("#row")) { sb.Remove(sb.ToString().IndexOf("#row"), 4); };
                            insert = false;
                            form.Insert = "Operator";
                            break;
                        default:
                            searchfor = " </math>";
                            insert = true;
                            break;
                    }

                    if (undo)
                    {
                        insert = false;
                    }

                    if (insert)
                    {
                        if (form.Algebraic != "None")
                        {
                            form.Oper1 = "&" + form.Algebraic;
                            form.Algebraic = "None";
                        }
                        if (form.Calculus != "None")
                        {
                            if (form.Calculus == "&infin;")
                            {
                                form.Num1 = "&" + "infin;";
                                form.Insert = "Number";
                                form.Calculus = "None";
                            }
                            else
                            {
                                form.Oper1 = "&" + form.Calculus;
                                form.Calculus = "None";
                            }
                        }
                        if (form.Ellipses != "None")
                        {
                            form.Oper1 = "&" + form.Ellipses;
                            form.Insert = "Operator";
                            form.Ellipses = "None";
                        }
                        if (form.Logic != "None")
                        {
                            form.Oper1 = "&" + form.Logic;
                            form.Insert = "Operator";
                            form.Logic = "None";
                        }
                        if (form.Vector != "None")
                        {
                            form.Oper1 = "&" + form.Vector;
                            form.Insert = "Operator";
                            form.Vector = "None";
                        }
                        if (form.GreekUpper != "None")
                        {
                            form.Ident1 = "&" + form.GreekUpper;
                            form.Insert = "Identifier";
                            form.GreekUpper = "None";
                        }
                        if (form.GreekLower != "None")
                        {
                            form.Ident1 = "&" + form.GreekLower;
                            form.Insert = "Identifier";
                            form.GreekLower = "None";
                        }
                        if (form.Insert == "Identifier" || form.Insert == "Operator" || form.Insert == "Number")
                        {
                            if (form.Ident1 != (string)Session["ident"])
                            {
                                form.Insert = "Identifier";
                                Session["ident"] = form.Ident1;
                            }
                            if (form.Oper1 != (string)Session["oper"])
                            {
                                form.Insert = "Operator";
                                Session["oper"] = form.Oper1;
                            }
                            if (form.Num1 != (string)Session["num"])
                            {
                                form.Insert = "Number";
                                Session["num"] = form.Num1;
                            }
                            if (form.Space != (string)Session["space"])
                            {
                                form.Insert = "Space";
                                Session["space"] = form.Space;
                            }
                            if (form.Text != (string)Session["text"])
                            {
                                form.Insert = "Text";
                                Session["text"] = form.Text;
                            }
                        }
                        switch (form.Insert)
                        {
                            case "Identifier":
                                if (sb.ToString().Contains(searchfor))
                                {
                                    if (form.BothNum)
                                    {
                                        sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num1 + "</mn>");
                                        Session["num"] = form.Num1;
                                    }
                                    if (!form.BothIdent)
                                    {
                                        if (form.BoldIdent)
                                        {
                                            sb.Insert(sb.ToString().IndexOf(searchfor), " <mi mathvariant='bold'>" + form.Ident1 + "</mi>");
                                            Session["ident"] = form.Ident1;
                                        }
                                        else
                                        {
                                            sb.Insert(sb.ToString().IndexOf(searchfor), " <mi>" + form.Ident1 + "</mi>");
                                            Session["ident"] = form.Ident1;
                                        }
                                    }
                                    else
                                    {
                                        if (form.BoldIdent)
                                        {
                                            sb.Insert(sb.ToString().IndexOf(searchfor), " <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mi mathvariant='bold'>" + form.Ident2 + "</mi>");
                                            Session["ident"] = form.Ident1;
                                        }
                                        else
                                        {
                                            sb.Insert(sb.ToString().IndexOf(searchfor), " <mi>" + form.Ident1 + "</mi> <mi>" + form.Ident2 + "</mi>");
                                            Session["ident"] = form.Ident1;
                                        }
                                    }
                                }
                                form.Insert = "Operator";
                                break;
                            case "Operator":
                                if (!form.BothOper && sb.ToString().Contains(searchfor)) 
                                { 
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mo>" + form.Oper1 + "</mo>");
                                    Session["oper"] = form.Oper1;
                                }
                                if (form.BothOper && sb.ToString().Contains(searchfor)) 
                                { 
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mo>" + form.Oper1 + "</mo> <mo>" + form.Oper2 + "</mo>");
                                    Session["oper"] = form.Oper1;
                                }
                                form.Insert = "Identifier";
                                break;
                            case "Number":
                                if (sb.ToString().Contains(searchfor)) 
                                { 
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num1 + "</mn>");
                                    Session["num"] = form.Num1;
                                }
                                form.Insert = "Operator";
                                break;
                            case "Subscript (Identifier, Number)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num2 + "</mn>");
                                }
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msub> <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mn>" + form.Num1 + "</mn> </msub>");
                                    Session["ident"] = form.Ident1;
                                    Session["num"] = form.Num1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msub> <mi>" + form.Ident1 + "</mi> <mn>" + form.Num1 + "</mn> </msub>");
                                    Session["ident"] = form.Ident1;
                                    Session["num"] = form.Num1;
                                }
                                form.Insert = "Operator";
                                break;
                            case "Subscript (Identifier, Identifier)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num1 + "</mn>");
                                    Session["num"] = form.Num1;
                                }
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msub> <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mi mathvariant='bold'>" + form.Ident2 + "</mi> </msub>");
                                    Session["ident"] = form.Ident1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msub> <mi>" + form.Ident1 + "</mi> <mi>" + form.Ident2 + "</mi> </msub>");
                                    Session["ident"] = form.Ident1;
                                }
                                form.Insert = "Operator";
                                break;
                            case "Subscript (Identifier, Row)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num1 + "</mn>");
                                    Session["num"] = form.Num1;
                                }
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msub> <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mrow> #subrow </mrow> </msub>");
                                    Session["ident"] = form.Ident1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msub> <mi>" + form.Ident1 + "</mi> <mrow> #subrow </mrow> </msub>");
                                    Session["ident"] = form.Ident1;
                                }
                                form.Target = "Subscript Row";
                                form.Insert = "Identifier";
                                break;
                            case "Subscript (Row, Identifier)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num1 + "</mn>");
                                    Session["num"] = form.Num1;
                                }
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msub> <mrow> #subrow </mrow> <mi mathvariant='bold'>" + form.Ident1 + "</mi> </msub>");
                                    Session["ident"] = form.Ident1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msub> <mrow> #subrow </mrow> <mi>" + form.Ident1 + "</mi> </msub>");
                                    Session["ident"] = form.Ident1;
                                }
                                form.Target = "Subscript Row";
                                form.Insert = "Identifier";
                                break;
                            case "Subscript (Row, Number)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num2 + "</mn>");
                                }
                                if (sb.ToString().Contains(searchfor)) 
                                { 
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msub> <mrow> #subrow </mrow> <mn>" + form.Num1 + "</mn> </msub>");
                                    Session["num"] = form.Num1;
                                }
                                form.Target = "Subscript Row";
                                form.Insert = "Identifier";
                                break;
                            case "Subscript (Row, Row)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num1 + "</mn>");
                                    Session["num"] = form.Num1;
                                }
                                if (sb.ToString().Contains(searchfor)) { sb.Insert(sb.ToString().IndexOf(searchfor), " <msub> <mrow> #subrow </mrow> <mrow> #subrow2 </mrow> </msub>"); }
                                form.Target = "Subscript Row";
                                form.Insert = "Identifier";
                                break;
                            case "Superscript (Identifier, Number)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num2 + "</mn>");
                                }
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msup> <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mn>" + form.Num1 + "</mn> </msup>");
                                    Session["ident"] = form.Ident1;
                                    Session["num"] = form.Num1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msup> <mi>" + form.Ident1 + "</mi> <mn>" + form.Num1 + "</mn> </msup>");
                                    Session["ident"] = form.Ident1;
                                    Session["num"] = form.Num1;
                                }
                                form.Insert = "Operator";
                                break;
                            case "Superscript (Identifier, Identifier)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num1 + "</mn>");
                                    Session["num"] = form.Num1;
                                }
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msup> <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mi>" + form.Ident2 + "</mi> </msup>");
                                    Session["ident"] = form.Ident1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msup> <mi>" + form.Ident1 + "</mi> <mi>" + form.Ident2 + "</mi> </msup>");
                                    Session["ident"] = form.Ident1;
                                }
                                form.Insert = "Operator";
                                break;
                            case "Superscript (Identifier, Row)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num1 + "</mn>");
                                    Session["num"] = form.Num1;
                                }
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msup> <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mrow> #suprow </mrow> </msup>");
                                    Session["ident"] = form.Ident1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msup> <mi>" + form.Ident1 + "</mi> <mrow> #suprow </mrow> </msup>");
                                    Session["ident"] = form.Ident1;
                                }
                                form.Target = "Superscript Row";
                                form.Insert = "Identifier";
                                break;
                            case "Superscript (Row, Identifier)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num1 + "</mn>");
                                    Session["num"] = form.Num1;
                                }
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msup> <mrow> #suprow </mrow> <mi mathvariant='bold'>" + form.Ident1 + "</mi>  </msup>");
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msup> <mrow> #suprow </mrow> <mi>" + form.Ident1 + "</mi>  </msup>");
                                    Session["ident"] = form.Ident1;
                                }
                                form.Target = "Superscript Row";
                                form.Insert = "Identifier";
                                break;
                            case "Superscript (Row, Number)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num2 + "</mn>");
                                }
                                if (sb.ToString().Contains(searchfor)) 
                                { 
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msup> <mrow> #suprow </mrow> <mn>" + form.Num1 + "</mn>  </msup>");
                                    Session["num"] = form.Num1;
                                }
                                form.Target = "Superscript Row";
                                form.Insert = "Identifier";
                                break;
                            case "Superscript (Row, Row)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num1 + "</mn>");
                                    Session["num"] = form.Num1;
                                }
                                if (sb.ToString().Contains(searchfor)) { sb.Insert(sb.ToString().IndexOf(searchfor), " <msup> <mrow> #suprow </mrow> <mrow> #suprow2 </mrow> </msup>"); }
                                form.Target = "Superscript Row";
                                form.Insert = "Identifier";
                                break;
                            case "SubSup (Identifier, Number, Number)":
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msubsup> <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mn>" + form.Num1 + "</mn> <mn>" + form.Num2 + "</mn> </msubsup>");
                                    Session["ident"] = form.Ident1;
                                    Session["num"] = form.Num1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msubsup> <mi>" + form.Ident1 + "</mi> <mn>" + form.Num1 + "</mn> <mn>" + form.Num2 + "</mn> </msubsup>");
                                    Session["ident"] = form.Ident1;
                                    Session["num"] = form.Num1;
                                }
                                form.Insert = "Operator";
                                break;
                            case "SubSup (Identifier, Number, Identifier)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num2 + "</mn>");
                                }
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msubsup> <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mn>" + form.Num1 + "</mn> <mi>" + form.Ident2 + "</mi> </msubsup>");
                                    Session["ident"] = form.Ident1;
                                    Session["num"] = form.Num1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msubsup> <mi>" + form.Ident1 + "</mi> <mn>" + form.Num1 + "</mn> <mi>" + form.Ident2 + "</mi> </msubsup>");
                                    Session["ident"] = form.Ident1;
                                    Session["num"] = form.Num1;
                                }
                                form.Insert = "Operator";
                                break;
                            case "SubSup (Identifier, Identifier, Number)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num2 + "</mn>");
                                }
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msubsup> <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mi>" + form.Ident2 + "</mi> <mn>" + form.Num1 + "</mn> </msubsup>");
                                    Session["ident"] = form.Ident1;
                                    Session["num"] = form.Num1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msubsup> <mi>" + form.Ident1 + "</mi> <mi>" + form.Ident2 + "</mi> <mn>" + form.Num1 + "</mn> </msubsup>");
                                    Session["ident"] = form.Ident1;
                                    Session["num"] = form.Num1;
                                }
                                form.Insert = "Operator";
                                break;
                            case "SubSup (Identifier, Row, Row)":
                                if (form.BothNum && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mn>" + form.Num1 + "</mn>");
                                    Session["num"] = form.Num1;
                                }
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msubsup> <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mrow> #subrow </mrow> <mrow> #suprow </mrow> </msubsup>");
                                    Session["ident"] = form.Ident1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <msubsup> <mi>" + form.Ident1 + "</mi> <mrow> #subrow </mrow> <mrow> #suprow </mrow> </msubsup>");
                                    Session["ident"] = form.Ident1;
                                }
                                form.Target = "Subscript Row";
                                form.Insert = "Identifier";
                                break;
                            case "Row":
                                if (sb.ToString().Contains(searchfor)) { sb.Insert(sb.ToString().IndexOf(searchfor), " <mrow> #row </mrow>"); }
                                form.Target = "Row";
                                form.Insert = "Identifier";
                                break;
                            case "Fraction":
                                if (sb.ToString().Contains(searchfor)) { sb.Insert(sb.ToString().IndexOf(searchfor), " <mstyle mathsize='1.2em'> <mfrac> <mrow> #numerator </mrow> <mrow> #denominator </mrow> </mfrac> </mstyle>"); }
                                form.Target = "Numerator";
                                form.Insert = "Number";
                                break;
                            case "Square Root":
                                if (sb.ToString().Contains(searchfor)) { sb.Insert(sb.ToString().IndexOf(searchfor), " <msqrt> <mrow> #sqrtrow </mrow> </msqrt>"); }
                                form.Target = "Square Root Row";
                                form.Insert = "Number";
                                break;
                            case "Root":
                                if (sb.ToString().Contains(searchfor)) 
                                { 
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mroot> <mrow> #rootrow </mrow> <mn>" + form.Num1 + "</mn></mroot>");
                                    Session["num"] = form.Num1;
                                }
                                form.Target = "Root Row";
                                form.Insert = "Number";
                                break;
                            case "Under Over":
                                if (sb.ToString().Contains(searchfor)) 
                                { 
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <munderover> <mo>" + form.Oper1 + "</mo> <mrow> #underrow </mrow> <mrow> #overrow </mrow> </munderover>");
                                    Session["oper"] = form.Oper1;
                                }
                                form.Target = "Under Row";
                                form.Insert = "Identifier";
                                break;
                            case "Over":
                                if (form.BoldIdent && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mover> <mi mathvariant='bold'>" + form.Ident1 + "</mi> <mo mathvariant='bold'>" + form.Oper1 + "</mo> </mover>");
                                    Session["ident"] = form.Ident1;
                                    Session["oper"] = form.Oper1;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mover> <mi>" + form.Ident1 + "</mi> <mo>" + form.Oper1 + "</mo> </mover>");
                                    Session["ident"] = form.Ident1;
                                    Session["oper"] = form.Oper1;
                                }
                                form.Insert = "Operator";
                                break;
                            case "Fenced 1":
                                sb.Insert(sb.ToString().IndexOf(searchfor), " <mfenced> <mi>" + form.Ident1 + "</mi> </mfenced>");
                                    Session["ident"] = form.Ident1;
                                form.Insert = "Operator";
                                break;
                            case "Fenced 2":
                                sb.Insert(sb.ToString().IndexOf(searchfor), " <mfenced> <mi>" + form.Ident1 + "</mi> <mi>" + form.Ident2 + "</mi> </mfenced>");
                                    Session["ident"] = form.Ident1;
                                form.Insert = "Operator";
                                break;
                            case "Fenced 3":
                                sb.Insert(sb.ToString().IndexOf(searchfor), " <mfenced> <mi>" + form.Ident1 + "</mi> <mi>" + form.Ident2 + "</mi> <mi>" + form.Num1 + "</mi> </mfenced>");
                                    Session["ident"] = form.Ident1;
                                    Session["num"] = form.Num1;
                                form.Insert = "Operator";
                                break;
                            case "Space":
                                if (sb.ToString().Contains(searchfor)) { sb.Insert(sb.ToString().IndexOf(searchfor), " <mspace width=" + form.Space + "em />"); }
                                    Session["space"] = form.Space;
                                form.Insert = "Text";
                                break;
                            case "Text":
                                if (form.BoldText && sb.ToString().Contains(searchfor))
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mspace width=.2em /> <mtext mathvariant='bold'>" + form.Text + "</mtext> <mspace width=.2em /> ");
                                    Session["text"] = form.Text;
                                }
                                else
                                {
                                    sb.Insert(sb.ToString().IndexOf(searchfor), " <mspace width=.2em /> <mtext>" + form.Text + "</mtext> <mspace width=.2em /> ");
                                    Session["text"] = form.Text;
                                }
                                form.Insert = "Identifier";
                                break;
                            case "Line Break":
                                if (sb.ToString().Contains(searchfor)) { sb.Insert(sb.ToString().IndexOf(searchfor), " <mspace linebreak='newline' /> "); }
                                break;
                            default:
                                break;
                        }
                    }

                if (form.ClearFormula) { form.ClearFormula = false; }
                if (form.BoldIdent) { form.BoldIdent = false; }
                if (form.BoldText) { form.BoldText = false; }
                if (form.BothIdent) { form.BothIdent = false; }
                if (form.BothOper) { form.BothOper = false; }
                if (form.BothNum) { form.BothNum = false; }

                Session["undo"] = Session["formula"];
                Session["formula"] = sb.ToString();

                }
                return View(form);
            }
            else
            {
                // there is something wrong with the data values
                return View(form);
            }
        }
    }
}