# Pong
## Description
This study is meant to show you my thought process when rewriting code. It will showcase rewriting part of a pong game I wrote in C#. The solution file in this repo is the original game, so you can download it and follow along. Please note, this lesson doesn't address some of the issues with the program such as the outdated drawing method. Also, please note WinForms is outdated.

## How is this project implemented
My goal in creating this project was to use some of the new OOP principles I'd learned to rewrite pong. That being said, when implementing the project, I created a Ball and Paddle class which store most of the program's state (or data) and also tried using the [game loop pattern](http://gameprogrammingpatterns.com/game-loop.html). Although I didn't follow the pattern properly, my game loop was composed of a fixed interval timer running drawing and physics code.

## 

