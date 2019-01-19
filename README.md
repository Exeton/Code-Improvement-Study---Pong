# Pong
## Description
This study is meant to show you my thought process when rewriting code. It will showcase rewriting part of a pong game I wrote in C#. The solution file in this repo is the original game, so you can download it and follow along. Please note, this lesson doesn't address some of the issues with the program such as the outdated drawing method. Also, please note WinForms is outdated.

## How is this project implemented
My goal in creating this project was to use some of the new OOP principles I'd learned to rewrite pong. That being said, when implementing the project, I created a Ball and Paddle class which store most of the program's state (or data) and also tried using the [game loop pattern](http://gameprogrammingpatterns.com/game-loop.html). Although I didn't follow the pattern properly, my game loop was composed of a fixed interval timer running drawing and physics code.

## General Readabillity

Here's the current implementation of Form1

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

Right off the bat, you can see that the methods aren't properly spaced, with some methods having spaces after them, and others not. This makes the code look sloppy in general, and is an easy fix. Using proper conventions makes your code easier to read, and saves the reader from having to do extra work when reading your code. It should also be noted that the constructor is above all the class variables, which is also bad practice.

```C#
     public partial class Form1 : Form
    {
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

		public Form1()
		{
			InitializeComponent();
		}

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

Now that the code is easier to skim, let's start from the top and look at Form1. We'll start with all the variables in Form1.

```C#
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
```

There are a few naming convention issues, however I'll try not to empathize these too much as naming conventions are language specific. That being said, "speedMultiplier" and "MovementPixels" clearly don't follow the same format. Naming conventions aside, this code raises a few questions.

..* What are speedMultiplier and MovementPixels? Are they related to the ball or paddles?
..* What does paddleOffset do? 
..* What are the width and height varriables for? Knowing they're for keeping track of the form width / heigh, why are they neccessary? Is this a design flaw?
..* Why are we storing the ball bitmap?


Okay, let's address those questions and a few others. A very crucial and also easy problem to fix are some poorly chosen names like speedMultiplier and MovementPixels. Looking at the code, speedMultiplier is multiplied by a random value to determine the ball's starting velocity. Perhaphs a better name would be InitialBallSpeedMultiplier or AverageInitialBallVelocity. The latter isn't truly reflective of the code, however if the code determining the ball's initial velocity was rewritten this would be an intuitive name.

We'll also change MovementPixels to PaddleSpeedInPixelsPerTick. This is a pretty long name, and can be changed if I think of a better one in the future.

Finally, we'll change paddleOffset to PaddlePadding which I find slightly more descriptive, especially if you have experience with css. This property determines the amount of pixels between the paddle and the left / right edge of the screen. This is called padding when refering to element's relative positions on a webpage.

```c#
        private const int InitialBallSpeedMultiplier = 6;
        public const int PaddleSpeedInPixelsPerTick = 10;
        private const int PaddlePadding = 20;
        private const int paddleWidth = 10;
        private const int paddleHeight = 85;

        private int height;
        private int width;

        private Paddle[] paddles;
        private Ball ball;
        private Timer t;

        private Bitmap ballBitmap;
```

Now that the naming conventions are improved, let's look at a few of my other concerns.
..* What are the width and height varriables for? Knowing they're for keeping track of the form width / heigh, why are they neccessary? Is this a design flaw?
..* Why are we storing the ball bitmap?
