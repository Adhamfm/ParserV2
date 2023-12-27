using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ParserV2
{
    public class Node
    {
        public string value;
        public List<Node> right;
        public Node left;
        public List<Node> middle;
        public int xleftCorner;
        public int yleftCorner;


        //constructor for terminal nodes(right and left and middle are all = null)
        public Node(string value)
        {
            this.value = value;
            this.right = null;
            this.left = null;
            this.middle = null;
        }

        //constructor for terminal nodes(right and left and = null and middle is not null)
        public Node(string value, List<Node> middle)
        {
            this.value = value;
            this.right = null;
            this.left = null;
            this.middle = middle;
        }

        //constructor for terminal nodes(the middle is null, but left and right are not)
        public Node(string value, Node left, List<Node> right)
        {
            this.value = value;
            this.right = right;
            this.left = left;
            this.middle = null;
        }

        //constructor for termainal nodes where all nodes is not null
        public Node(string value, Node left, List<Node> right, List<Node> middle)
        {
            this.value = value;
            this.right = right;
            this.left = left;
            this.middle = middle;
        }
        public override string ToString()
        {
            return ToString(0);
        }


        public int CountChildren()
        {
            return 5;
        }

        private string ToString(int depth)
        {
            string indent = new string(' ', depth * 2);
            string result = $"{indent}{value}\n";

            if (left != null)
            {
                result += left.ToString(depth + 1);
            }

            if (middle != null)
            {
                foreach (var node in middle)
                {
                    result += node.ToString(depth + 1);
                }
            }

            if (right != null)
            {
                foreach (var node in right)
                {
                    result += node.ToString(depth + 1);
                }
            }

            return result;
        }



        private static Bitmap _nodeBg = new Bitmap(100, 60);

        private static Size _freeSpace = new Size((_nodeBg.Width / 2) / 8, (int)(_nodeBg.Height * 1.3f));


        private static readonly float Coef = _nodeBg.Width / 40f;

        //static Node()
        //{
        //    var g = Graphics.FromImage(_nodeBg);                                    // get a Graphics from _nodeBg bitmap, 
        //    g.SmoothingMode = SmoothingMode.HighQuality;                            // set the smoothing mode
        //    //var rcl = new Rectangle(1, 1, _nodeBg.Width - 2, _nodeBg.Height - 2);   // get a rectangle of drawer
        //    //g.FillRectangle(Brushes.White, rcl);
        //    //g.DrawRectangle(new Pen(Color.Black, 1.2f), rcl);                          // draw ellipse, you could also comment this line, and uncomment the above line as another option for background image
        //    //g.FillEllipse(new LinearGradientBrush(new Point(0, 0), new Point(_me.Width, _me.Height), Color.Goldenrod, Color.Black), rcl);

        //}


        private bool _isChanded = true;

        Image _lastImage;
        /// <summary>
        /// the location of the node on the top of the _lastImage.
        /// </summary>
        private int _lastImageLocationOfStarterNode;
        //private static Font font = new Font("Tahoma", 14f * Coef/2f);
        /// <summary>
        /// paints the node and it's childs
        /// </summary>
        /// <param name="center">the location of the node on the top of the drawed image.</param>
        /// <returns>the image representing the current node and it's childs</returns>
        /// 
        static public int extra = 0;
        public Image Draw(out int center)
        {
            var gd = Graphics.FromImage(_nodeBg);
            var rcl = new Rectangle(1, 1, _nodeBg.Width - 2, _nodeBg.Height - 2);   // get a rectangle of drawer

            center = _lastImageLocationOfStarterNode;
            //if (!IsChanged) // if the current node and it's childs are up to date, just return the last drawed image.
            //    return _lastImage;
            var lCenter = 0 + extra;
            var rCenter = 0 + extra;
            var mCenter = 0 + extra;
            //Console.WriteLine("this is extra "+ extra, lCenter);

            Image lNodeImg = null, rNodeImg = null, mNodeImg = null;
            if (left != null)       // draw left node's image
                lNodeImg = left.Draw(out lCenter);
            if (right != null)      // draw right node's image
            {
                foreach (var node in right)
                { rNodeImg = node.Draw(out rCenter); }
            }
            //rNodeImg = Riddle.Draw(out rCenter);
            if (middle != null)      // draw middle node's image
            {
                foreach (var node in middle)
                {
                    mNodeImg = node.Draw(out mCenter);
                }
            }
            //mNodeImg = Middle.Draw(out mCenter);

            // draw current node and it's childs (left node image and right node image)
            var lSize = new Size();
            var rSize = new Size();
            var mSize = new Size();
            var under = (lNodeImg != null) || (rNodeImg != null) || (mNodeImg != null);// if true the current node has childs
            if (lNodeImg != null)
                lSize = lNodeImg.Size;
            if (rNodeImg != null)
                rSize = rNodeImg.Size;
            if (mNodeImg != null)
                mSize = mNodeImg.Size;

            var maxHeight = lSize.Height;
            if (maxHeight < rSize.Height)
                maxHeight = rSize.Height;
            if (maxHeight < mSize.Height)
                maxHeight = mSize.Height;

            if (lSize.Width <= 0)
                lSize.Width = (_nodeBg.Width - _freeSpace.Width) / 2;
            if (rSize.Width <= 0)
                rSize.Width = (_nodeBg.Width - _freeSpace.Width) / 2;
            if (mSize.Width <= 0)
                mSize.Width = (_nodeBg.Width - _freeSpace.Width) / 2;

            var resSize = new Size
            {
                Width = lSize.Width + rSize.Width + mSize.Width + _freeSpace.Width * 2,
                Height = _nodeBg.Size.Height + (under ? maxHeight + _freeSpace.Height : 0)
            };

            var result = new Bitmap(resSize.Width, resSize.Height);
            var g = Graphics.FromImage(result);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), resSize));
            g.DrawImage(_nodeBg, lSize.Width - _nodeBg.Width / 2 + _freeSpace.Width / 2, 0);
            var str = value.ToString();
            //g.DrawString(str, font, Brushes.Black, lSize.Width - _nodeBg.Width / 2 + _freeSpace.Width / 2 + (2 + (str.Length == 1 ? 10 : str.Length == 2 ? 5 : 0)) * Coef, _nodeBg.Height / 2f -10 * Coef);


            center = lSize.Width + _freeSpace.Width / 2;
            var pen = new Pen(Brushes.Black, 1.2f * Coef)
            {
                EndCap = LineCap.ArrowAnchor,
                StartCap = LineCap.Round
            };
            extra += 100;

            float x1 = center;
            float y1 = _nodeBg.Height;
            float y2 = _nodeBg.Height + _freeSpace.Height;
            float x2 = lCenter;
            float y3 = _nodeBg.Height + _freeSpace.Height;
            float x3 = 400;
            var h = Math.Abs(y2 - y1);
            var w = Math.Abs(x2 - x1);
            if (lNodeImg != null)
            {
                g.DrawImage(lNodeImg, 0, _nodeBg.Size.Height + _freeSpace.Height);
                var points1 = new List<PointF>
                                  {
                                      new PointF(x1, y1),
                                      new PointF(x1 - w/6, y1 + h/3.5f),
                                      new PointF(x2 + w/6, y2 - h/3.5f),
                                      new PointF(x2, y2),
                                  };
                g.DrawCurve(pen, points1.ToArray(), 0.5f);
            }
            if (rNodeImg != null)
            {
                if (mNodeImg == null)
                {
                    g.DrawImage(rNodeImg, lSize.Width + _freeSpace.Width, _nodeBg.Size.Height + _freeSpace.Height);
                    x3 = rCenter + lSize.Width + _freeSpace.Width;
                    w = Math.Abs(x2 - x1);
                }
                else
                {
                    g.DrawImage(rNodeImg, 2 * lSize.Width + _freeSpace.Width, _nodeBg.Size.Height + _freeSpace.Height);
                    x3 = rCenter + 2 * lSize.Width + _freeSpace.Width;
                    w = 2 * Math.Abs(x2 - x1);
                }
                var points = new List<PointF>
                                 {
                                     new PointF(x1, y1),
                                     new PointF(x1 + w/6, y1 + h/3.5f),
                                     new PointF(x3 - w/6, y2 - h/3.5f),
                                     new PointF(x3, y2)
                                 };
                g.DrawCurve(pen, points.ToArray(), 0.5f);
            }
            if (mNodeImg != null)
            {
                g.DrawImage(mNodeImg, lSize.Width + _freeSpace.Width, _nodeBg.Size.Height + _freeSpace.Height);
                x2 = mCenter + lSize.Width + _freeSpace.Width;
                w = Math.Abs(x2 - x1);
                var points = new List<PointF>
                                 {
                                     new PointF(x1, y1),
                                     new PointF(x1 + w/6, y1 + h/3.5f),
                                     new PointF(x2 - w/6, y2 - h/3.5f),
                                     new PointF(x2, y2)
                                 };
                g.DrawCurve(pen, points.ToArray(), 0.5f);
            }
            //IsChanged = false;
            _lastImage = result;
            _lastImageLocationOfStarterNode = center;

            return result;
        }


    }
}