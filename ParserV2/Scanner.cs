using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParserV2
{
    partial class Scanner
    {
        private Dictionary<string, bool> STATES = new Dictionary<string, bool>
    {
        { "START", true },
        { "IN_COMMENT", false },
        { "IN_IDENTIFIER", false },
        { "IN_NUMBER", false },
        { "IN_ASSIGNMENT", false },
        { "DONE", false },
        { "OTHER", false }
    };

        public List<string[]> tokens = new List<string[]>();
        private bool stateOther = false;

        private List<string> KEYWORDS = new List<string> { "else", "end", "if", "repeat", "then", "until", "read", "write" };

        Dictionary<string, string> OPERATORS = new Dictionary<string, string>
        {
            { ";", "SEMICOLON" },
            { ":=", "ASSIGN" },
            { "<", "LESSTHAN" },
            { "=", "EQUAL" },
            { "+", "PLUS" },
            { "-", "MINUS" },
            { "*", "MULT" },
            { "/", "DIV" },
            { "(", "OPENBRACKET" },
            { ")", "CLOSEDBRACKET" }
        };

        public void Scan(string inputText)
        {

            string token = "";

            foreach (char c in inputText)
            {
                if (GetState("START"))
                {
                    if (IsSymbol(c.ToString()))
                    {
                        SetState("DONE");
                    }
                    else if (c == ' ')
                    {
                        SetState("START");
                        continue;
                    }
                    else if (c == '{')
                    {
                        SetState("IN_COMMENT");
                    }
                    else if (IsNum(c.ToString()))
                    {
                        SetState("IN_NUMBER");
                    }
                    else if (IsStr(c.ToString()))
                    {
                        SetState("IN_IDENTIFIER");
                    }
                    else if (IsCol(c))
                    {
                        SetState("IN_ASSIGNMENT");
                    }
                }
                else if (GetState("IN_COMMENT"))
                {
                    if (c == '}')
                    {
                        SetState("DONE");
                    }
                    else
                    {
                        SetState("IN_COMMENT");
                    }
                }
                else if (GetState("IN_NUMBER"))
                {
                    if (IsNum(c.ToString()))
                    {
                        SetState("IN_NUMBER");
                    }
                    else if (c == ' ')
                    {
                        SetState("DONE");
                    }
                    else
                    {
                        SetState("OTHER");
                    }
                }
                else if (GetState("IN_IDENTIFIER"))
                {
                    if (IsStr(c.ToString()))
                    {
                        SetState("IN_IDENTIFIER");
                    }
                    else if (c == ' ')
                    {
                        SetState("DONE");
                    }
                    else
                    {
                        SetState("OTHER");
                    }
                }
                else if (GetState("IN_ASSIGNMENT"))
                {
                    if (c == '=')
                    {
                        SetState("DONE");
                    }
                    else
                    {
                        SetState("OTHER");
                    }
                }

                if (!GetState("OTHER"))
                {
                    token += c;

                }

                if (GetState("OTHER"))
                {
                    SetState("DONE");
                    stateOther = true;
                }

                if (GetState("DONE"))
                {
                    Classify(token);
                    if (stateOther)
                    {
                        token = c.ToString();
                        if (IsCol(c)) SetState("IN_ASSIGNMENT");
                        if (IsComment(c.ToString())) SetState("IN_COMMENT");
                        if (IsNum(c.ToString())) SetState("IN_NUMBER");
                        if (IsStr(c.ToString())) SetState("IN_IDENTIFIER");
                        if (IsSymbol(c.ToString()))
                        {
                            Classify(c.ToString());
                            token = "";
                            SetState("START");
                        }
                        stateOther = false;
                    }
                    else
                    {
                        token = "";
                    }
                    SetState("START");
                }
            }
        }

        private void SetState(string state)
        {
            foreach (string key in STATES.Keys)
            {
                STATES[key] = false;
            }
            STATES[state] = true;
        }

        private bool GetState(string state)
        {
            return STATES[state];
        }

        private void Classify(string token)
        {
            if (token.EndsWith(" ")) token = token.Substring(0, token.Length - 1);
            if (IsStr(token))
            {
                if (KEYWORDS.Contains(token))
                {
                    tokens.Add(new[] { token, token.ToUpper() });
                }
                else
                {
                    tokens.Add(new[] { token, "IDENTIFIER" });
                }
            }
            else if (IsNum(token))
            {
                tokens.Add(new[] { token, "NUMBER" });
            }
            else if (OPERATORS.TryGetValue(token, out string? value))
            {
                tokens.Add(new[] { token, value });
            }
            else if (IsComment(token))
            {
                tokens.Add(new[] { token, "COMMENT" });
            }
        }



        private bool IsStr(string token)
        {
            return token.All(char.IsLetter);
        }

        private bool IsNum(string token)
        {
            return token.All(char.IsDigit);
        }

        private bool IsCol(char c)
        {
            return c == ':';
        }

        private bool IsSymbol(string token)
        {
            List<string> symbol = new List<string> { "+", "-", "*", "/", "=", "<", ">", "(", ")", ";" };
            return symbol.Contains(token);
        }

        private bool IsComment(string token)
        {
            // return MyRegex().IsMatch(token);
            return true;
        }

        private string ReadFile(string fileName)
        {
            using StreamReader reader = new StreamReader(fileName);

            string inputText = reader.ReadToEnd();
            inputText = inputText.Replace(Environment.NewLine, " ");
            inputText += " ";
            return inputText;
        }

        public string Output()
        {
            string output = "";
            //using StreamWriter writer = new StreamWriter("output.txt");
            //writer.WriteLine("{0,-12}  {1,12}", "Type", "Token");
            //writer.WriteLine("{0,-12}  {1,12}", "=====", "=====");
            foreach (string[] token in tokens)
            {
                //writer.WriteLine("{0,-12}  {1,12}", token[1], token[0]);
                output += token[0] + "," + token[1] + "\r\n";
            }
            return output;
        }
    }

}

