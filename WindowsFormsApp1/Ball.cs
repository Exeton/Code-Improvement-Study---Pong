using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Ball
    {
        private int x;
        private int y;
        private Point velocity;
        public bool justScored;
        Rectangle screen;
        Bitmap ballBitmap;
        public Ball(int X, int Y, Point Velocity, Bitmap BallBitmap, Rectangle Screen)
        {
            x = X;
            y = Y;
            velocity = Velocity;
            ballBitmap = BallBitmap;
            justScored = false;
            screen = Screen;
        }

        public void update(Paddle[] paddles)//No ray tracing
        {
            Point expectedDestination = new Point(x + velocity.X, y + velocity.Y);
            Rectangle expectedLocationRectangle = new Rectangle(expectedDestination.X, expectedDestination.Y, ballBitmap.Width, ballBitmap.Height);
            detectPaddleCollisions(expectedLocationRectangle, paddles);
            detectWallCollisions(expectedLocationRectangle, paddles);

            x += velocity.X;
            y += velocity.Y;
        }
        public void draw(Graphics g)
        {
            g.DrawImage(ballBitmap, new Point(x, y));
        }

        private void detectPaddleCollisions(Rectangle expectedBallLocation, Paddle[] paddles)
        {
            foreach (Paddle paddle in paddles)
            {
                if (paddle.paddleRect.IntersectsWith(expectedBallLocation))
                {
                    velocity.X = -velocity.X;
                    velocity.Y += new Random().Next(-2, 3);
                    break;
                }
            }
        } 
        private void detectWallCollisions(Rectangle expectedBallLocation, Paddle[] paddles)
        {
            if (expectedBallLocation.Bottom > screen.Bottom || expectedBallLocation.Top < screen.Top)
            {
                velocity.Y = -velocity.Y;
                velocity.X += new Random().Next(-1, 2);
            }

            if (expectedBallLocation.Left < 0)
            {
                justScored = true;
                paddles[1].Score();
            }
            if (expectedBallLocation.Right > screen.Right)
            {
                justScored = true;
                paddles[0].Score();
            }
        }
    }
}
