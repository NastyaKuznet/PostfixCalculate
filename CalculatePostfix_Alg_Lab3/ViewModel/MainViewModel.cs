using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Alg_Lab3;

namespace CalculatePostfix_Alg_Lab3.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private string infixExpression = "";
        private string postfixExpression = "";

        public string InfixExpression
        {
            get { return infixExpression; }
            set
            {
                infixExpression = value;

                OnPropertyChanged();
            }
        }

        public string PostfixExpression
        {
            get { return postfixExpression; }
            set
            {
                postfixExpression = value;

                OnPropertyChanged();
            }
        }

        public ICommand Input => new CommandDelegate((obj) => {
            WriteInTextBlock(obj);
        });

        public ICommand Result => new CommandDelegate((obj) => {
            try{
                if (InfixExpression.Length == 0) return;
                PostfixCalculator calc = new PostfixCalculator(InfixExpression);
                calc.Calculate(false);
                InfixExpression = calc.GetResult() + " ";
                PostfixExpression = calc.GetPostfixStr();
            }
            catch (Exception e){
                InfixExpression = "Неверное выражение";
            }
        });
        public ICommand Reset => new CommandDelegate((obj) => {
            PostfixExpression = "";
            if(InfixExpression.Length != 0)
                InfixExpression = InfixExpression.Substring(0, InfixExpression.Length - 1); 
        });

        public ICommand Clear => new CommandDelegate((obj) => {
            InfixExpression = "";
            PostfixExpression = "";
        });

        private void WriteInTextBlock(object i)
        {
            if (InfixExpression == null && !i.ToString().Equals(','))
                InfixExpression = CorrectFormat(i.ToString());
            else
            {
                InfixExpression += CorrectFormat(i.ToString());
            }
        }

        private string CorrectFormat(string i)
        {
            string result = "";
            if (InfixExpression.Length == 0 && PostfixCalculator.Operations.ContainsKey(i) && !i.Equals("-") && !i.Equals("(") && !i.Equals(")") 
                && !i.Equals("sin") && !i.Equals("cos") && !i.Equals("ln") && !i.Equals("sqrt")) return result;
            if(i.Equals("-") && InfixExpression.Length > 4)
            {
                if (InfixExpression[InfixExpression.Length - 2].Equals("+"))
                {
                    InfixExpression = InfixExpression.Substring(0, InfixExpression.Length - 2);
                    result = "- ";
                }
                else if (InfixExpression[InfixExpression.Length - 2].Equals("-"))
                {
                    InfixExpression = InfixExpression.Substring(0, InfixExpression.Length - 2);
                    result = "+ ";
                }
                else if (PostfixCalculator.Operations.ContainsKey(InfixExpression[InfixExpression.Length - 2].ToString()))
                {
                    result = "-";
                }
            }
            else if (i.Equals("-") && InfixExpression.Length < 4)
            {
                result = "-";
            }
            else if (PostfixCalculator.Operations.ContainsKey(i) || !i.Equals("(") || !i.Equals(")"))
                result = " " + i + " ";
            else
                result = i;
            return result;
        }
    }
}
