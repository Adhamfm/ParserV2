using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParserV2
{
    public class Parser
    {
        //private string[] tokens;
        public List<string[]> tokens = new List<string[]>();
        public static bool valid = true;
        private int currentIndex;
        public List<Node> PrintTree()
        {
            List<Node> nodes = ParseProgram();
            foreach (var node in ParseProgram())
            {
                if (node != null)
                    Console.WriteLine(node.ToString());
            }
            return nodes;
        }

        public Parser(List<string[]> tokens)
        {
            this.tokens = tokens;
            this.currentIndex = 0;
        }

        public List<Node> ParseProgram()
        {
            return ParseStmtSequence();
        }

        private List<Node> ParseStmtSequence()
        {
            List<Node> nodes = new List<Node>();
            nodes.Add(ParseStatement());

            while (Match(";"))
            {
                nodes.Add(ParseStatement());
            }
            return nodes;
        }

        private Node ParseStatement()
        {
            Node temp = null;
            if (Match("if"))
            {
                temp = ParseIfStatement();
            }
            else if (Match("repeat"))
            {
                temp = ParseRepeatStatement();
            }
            else if (IsIdentifier())
            {
                temp = ParseAssignStatement();
            }
            else if (Match("read"))
            {
                temp = ParseReadStatement();
            }
            else if (Match("write"))
            {
                temp = ParseWriteStatement();
            }
            else
            {
                //Console.WriteLine("Invalid statement");
            }

            return temp;
        }

        private Node ParseIfStatement()
        {
            Node temp = new Node("if");
            temp.left = ParseExp();
            Match("then");
            temp.middle = ParseStmtSequence();

            if (Match("else"))
            {
                temp.right = ParseStmtSequence();
            }

            Match("end");

            return temp;
        }

        private Node ParseRepeatStatement()
        {
            Node temp = new Node("repeat");
            temp.middle = ParseStmtSequence();
            Match("until");
            temp.right = new List<Node> { ParseExp() };
            return temp;
        }

        private Node ParseAssignStatement()
        {
            Node temp = new Node("assign <" + tokens[currentIndex][0] + ">");
            MatchIdentifier();
            Match(":=");
            temp.middle = new List<Node> { ParseExp() }; ;
            return temp;
        }

        private Node ParseReadStatement()
        {
            Match("read");
            Node temp = new Node("read (" + tokens[currentIndex][0] + ")");
            MatchIdentifier();
            return temp;
        }

        private Node ParseWriteStatement()
        {
            Match("write");
            Node temp = new Node("write");
            temp.middle = new List<Node> { ParseExp() };
            return temp;
        }

        private Node ParseExp()
        {
            Node temp = ParseSimpleExp();

            if (IsComparisonOp())
            {
                Node res = new Node("op(" + tokens[currentIndex][0] + ")");
                res.left = temp;
                ParseComparisonOp();
                res.right = new List<Node> { ParseSimpleExp() };
                temp = res;
            }
            return temp;
        }

        private void ParseComparisonOp()
        {
            if (Match("<") || Match("="))
            {
                // Do nothing, matched successfully
            }
            else
            {
                throw new Exception("Invalid comparison operator");
            }
        }

        private Node ParseSimpleExp()
        {
            Node temp, res;
            temp = ParseTerm();

            while (IsAddOp())
            {
                res = new Node("op(" + tokens[currentIndex][0] + ")");
                ParseAddOp();
                res.left = temp;
                res.right = new List<Node> { ParseTerm() };
                temp = res;
            }
            return temp;
        }

        private void ParseAddOp()
        {
            if (Match("+") || Match("-"))
            {
                // Do nothing, matched successfully
            }
            else
            {
                valid = false;
                throw new Exception("Invalid addition operator");
            }
        }



        private Node ParseTerm()
        {
            Node temp, res;
            temp = ParseFactor();

            while (IsMulOp())
            {
                res = new Node("op(" + tokens[currentIndex][0] + ")");
                ParseMulOp();
                res.left = temp;
                res.right = new List<Node> { ParseFactor() };
                temp = res;
            }
            return temp;
        }

        private void ParseMulOp()
        {
            if (Match("*") || Match("/"))
            {
                // Do nothing, matched successfully
            }
            else
            {
                valid = false;
                throw new Exception("Invalid multiplication operator");
            }
        }

        private Node ParseFactor()
        {
            Node temp;
            if (Match("("))
            {
                temp = ParseExp();
                Match(")");
            }
            else if (IsNumber())
            {
                temp = new Node("const(" + tokens[currentIndex][0] + ")");
                MatchNumber();
            }
            else if (IsIdentifier())
            {
                temp = new Node("id(" + tokens[currentIndex][0] + ")");
                MatchIdentifier();
            }
            else
            {
                valid = false;
                throw new Exception("Invalid factor");
            }
            return temp;
        }

        private bool Match(string expectedToken)
        {
            if (currentIndex < tokens.Count && tokens[currentIndex][0] == expectedToken)
            {
                currentIndex++;
                return true;
            }
            return false;
        }

        private bool IsIdentifier()
        {
            return currentIndex < tokens.Count && tokens[currentIndex][1] == "IDENTIFIER";
        }

        private void MatchIdentifier()
        {
            if (IsIdentifier())
            {
                currentIndex++;
            }
            else
            {
                valid = false;
                throw new Exception("Expected identifier " + tokens[currentIndex][0]);
            }
        }

        private bool IsNumber()
        {
            return currentIndex < tokens.Count && tokens[currentIndex][1] == "NUMBER";
        }

        private void MatchNumber()
        {
            if (IsNumber())
            {
                currentIndex++;
            }
            else
            {
                valid = false;
                throw new Exception("Expected number");
            }
        }

        private bool IsAddOp()
        {
            return currentIndex < tokens.Count && (tokens[currentIndex][0] == "+" || tokens[currentIndex][0] == "-");
        }

        private bool IsMulOp()
        {
            return currentIndex < tokens.Count && (tokens[currentIndex][0] == "*" || tokens[currentIndex][0] == "/");
        }

        private bool IsComparisonOp()
        {
            return currentIndex < tokens.Count && (tokens[currentIndex][0] == "<" || tokens[currentIndex][0] == "=");
        }
    }
}


