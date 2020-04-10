using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmCshap
{
    #region 1. 동적배열
    // 1. 동적배열 구현
    class MyList<T>
    {
        const int DEFAULTSize = 1;
        T[] _data = new T[DEFAULTSize];

        public int Count = 0; // 실제로 사용중인 데이터 개수
        public int Capacity { get { return _data.Length; } } // 예약된 데이터 개수


        // O(1), 예외케이스 : 추가 할당은 예외로 가정한다.
        public void Add(T item)
        {
            // 1. 공간이 충분히 남아있는지 확인
            if (Count >= Capacity)
            {
                // 공간을 다시 늘려서 확보한다
                T[] newArray = new T[Count * 2]; // 우리 정책은 2배로 늘린다.
                for (int i = 0; i < Count; ++i)
                    newArray[i] = _data[i];
                _data = newArray;
            }

            // 2. 공간에 데이터를 넣어준다
            _data[Count] = item;
            Count++;
        }

        // O(1)
        public T this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; } // rValue는 value 키워드로 들어옴
        }

        // O(N) : 엄밀히 N은 아니겠지만, 야매로 N이라고 하는편.
        public void RemoveAt(int index)
        {
            for (int i = index; i < Count - 1; ++i)
                _data[i] = _data[i + 1];
            _data[Count - 1] = default(T); // 정수형 0, 클래스 타입은 null
            Count--;
        }
    }
    #endregion

    #region 2. 연결리스트
    class MyLinkedListNode<T>
    {
        public T Data;
        // 참조값(주소값을 가지고 있음)
        public MyLinkedListNode<T> Next;
        public MyLinkedListNode<T> Prev;
    }

    /////////////////////////////////////////////
    /////////////////////////////////////////////

    // 2. 연결리스트 구현
    class MyLinkedList<T>
    {
        public int Count = 0; // 총 방의 개수
        public MyLinkedListNode<T> Head = null; // 첫번째 방 위치 주소
        public MyLinkedListNode<T> Tail = null; // 마지막 방 위치 주소

        // O(1)
        public MyLinkedListNode<T> AddLast(T data)
        {
            MyLinkedListNode<T> newRoom = new MyLinkedListNode<T>(); // 새로운 방 생성
            newRoom.Data = data; // 거기에 값 대입

            // 아직 방이 아예 없었다면, 새로 추가한 방이 Headㄴ
            if (Head == null)
                Head = newRoom;

            // 마지막 방이 존재했다면, 새로 추가한 방과 '연결'
            if (Tail != null)
            {
                Tail.Next = newRoom;
                newRoom.Prev = Tail;
            }

            Tail = newRoom; // 마지막 방 주소 'Update'
            Count++; // 방 개수 증가
            return newRoom;
        }

        // O(1) : 연결리스트의 장점. 제거 속도가 상수시간임!ㄴ
        public void Remove(MyLinkedListNode<T> room)
        {
            /// 없는 방 제거와 관려된 예외처리는 일단 귀찮아서 생략(하지만 필수임)

            // 1. 하필, 첫번째 방을 제거한다면
            if (Head == room)
                Head = Head.Next;

            // 2. 하필, 마지막 방을 제거한다면
            if (Tail == room)
                Tail = Tail.Prev;

            // 3-1. 그게 아닌 일반적인 상황 (왼쪽 노드)
            if (room.Prev != null)
                room.Prev.Next = room.Next;

            // 3-2. 오른쪽 노드가 있다면
            if (room.Next != null)
                room.Next.Prev = room.Prev;

            Count--;
        }

    }
    #endregion

    /////////////////////////////////////////////
    /////////////////////////////////////////////

    class Board
    {
        public int[] _data = new int[25]; // 배열
        public MyList<int> _data2 = new MyList<int>(); // 동적배열(cpp에서는 vector임)
        public MyLinkedList<int> _data3 = new MyLinkedList<int>(); // 연결리스트(양방향) (cpp에서는 list임)

        public TileType[,] Tile { get; private set; } // 맵 타일 2차원 배열
        public int Size { get; private set; }
        const char CIRCLE = '\u25cf'; // 동그라미

        public int DestY { get; private set; }
        public int DestX { get; private set; }


        Player _player;

        /*
         * 가능하다면 0 또는 1과 같은 숫자로 하드코딩하기 보다는
         * enum 타입을 이용해서 관리해주는게 더 좋다.
         */
        public enum TileType
        {
            Empty,
            Wall,
        }

        public void Initialize(int size, Player player)
        {
            #region 동적배열 실행예제
            _data2.Add(101);
            _data2.Add(102);
            _data2.Add(103);
            _data2.Add(104);
            _data2.Add(105);

            int temp = _data2[2];

            _data2.RemoveAt(2);
            #endregion

            // 연결리스트 실행예제
            {
                _data3.AddLast(101);
                _data3.AddLast(102);
                MyLinkedListNode<int> node = _data3.AddLast(103);
                _data3.AddLast(104);
                _data3.AddLast(105);

                _data3.Remove(node);

                // ※ 연결리스트는 검색이 O(n) 이라는 단점이 있다.
            }

            /* 
             * 우리 미로의 특성상 홀수만 들어와야함.
             * 짝수라면 길뚫는 작업에서 x+1에서 범위를 벗어남.
             * 그래서 아래처럼 return으로 임시 예외처리 해놓음.
             */
            if (size % 2 == 0)
                return;

            Tile = new TileType[size, size];
            Size = size;

            DestY = Size - 2;
            DestX = Size - 2;

            _player = player;

            // Mazes for programmers 책 서적에 의한 미로 생성 알고리즘을 따라가고 있음.
            // GenerateByBinaryTree(); // 맵생성 알고리즘
            GenerateBySideWinder(); // 2번째 맵생성 알고리즘.
        }

        private void GenerateBySideWinder()
        {
            // 길을 막는 작업
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // 벽 지정
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    // 내부 지정
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }

            Random rand = new Random(); // 랜덤을 생성

            for (int y = 0; y < Size; y++)
            {
                int count = 1; //점의 개수(1부터 시작)
                for (int x = 0; x < Size; x++)
                {
                    #region 벽 생성 예외부분
                    // 짝수 부분(벽으로 지정됨)은 넘어간다.
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    // 가장 마지막 위치를 넘어가게함.
                    if (y == Size - 2 && x == Size - 2)
                        continue;

                    // 가장 외각은 막아야 한다.
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue; // 뚫는 작업 무시하고 다음으로 넘어가게함.
                    }

                    // 가장 외각은 막아야 한다.
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue; // 뚫는 작업 무시하고 다음으로 넘어가게함.
                    }
                    #endregion

                    if (rand.Next(0, 2) == 0)
                    {
                        // 0일때는 우측에 길을 만든다. 우측이니 x+1임
                        Tile[y, x + 1] = TileType.Empty;
                        count++;
                    }
                    else
                    {
                        int randomIndex = rand.Next(0, count);
                        Tile[y + 1, x - randomIndex * 2] = TileType.Empty;
                        count = 1;
                    }
                }
            }
        }

        //(Binary Tree Algorithm) 맵생성
        void GenerateByBinaryTree()
        {
            // 길을 막는 작업
            // 첫 루프는 y좌표를 넣어줄거임
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // 벽 지정
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    // 내부 지정
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }

            Random rand = new Random(); // 랜덤을 생성

            // 랜덤으로 우측 혹은 아래로 길을 뚫는 작업 
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // 벽 부분
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    // 가장 마지막 위치를 넘어가게함.
                    if (y == Size - 2 && x == Size - 2)
                        continue;

                    // 가장 외각은 막아야 한다.
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue; // 뚫는 작업 무시하고 다음으로 넘어가게함.
                    }

                    // 가장 외각은 막아야 한다.
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue; // 뚫는 작업 무시하고 다음으로 넘어가게함.
                    }

                    // 0혹은 1 둘중 하나가 나옴(정수), 두번째 2는 미포함임!
                    if (rand.Next(0, 2) == 0)
                    {
                        // 0일때는 우측에 길을 만든다. 우측이니 x+1임
                        Tile[y, x + 1] = TileType.Empty;
                    }
                    else
                    {
                        Tile[y + 1, x] = TileType.Empty;
                    }
                }
            }
        }

        public void Render()
        {
            // 디폴트 색 지정
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // 플레이어 좌표를 갖고와서, 그 좌표랑 현재 y, x가 일치하면 플레이어 전용 색상으로 표시.
                    if (y == _player.PosY && x == _player.PosX)
                        Console.ForegroundColor = ConsoleColor.Blue; // 플레이어 색상 지정
                    else if (y == DestY && x == DestX)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = GetTileColor(Tile[y, x]); // 색지정

                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }

            // 디폴트 색 복원
            Console.ForegroundColor = prevColor;
        }

        ConsoleColor GetTileColor(TileType type)
        {
            switch (type)
            {
                case TileType.Empty:
                    return ConsoleColor.Green; // return은 굳이 break 필요없음
                case TileType.Wall:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Green;
            }
        }
    }
}
