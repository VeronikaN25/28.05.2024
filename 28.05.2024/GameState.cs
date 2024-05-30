using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _28._05._2024
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir {get; private set;}
        public int Score {get; private set;}
        public bool GameOver {get; private set;}

        private readonly LinkedList<Position> snakePositions =  new LinkedList<Position>(); 
        private readonly Random random = new Random();  
        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();

        public GameState(int rows, int cols)
        {
            Rows = rows;

            Cols = cols;

            Grid = new GridValue[rows,cols];

            Dir = Direction.Right;
            Random rand = new Random();
            int foodTypeRand = rand.Next(100);
            AddSnake();
            AddFood(foodTypeRand);

        }
        private void AddSnake()
        {
            int r = Rows / 2;
            for(int c=1;c<= 3; c++)
            {
                Grid[r, c] =  GridValue.Snake;
                snakePositions.AddFirst(new Position(r, c));
            }

        }
        private IEnumerable<Position> EmptyPositions()
        {
            for(int r =0;r< Rows; r++)
            {
                for(int c=0;c<Cols; c++)
                {
                    if (Grid[r, c] == GridValue.Empty)
                    {
                        yield return new Position(r, c);
                    }
                }
            }
        }

        private void AddFood(int foodTypeRand)
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if(empty.Count == 0)
            {
                return;
            }

            GridValue foodType;
            Position pos= empty[random.Next(0, empty.Count)];
            if (foodTypeRand < 50)
            {
                foodType = GridValue.Food1;
            }
            else if(foodTypeRand < 90)
            {
                foodType = GridValue.Food2;
            }
            else
            {
                foodType = GridValue.Food3;
            }
            Grid[pos.Row,pos.Col] = foodType;
        }

        public Position HeadPosition()
        {
            return snakePositions.First.Value;
        }

        public Position TailPosition()
        {
            return snakePositions.Last.Value;
        }

        public IEnumerable<Position> SnakePositions()
        {
            return snakePositions;
        }

        private void AddHead(Position pos)
        {
            snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            Position tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            snakePositions.RemoveLast();
        }

        private Direction GetLastDirection()
        {
            if(dirChanges.Count == 0)
            {
                return Dir;
            }

            return dirChanges.Last.Value;
        }

        private bool CanChangeDirection(Direction newDir)
        {
            if(dirChanges.Count == 2)
            {
                return false;
            }

            Direction lastDir = GetLastDirection();
            return newDir != lastDir && newDir != lastDir.Opposite();
        }

        public void ChangeDirection(Direction dir)
        {
            if (CanChangeDirection(dir))
            {
                dirChanges.AddLast(dir);
            }
        }

        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;
        }

        private GridValue WillHit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }
            if(newHeadPos == TailPosition())
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        public void Move()
        {
            if (dirChanges.Count>0)
            {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }
            Position newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);

            if(hit == GridValue.Outside || hit == GridValue.Snake)
            {
                GameOver = true;
            }
            else if(hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            else if(hit == GridValue.Food2)
            {
              
                AddHead(newHeadPos);
                Score += 10;
                Random rand = new Random();
                int foodTypeRand = rand.Next(100);
                AddFood(foodTypeRand);
            }
            else if (hit == GridValue.Food1)
            {

                AddHead(newHeadPos);
                Score ++;
                Random rand = new Random();
                int foodTypeRand = rand.Next(100);
                AddFood(foodTypeRand);
            }
            else if (hit == GridValue.Food3)
            {

                AddHead(newHeadPos);
                Score += 50;
                Random rand = new Random();
                int foodTypeRand = rand.Next(100);
                AddFood(foodTypeRand);
            }

        }
    }
}
