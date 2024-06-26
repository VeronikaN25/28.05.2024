﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _28._05._2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> gridValRoImage = new()
        {
            {GridValue.Empty , Images.Empty },
            {GridValue.Snake , Images.Body },
            {GridValue.Food1 , Images.Food1 },
            {GridValue.Food2 , Images.Food2 },
            {GridValue.Food3 , Images.Food3 },
        };

        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            {Direction.Up , 0 },
            {Direction.Right , 90 },
            {Direction.Down , 100 },
            {Direction.Left , 270 },
        };


        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;
        private int gameSpeed = 130, speed = 6;
      

        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols);
            UpdateSpeedDisplay();
        }

        private async Task RunGame() 
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            gameState = new GameState(rows, cols);
        } 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Left:
                    gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.Right:
                    gameState.ChangeDirection(Direction.Right);
                    break;
                case Key.Up:
                    gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    gameState.ChangeDirection(Direction.Down);
                    break;
                case Key.Add:
                case Key.OemPlus:
                    gameSpeed = Math.Max(10, gameSpeed-10);
                    speed += 1;
                    UpdateSpeedDisplay();
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    gameSpeed +=10;
                    speed -= 1;
                    UpdateSpeedDisplay();
                    break;

            }
        }
        private void UpdateSpeedDisplay()
        {
            SpeedText.Text = $"Speed: {speed}";
        }
        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(gameSpeed);
                gameState.Move();
                Draw();
            }
        }
        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;

            for (int r=0; r < rows; r++)
            {
                for(int c = 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5,0.5)
                    };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }
        private void Draw()
        {
            
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"Score: {gameState.Score}";
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if (!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
            }
        }

        private void DrawGrid()
        {
            for(int r =0; r < rows; r++)
            {
                for(int c=0; c < cols; c++)
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    gridImages[r, c].Source = gridValRoImage[gridVal];
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }
        private async Task DrawDeadSnake()
        {
            List<Position> positions = new List<Position>(gameState.SnakePositions());
            for(int i =0; i <positions.Count; i++)
            {
                Position pos = positions[i];
                ImageSource source = (i==0) ? Images.DeadHead : Images.DeadBody;
                gridImages[pos.Row, pos.Col].Source = source;
                await Task.Delay(50);
            }
        }
        private void DrawSnakeHead()
        {
            Position headPos = gameState.HeadPosition();
            Image image = gridImages[headPos.Row, headPos.Col];
            image.Source = Images.Head;

            int rotation = dirToRotation[gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }

        private async Task ShowCountDown()
        {
            for(int i =3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }
        private async Task ShowGameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "PRESS ANY KEY TO START";
        }
    }
}