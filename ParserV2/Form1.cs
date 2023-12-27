using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParserV2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_showForm2_Click(object sender, EventArgs e)
        {
            globalX = 0;
            Parser.valid = true;
            string input = inputTextBox.Text;
            List<string[]> tokens = new List<string[]>();
            try
            {
                tokens = inputCheck(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            try
            {
                foreach (string[] pair in tokens)
                {
                    outputTextBox.Text += pair[0] + ", " + pair[1] + "\r\n";
                }

            }
            catch (Exception ex)
            {
                outputTextBox.Text = "Wrong Input";

            }

            Parser parser = new Parser(tokens);
            List<Node> nodes = null;
            try
            {
                nodes = parser.PrintTree();
                if (!Parser.valid)
                {
                    outputTextBox.Text = "Wrong Parsing asdkjfuhasdk";
                    return;
                }
                foreach (Node node in nodes)
                {
                    if (node.right != null)
                    {
                        foreach (Node right in node.right)
                        {
                            foreach (Node x in right.right)
                            {
                            }
                        }
                    }

                    //drawTest(node);
                }
                Console.WriteLine("Parsing and printing successful!");
            }
            catch (Exception ex)
            {

                Console.WriteLine("Parsing failed: " + ex.Message);
            }
            //MeDraw(nodes);
            try
            {
                drawTest(nodes);
            } catch (Exception ex)
            {
                ShowErrorDialog("Incorrect Grammar", "Input is wrong, check your input \r\n " + ex.Message);
            }
        }
        private void ShowErrorDialog(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void drawTest(List<Node> x)
        {
            int temp;
            Form newForm = new Form();

            // Set properties for the new form
            newForm.Text = "Dynamic Form";
            newForm.Size = new System.Drawing.Size(500, 500);
            //newForm.StartPosition = FormStartPosition.CenterScreen;
            PictureBox pictureBox = new PictureBox();

            pictureBox.Image = MeDraw(x);
            //pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.Controls.Add(pictureBox);
            panel.AutoScroll = true;
            panel.Padding = new Padding(10, 10, 10, 10);
            newForm.Controls.Add(panel);
            // Show the new form
            newForm.Show();

            //pictureBox1.Image = x.Draw(out temp);
        }
        private static readonly float Coef = 90 / 40f;
        private static Font font = new Font("Ariel Narrow", 14f * Coef / 2f);

        //private static Font font = new Font("Arial", 15f, FontStyle.Regular);
        int sizeWidth = 4000;
        int sizeHeight = 1000;
        int globalX = 10;
        int levelY0 = 20;
        int levelY1 = 120;
        int levelY2 = 240;
        int levelY3 = 360;
        int levelY4 = 480;
        int recWidth = 100;
        int recHeight = 50;
        // grid 100x100
        string str = "";
        public Image MeDraw(List<Node> nodes)
        {
            var result = new Bitmap(sizeWidth, sizeHeight);
            Graphics g = Graphics.FromImage(result);
            int a = 0;
            // Level 0
            if (nodes == null) return null;
            foreach (Node node in nodes)
            {
                a++;
                int _child = 0;
                _child = node.CountChildren() - 1;
                var rcl = new Rectangle(globalX, levelY0, recWidth, recHeight);
                g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl);
                node.xleftCorner = globalX;
                node.yleftCorner = levelY0;
                str = node.value;
                g.DrawString(str, font, Brushes.Black, globalX + 5, levelY0 + 5);

                int b = 0;
                // Level 1 Left
                if (node.left != null)
                {
                    b++;
                    int localX = 0;
                    if (globalX > 50)
                        localX = globalX - 100;
                    else
                        localX = 10;

                    var rcl2 = new Rectangle(localX, levelY1, recWidth, recHeight);
                    g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl2);
                    node.left.xleftCorner = localX;
                    node.left.yleftCorner = levelY1;
                    str = node.left.value;
                    g.DrawString(str, font, Brushes.Black, localX + 5, levelY1 + 5);
                    // Level 2 left
                    if (node.left.left != null)
                        recDraw(g, node.left.left);
                    if (node.left.middle != null)
                        recDrawMiddle(g, node.left.middle);
                    if (node.left.right != null)
                        recDrawRight(g, node.left.right);

                    globalX += 100;
                }
                // Level 1 Middle
                if (node.middle != null)
                {
                    foreach (Node midNode1 in node.middle)
                    {
                        int localX = 0;
                        int guiX = 0;
                        if (globalX > 50)
                            localX = globalX;
                        else
                            localX = 10;
                        var rcl3 = new Rectangle(localX, levelY1, recWidth, recHeight);
                        g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl3);
                        midNode1.xleftCorner = localX;
                        midNode1.yleftCorner = levelY1;
                        str = midNode1.value;
                        g.DrawString(str, font, Brushes.Black, localX + 5, levelY1 + 5);
                        // Level 2 Middle
                        recDraw(g, midNode1.left);
                        recDrawMiddle(g, midNode1.middle);
                        recDrawRight(g, midNode1.right);
                        globalX += 150;
                    }
                }

                // Level 1 Right
                if (node.right != null)
                {
                    foreach (Node rightNode1 in node.right)
                    {
                        int localX = 0;
                        if (globalX > 50)
                            localX = globalX + 50;
                        else
                            localX = 10;
                        var rcl4 = new Rectangle(localX, levelY1, recWidth, recHeight);
                        g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl4);
                        str = rightNode1.value;
                        rightNode1.xleftCorner = localX;
                        rightNode1.yleftCorner = levelY1;
                        g.DrawString(str, font, Brushes.Black, localX + 5, levelY1 + 5);
                        // Level 2 Right
                        try
                        {

                            recDraw(g, rightNode1.left);
                            recDrawMiddle(g, rightNode1.middle);
                            recDrawRight(g, rightNode1.right);
                            globalX += 150;
                        } catch (Exception ex)
                        {
                            ShowErrorDialog("Incorrect Grammar", ex.Message);
                        }
                    }
                }
                globalX += 200;
            }

            DrawLines(g, nodes);
            return result;
        }


        public void DrawLines(Graphics g, List<Node> nodes)
        {
            if (nodes == null) return;
            Node currentNode = null;
            Node neighborNode = null;
            // Draw lines between parent and child

            foreach (Node node in nodes)
            {
                DrawChildrenLines(g, node);
            }
            // Same level
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                currentNode = nodes[i];
                neighborNode = nodes[i + 1];
                Point startPoint = new Point(currentNode.xleftCorner + recWidth, currentNode.yleftCorner + recHeight / 2);
                Point endPoint = new Point(neighborNode.xleftCorner, neighborNode.yleftCorner + recHeight / 2);
                g.DrawLine(new Pen(Color.Black, 2f), startPoint, endPoint);
            }
        }
        public void DrawChildrenLines(Graphics g, Node node)
        {
            if (node == null) return;
            Node currentNode = null;
            Node parentNode = null;
            // Draw lines between parent and child
            // in our example: read, if
            if (node.left != null)
            {
                // op(<)
                parentNode = node;
                currentNode = node.left;
                Point startPoint = new Point(parentNode.xleftCorner + recWidth / 2, parentNode.yleftCorner + recHeight);
                Point endPoint = new Point(currentNode.xleftCorner + recWidth / 2, currentNode.yleftCorner);
                g.DrawLine(new Pen(Color.Black, 2f), startPoint, endPoint);
                DrawChildrenLines(g, currentNode);
            }
            if (node.middle != null)
            {
                parentNode = node;
                currentNode = node.middle[0];
                Point startPoint = new Point(parentNode.xleftCorner + recWidth / 2, parentNode.yleftCorner + recHeight);
                Point endPoint = new Point(currentNode.xleftCorner + recWidth / 2, currentNode.yleftCorner);
                g.DrawLine(new Pen(Color.Black, 2f), startPoint, endPoint);
                DrawLines(g, node.middle);
                DrawChildrenLines(g, node.middle[0]);
            }
            if (node.right != null)
            {
                parentNode = node;
                currentNode = node.right[0];
                Point startPoint = new Point(parentNode.xleftCorner + recWidth / 2, parentNode.yleftCorner + recHeight);
                Point endPoint = new Point(currentNode.xleftCorner + recWidth / 2, currentNode.yleftCorner);
                g.DrawLine(new Pen(Color.Black, 2f), startPoint, endPoint);
                DrawLines(g, node.right);
                DrawChildrenLines(g, node.right[0]);
            }
        }

        public void recDraw(Graphics g, Node node)
        {
            if (node == null) return;
            int localX2 = 0;
            if (globalX > 50)
                localX2 = globalX - 150;
            else
                localX2 = 10;
            var rcl2l = new Rectangle(localX2, levelY2, recWidth, recHeight);
            g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl2l);
            node.xleftCorner = localX2;
            node.yleftCorner = levelY2;
            str = node.value;
            g.DrawString(str, font, Brushes.Black, localX2 + 5, levelY2 + 5);
            if (node.left != null)
                recDraw2(g, node.left);
            if (node.middle != null)
                recDraw2(g, node.middle);

            if (node.right != null)
                recDraw2(g, node.right);

        }
        public void recDrawRight(Graphics g, List<Node> nodes)
        {
            if (nodes == null) return;
            foreach (Node node in nodes)
            {
                int localX2 = globalX;
                if (globalX > 50)
                    localX2 = globalX;
                else
                    localX2 = 10;
                var rcl3l = new Rectangle(localX2, levelY2, recWidth, recHeight);
                g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl3l);
                node.xleftCorner = localX2;
                node.yleftCorner = levelY2;
                str = node.value;
                g.DrawString(str, font, Brushes.Black, localX2 + 5, levelY2 + 5);
                recDraw2(g, node.left);
                recDraw2(g, node.middle);
                recDraw2(g, node.right);
                globalX += 200;
            }
        }
        public void recDrawMiddle(Graphics g, List<Node> nodes)
        {
            if (nodes == null) return;
            foreach (Node node in nodes)
            {
                if (node == null)
                {
                    //ShowErrorDialog("An error occurred", "sad");
                    //return;
                }
                int localX2 = globalX;
                if (globalX > 50)
                    localX2 = globalX;
                else
                    localX2 = 10;
                var rcl3l = new Rectangle(localX2, levelY2, recWidth, recHeight);
                g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl3l);
                node.xleftCorner = localX2;
                node.yleftCorner = levelY2;
                str = node.value;
                g.DrawString(str, font, Brushes.Black, localX2 + 5, levelY2 + 5);
                recDraw2(g, node.left);
                recDraw2(g, node.middle);
                recDraw2(g, node.right);
                globalX += 200;
            }
        }
        public void recDraw2(Graphics g, Node node)
        {
            if (node == null) return;
            int localX2 = 0;
            if (globalX > 50)
                localX2 = globalX - 150;
            else
                localX2 = 10;

            var rcl2l = new Rectangle(localX2, levelY3, recWidth, recHeight);
            g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl2l);
            node.xleftCorner = localX2;
            node.yleftCorner = levelY3;
            str = node.value;
            g.DrawString(str, font, Brushes.Black, localX2 + 5, levelY3 + 5);
            if (node.left != null)
                recDraw3(g, node.left);
            if (node.middle != null)
                recDraw3(g, node.middle);
            if (node.right != null)
                recDraw3(g, node.right);
        }
        public void recDraw2(Graphics g, List<Node> nodes)
        {
            if (nodes == null) return;
            foreach (Node node in nodes)
            {
                int localX2 = globalX;
                if (globalX > 50)
                    localX2 = globalX;
                else
                    localX2 = 10;
                var rcl3l = new Rectangle(localX2, levelY3, recWidth, recHeight);
                g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl3l);
                node.xleftCorner = localX2;
                node.yleftCorner = levelY3;
                str = node.value;
                g.DrawString(str, font, Brushes.Black, localX2 + 5, levelY3 + 5);
                recDraw3(g, node.left);
                recDraw3(g, node.middle);
                recDraw3(g, node.right);
                globalX += 200;
            }
        }
        public void recDraw3(Graphics g, Node node)
        {
            if (node == null) return;
            int localX2 = 0;
            if (globalX > 50)
                localX2 = globalX - 150;
            else
                localX2 = 10;

            var rcl2l = new Rectangle(localX2, levelY4, recWidth, recHeight);
            g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl2l);
            node.xleftCorner = localX2;
            node.yleftCorner = levelY4;
            str = node.value;
            g.DrawString(str, font, Brushes.Black, localX2 + 5, levelY4 + 5);

        }
        public void recDraw3(Graphics g, List<Node> nodes)
        {
            if (nodes == null) return;
            foreach (Node node in nodes)
            {
                int localX2 = globalX;
                if (globalX > 50)
                    localX2 = globalX;
                else
                    localX2 = 10;
                var rcl3l = new Rectangle(localX2, levelY4, recWidth, recHeight);
                g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl3l);
                node.xleftCorner = localX2;
                node.yleftCorner = levelY4;
                str = node.value;
                g.DrawString(str, font, Brushes.Black, localX2 + 5, levelY4 + 5);

                globalX += 200;
            }
        }
        public List<string[]> inputCheck(string input)
        {
            List<string[]> tokens = new List<string[]>();
            outputTextBox.Text = "";
            input = input.Replace("\r", "");
            input = input.Replace(" ", "");
            // TODO remove last enter if exists
            if (input[input.Length - 1] == '\n') input = input.Remove(input.Length - 1);
            // Split the input string into individual tokens
            string[] tokenPairs = input.Split('\n');
            // Process each token pair and add to the list
            foreach (string pair in tokenPairs)
            {
                //Console.WriteLine(pair);
                string[] token = pair.Split(',');
                //Console.WriteLine(token[1]);
                tokens.Add(new string[] { token[0], token[1] });
            }

            return tokens;
        }

        private void inputTxtBtn_Click(object sender, EventArgs e)
        {
            // Create OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the file dialog properties
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.Title = "Select a Text File";

            // Show the file dialog
            DialogResult result = openFileDialog.ShowDialog();

            // Check if the user clicked OK
            if (result == DialogResult.OK)
            {
                // Get the selected file path
                string filePath = openFileDialog.FileName;

                try
                {
                    // Read the content of the text file
                    string fileContent = File.ReadAllText(filePath);
                    inputTextBox.Text = fileContent;
                    // Display the content (optional)
                    //MessageBox.Show("File Content:\n\n" + fileContent, "File Content");

                    // You can use the 'fileContent' variable to work with the content of the file
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading the file: " + ex.Message, "Error");
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = inputTextBox.Text;
            // Function scanner to string
            Scanner scanner = new Scanner();
            scanner.Scan(input);
            string tokens = scanner.Output();

            outputTextBox.Text = input;
            inputTextBox.Text = tokens;
        }
    }
}
