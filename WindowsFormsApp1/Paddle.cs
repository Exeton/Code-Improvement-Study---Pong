using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Paddle
    {
        private enum keys
        {
            Up = 0,
            Down = 1,
        }
        private static int yScoreOffset = 10;//Make final?
        private int fontSize = 24;
        private bool xOffsetIsRightBoundOfScore;
        Keys[] keyCodes;
        bool[] pressedKeys;
        int xPos;
        int yPos;
        int score;//
        int maxPaddleHeight;
        Bitmap paddlePicture;

        public Rectangle paddleRect
        {
            get
            {
                return new Rectangle(xPos, yPos, paddlePicture.Width, paddlePicture.Height);
            }
        }
        public Paddle(int XPos, int YPos, int MaxPaddleHeight, Keys[] KeyCodes, Bitmap PaddlePicture, bool XOffsetIsRightBoundOfScore)
        {
            xOffsetIsRightBoundOfScore = XOffsetIsRightBoundOfScore;
            keyCodes = KeyCodes;
            xPos = XPos;
            yPos = YPos;
            maxPaddleHeight = MaxPaddleHeight;
            paddlePicture = PaddlePicture;

            pressedKeys = new bool[2];
        }
        public void Score()
        {
            score++;
        }
        private void moveUp()
        {
            if (yPos - Form1.MovementPixels > 0)
            {
                yPos -= Form1.MovementPixels;
            }
        }
        private void moveDown()
        {
            if (yPos + Form1.MovementPixels < maxPaddleHeight - paddlePicture.Size.Height)
            {
                yPos += Form1.MovementPixels;
            }
        }
        private void tryMove()
        {
            if (pressedKeys[0])
                moveUp();
            if (pressedKeys[1])
                moveDown();
        }

        private bool checkCollision(Rectangle Ball)
        {
            Rectangle Paddle = new Rectangle(new Point(xPos, yPos), paddlePicture.Size);
            if (Ball.IntersectsWith(Paddle))
                return true;
            return false;
        }

        public void doTick()
        {
            tryMove();
        }
        public void Draw(Graphics g)
        {
            g.DrawImage(paddlePicture, paddleRect);
            DrawScore(g, score);
        }
        private void DrawScore(Graphics g, int score)
        {
            string Score = score.ToString();
            Font font = new Font(FontFamily.GenericSerif, 35, FontStyle.Regular);
            int offset = 0;
            if (xOffsetIsRightBoundOfScore)
            {
                offset = (int)g.MeasureString(Score, font).Width;
            }
            g.DrawString(Score, font, Brushes.DarkGoldenrod, new Point(xPos - offset, yScoreOffset));
        }

        public void KeyUp(Keys keyCode)
        {
            for (int i = 0; i < keyCodes.Length; i++) 
            {
                if (keyCode == keyCodes[i])
                {
                    pressedKeys[i] = false;
                }
            }
        }
        public void KeyDown(Keys keyCode)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (keyCode == keyCodes[i])
                {
                    pressedKeys[i] = true;
                }
            }
        }
    }
}
