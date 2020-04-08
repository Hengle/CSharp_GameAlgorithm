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
        const int DEFAULT_SIZE = 1;
        T[] _data = new T[DEFAULT_SIZE];

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

        public void Initialize()
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

        }
    }
}
