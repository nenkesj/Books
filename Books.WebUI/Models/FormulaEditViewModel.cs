using System.Collections.Generic;
using Books.Domain.Concrete;
using Books.WebUI.Infrastructure;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Books.WebUI.Models
{
    public class FormulaEditViewModel
    {
        public int NodeID { get; set; }
        public bool ClearFormula { get; set; }
        public bool BothIdent { get; set; }
        public bool BoldIdent { get; set; }
        public string Ident1 { get; set; }
        public string Ident2 { get; set; }
        public string Oper1 { get; set; }
        public string Oper2 { get; set; }
        public bool BothOper { get; set; }
        public string Num1 { get; set; }
        public string Num2 { get; set; }
        public bool BothNum { get; set; }
        public string Space { get; set; }
        public string Text { get; set; }
        public bool BoldText { get; set; }
        public string Insert { get; set; }
        public string Target { get; set; }
        public string Block { get; set; }
        public string Algebraic { get; set; }
        public string Calculus { get; set; }
        public string Ellipses { get; set; }
        public string Logic { get; set; }
        public string Vector { get; set; }
        public string GreekUpper { get; set; }
        public string GreekLower { get; set; }
    }
}