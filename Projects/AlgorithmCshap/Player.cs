using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * 시작전 설계 아이디어
 * Board와 Player와의 연결이 필요함.
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

            //BFS();

            AStar();
        }

        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo(PQNode other)
            {
                if (F == other.F)
                    return 0;
                // 작으면 좋은거(1)
                return F < other.F ? 1 : -1;
            }
        }



        void AStar()
        {
            // 상하좌우 이동가능성 판별 여부에 사용하는 변수 (U L D R)
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };
            int[] cost = new int[] { 1, 1, 1, 1 };

            // 대각선을 넣을수도 있다 A*는 (U L D R, UL DL DR UR)
            //int[] deltaY = new int[] { -1, 0, 1, 0, -1, 1, 1, -1 };
            //int[] deltaX = new int[] { 0, -1, 0, 1, -1, -1, 1, 1 };
            //int[] cost = new int[] { 10, 10, 10, 10, 14, 14, 14, 14 };

            // 점수 매기기
            // F = G + H
            // F = 최종 점수 (작을 수록 좋음, 경로에 따라 달라짐)
            // G = 시작점에서 해당 좌표까지 이동하는데 드는 비용 (작을 수록 좋음, 경로에 따라 달라짐)
            // H(휴리스틱) = 목적지에서 얼마나 가까운지 (작을수록 좋음, 고정인값)

            // (y,x) 이미 방문했는지 여부를 기록 (방문 = closed 상태)
            bool[,] closed = new bool[_board.Size, _board.Size]; // CloseList

            // (y,x) 가는 길을 한번이라도 발견했었는지
            // 발견 X => Int32.MaxValue
            // 발견 O => F = G + H
            int[,] open = new int[_board.Size, _board.Size]; // OpenList
            // 초기화
            for (int y = 0; y < _board.Size; ++y)
                for (int x = 0; x < _board.Size; ++x)
                    open[y, x] = Int32.MaxValue;

            // 경로 기록 변수
            Pos[,] parent = new Pos[_board.Size, _board.Size];

            // 제일 좋은거 뽑는데 최적화된 자료구조임!
            // open에 있는 정보들 중에서, 가장 종흔 후보를 빠르게 뽑아오기 위한 도구
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();


            // 시작점 발견 (예약 출발) F = G + H
            open[PosY, PosX] = 0 + (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX));
            pq.Push(new PQNode() { F = (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX)), G = 0, Y = PosY, X = PosX });
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (pq.Count > 0)
            {
                // 제일 좋은 후보를 찾는다.
                PQNode node = pq.Pop();
                // 동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문(closed)된 경우 스킵
                if (closed[node.Y, node.X])
                    continue;

                // 방문한다
                closed[node.Y, node.X] = true;

                // 목적지 도착했으면 바로 종료
                if (node.Y == _board.DestY && node.X == _board.DestX)
                    break;

                // 상하좌우 등 이동할 수 있는 좌표인지 확인해서 예약(open)한다.
                for (int i = 0; i < deltaY.Length; ++i)
                {
                    // 다음 좌표계산
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];

                    // 경로 벗어나면 스킵
                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    // 다음 경로가 벽이면 스킵
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    // 이미 지나온 길이라면 스킵
                    if (closed[nextY, nextX])
                        continue;

                    // 비용 계산
                    int g = node.G + cost[i];
                    int h = (Math.Abs(_board.DestY - nextY) + Math.Abs(_board.DestX - nextX));

                    // 다른 경로에서 더 빠른 길을 이미 찾았으면 스킵
                    if (open[nextY, nextX] < g + h)
                        continue;

                    /// 여기까지오면 가장 빠른 길을 찾은거
                    /// 예약을 최종적으로 진행한다.
                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X);
                }
            }

            CalcPathFromParent(parent);
        }


        void CalcPathFromParent(Pos[,] parent)
        {
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


        void BFS()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };


            bool[,] found = new bool[_board.Size, _board.Size];
            Pos[,] parent = new Pos[_board.Size, _board.Size];

            Queue<Pos> q = new Queue<Pos>();
            q.Enqueue(new Pos(PosY, PosX));
            found[PosY, PosX] = true;
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (q.Count > 0)
            {
                Pos pos = q.Dequeue();
                int nowY = pos.Y;
                int nowX = pos.X;

                // 4방향 모두 검사
                for (int i = 0; i < 4; ++i)
                {
                    int nextY = nowY + deltaY[i];
                    int nextX = nowX + deltaX[i];

                    // 경로 벗어나면 넘기기
                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
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

            CalcPathFromParent(parent);
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

        const int MOVE_TICK = 40; // 100 ms (0.1초)
        int _sumTick = 0;
        int _lastIndex = 0;

        public void Update(int deltaTick)
        {
            if (_lastIndex >= _points.Count)
            {
                _lastIndex = 0;
                _points.Clear();
                _board.Initialize(_board.Size, this);
                Initialize(1, 1, _board);
            }

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
