using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * 시작전 설계 아이디어 :
 * Board와 Player와의 연결이 필요함.
 * 
 */
namespace AlgorithmCshap
{
    class Pos
    {
        public Pos(int y, int x) { Y = y; X = x; }
        public int Y;
        public int X;
    }

    class Player
    {
        public int PosY { get; private set; }
        public int PosX { get; private set; }

        Board _board;

        Random _random = new Random();

        enum Dir // Direction 방향
        {
            Up,
            Left,
            Down,
            Right
        }

        int _dir = (int)Dir.Up; //연산하기 편하게 int값으로 갖고있음

        List<Pos> _points = new List<Pos>(); // 실제 점 위치

        public void Initialize(int posY, int posX, Board board)
        {
            PosY = posY;
            PosX = posX;

            _board = board;

            // 우수법
            //RightHand();

            BFS();
            BFS();
            
        }

        void BFS()
        {
            int[] deltaY = new int[] {-1,0,1,0 };
            int[] deltaX = new int[] {0,-1,0,1 };


            bool[,] found = new bool[_board.Size, _board.Size];
            Pos[,] parent = new Pos[_board.Size, _board.Size];

            Queue<Pos> q = new Queue<Pos>();
            q.Enqueue(new Pos(PosY, PosX));
            found[PosY, PosX] = true;
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while(q.Count>0)
            {
                Pos pos = q.Dequeue();
                int nowY = pos.Y;
                int nowX = pos.X;

                // 4방향 모두 검사
                for(int i=0; i<4;++i)
                {
                    int nextY = nowY + deltaY[i];
                    int nextX = nowX + deltaX[i];

                    // 경로 벗어나면 넘기기
                    if (nextX < 0 || nextX >= _board.Size || nextY<0|| nextY>= _board.Size)
                        continue;
                    // 다음 경로가 벽이면 넘어가기
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    // 이미 지나온 길이라면 넘어가기
                    if (found[nextY, nextX])
                        continue;

                    q.Enqueue(new Pos(nextY, nextX));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(nowY, nowX);
                }
            }

            // 마지막점 기록
            int y = _board.DestY;
            int x = _board.DestX;

            // 거꾸로 거슬러 올라가면서 시작위치까지 추적
            while (parent[y, x].Y != y || parent[y, x].X != x)
            {
                _points.Add(new Pos(y, x)); //
                Pos pos = parent[y, x]; // 현재위치를 이전 부모의 위치로 바꿔줌
                y = pos.Y;
                x = pos.X;
            }

            _points.Add(new Pos(y, x)); // 마지막 시작위치점을 넣어줌
            // 여기까지 하면 역추적한 순서대로 _point라는 리스트 변수에 다 들어갈텐데, 실제 실행은 반대로 되길 바라기 때문에 Reverse()기능을 이용해서 뒤집음.
            _points.Reverse();
        }

        // 우수법
        void RightHand()
        {
            // 현재 바라보고 있는 방향을 기준으로 좌표 변화를 나타낸다.
            int[] frontY = new int[] { -1, 0, 1, 0 }; //4개짜리 배열
            int[] frontX = new int[] { 0, -1, 0, 1 }; //4개짜리 배열

            int[] rightY = new int[] { 0, -1, 0, 1 };
            int[] rightX = new int[] { 1, 0, -1, 0 };

            _points.Add(new Pos(PosY, PosX)); // 시작 위치 넣어줌.

            // 목표지점에 도착할때까지 실행 (오른손 법칙, 우수법)
            while (PosY != _board.DestY || PosX != _board.DestX)
            {
                // 1. 현재 바라보는 방향을 기준으로 오른쪽으로 갈 수 있는지 확인.
                if (_board.Tile[PosY + rightY[_dir], PosX + rightX[_dir]] == Board.TileType.Empty)
                {
                    // 오른쪽 방향으로 90도 회전
                    _dir = (_dir - 1 + 4) % 4;
                    // 앞으로 한 보 전진 
                    PosY = PosY + frontY[_dir];
                    PosX = PosX + frontX[_dir];
                    _points.Add(new Pos(PosY, PosX)); // 위치 변화 기록
                }
                // 2.현재 바라보는 방향을 기준으로 전진할 수 있는지 확인.
                else if (_board.Tile[PosY + frontY[_dir], PosX + frontX[_dir]] == Board.TileType.Empty)
                {
                    // 앞으로 한 보 전진
                    PosY = PosY + frontY[_dir];
                    PosX = PosX + frontX[_dir];
                    _points.Add(new Pos(PosY, PosX)); // 위치 변화 기록
                }
                // 3. 그 외 모든 경우
                else
                {
                    // 왼쪽 방향으로 90도 회전
                    _dir = (_dir + 1) % 4;
                }
            }
        }
        

        const int MOVE_TICK = 50; // 100 ms (0.1초)
        int _sumTick = 0;

        int _lastIndex = 0;
        public void Update(int deltaTick)
        {
            if (_lastIndex >= _points.Count)
                return;

            _sumTick += deltaTick;
            if (_sumTick >= MOVE_TICK)
            {
                _sumTick = 0; // 다시 초기화

                PosY = _points[_lastIndex].Y;
                PosX = _points[_lastIndex].X;
                _lastIndex++;
                #region
                /*
                // 0.1초마다 실행될 로직
                int randValue = _random.Next(0, 5);
                switch (randValue)
                {
                    case 0: // 상
                        // 바로 윗부분이 빈 공간이라면
                        if (PosY - 1 >= 0 && _board.Tile[PosY - 1, PosX] == Board.TileType.Empty)
                            PosY = PosY - 1; // 위치를 바꿔주기
                        break;
                    case 1: // 하
                        if (PosY + 1 < _board.Size && _board.Tile[PosY + 1, PosX] == Board.TileType.Empty)
                            PosY = PosY + 1; // 위치를 바꿔주기
                        break;
                    case 2: // 좌
                        if (PosX - 1 >= 0 && _board.Tile[PosY, PosX - 1] == Board.TileType.Empty)
                            PosX = PosX - 1; // 위치를 바꿔주기
                        break;
                    case 3: // 우
                        if (PosX + 1 < _board.Size && _board.Tile[PosY, PosX + 1] == Board.TileType.Empty)
                            PosX = PosX + 1; // 위치를 바꿔주기
                        break;
                    default:
                        break;
                }
                */
                #endregion
            }
        }
    }
}
