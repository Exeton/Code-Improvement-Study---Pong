# Pong
## Description
This study is meant to show you my thought process when rewriting code. It will showcase rewriting part of a pong game I wrote in C#. The solution file in this repo is the original game, so you can download it and follow along. Please note, this lesson doesn't address some of the issues with the program such as the outdated drawing method. Also, please note WinForms is outdated.

## How is this project implemented
My goal in creating this project was to use some of the new OOP principles I'd learned to rewrite pong. That being said, when implementing the project, I created a Ball and Paddle class which store most of the program's state (or data) and also tried using the [game loop pattern](http://gameprogrammingpatterns.com/game-loop.html). Although I didn't follow the pattern properly, my game loop was composed of a fixed interval timer running drawing and physics code.

## Looking at Form1

```C#
public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private const int speedMultiplier = 6;
        public const int MovementPixels = 10;
        private const int paddleOffset = 20;
        private const int paddleWidth = 10;
        private const int paddleHeight = 85;

        private int height;
        private int width;

        private Paddle[] paddles;
        private Ball ball;
        private Timer t;

        private Bitmap ballBitmap;

        private void Form1_Load(object sender, EventArgs e)
        {
		
            DoubleBuffered = true;

            height = Height + this.Top - RectangleToScreen(ClientRectangle).Top;
            width = Width + this.Left - RectangleToScreen(ClientRectangle).Left;

            Bitmap paddlePicture = new Bitmap(paddleWidth, paddleHeight);
            Graphics bitmapGraphs = Graphics.FromImage(paddlePicture);
            bitmapGraphs.FillRectangle(Brushes.DarkOrange, 0, 0, paddleWidth, paddleHeight);

            ballBitmap = new Bitmap(8, 8);
            Graphics ballBitmapGraphics = Graphics.FromImage(ballBitmap);
            ballBitmapGraphics.FillRectangle(Brushes.Red, 0, 0, 8, 8);
            //bitmapGraphs.Dispose();

            int halfFormHeight = height / 2;

            Keys[] PaddleOneControls = { Keys.W, Keys.S };
            Keys[] PaddleTwoControls = { Keys.Up, Keys.Down };

            paddles = new Paddle[2];
            Paddle LeftPadle = new Paddle(paddleOffset, halfFormHeight, height, PaddleOneControls, paddlePicture, false);
            Paddle RightPadle = new Paddle(width - paddleOffset - paddlePicture.Width, halfFormHeight, height, PaddleTwoControls, paddlePicture, true);
            paddles[0] = LeftPadle;
            paddles[1] = RightPadle;

            ball = newBall();
            
            t = new Timer();
            t.Interval = 25;
            t.Tick += new EventHandler(gameLoop);
            t.Start();
        }
        private void gameLoop(Object myObject, EventArgs args)
        {
            BufferedGraphicsContext myContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer;
            myBuffer = myContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            Graphics BufferGraphics = myBuffer.Graphics;

            BufferGraphics.Clear(Color.LightPink);
            foreach (Paddle paddle in paddles)
            {
                paddle.doTick();
                paddle.Draw(BufferGraphics);
            }
            ball.draw(BufferGraphics);
            ball.update(paddles);
            if (ball.justScored)
            {
                ball = newBall();
            }

            myBuffer.Render();
        }
        
        private Ball newBall()
        {
            Random r = new Random();
            Point ballVelocity = new Point(speedMultiplier * r.Next(2, 3), speedMultiplier * r.Next(1, 3));
            if (r.Next(0,2) == 1)
            {
                ballVelocity.X = -ballVelocity.X;
            }
            if (r.Next(0, 2) == 1)
            {
                ballVelocity.Y = -ballVelocity.Y;
            }
            return new Ball(width / 2, height / 2, ballVelocity, ballBitmap ,new Rectangle(0, 0, width, height));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (Paddle paddle in paddles)
            {
                paddle.KeyDown(e.KeyCode);
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (Paddle paddle in paddles)
            {
                paddle.KeyUp(e.KeyCode);
            }
        }      
    }

```
