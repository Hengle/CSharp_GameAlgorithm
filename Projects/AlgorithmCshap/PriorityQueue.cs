﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmCshap
{
    #region Priority Queue(heap)

    class PriorityQueue<T> where T : IComparable<T> // T이긴한데, 반드시 IComparable 이라는 인터페이스를 제공해야한다.
    {
        List<T> _heap = new List<T>(); //동적배열 형태로 데이터 소지

        public void Push(T data)
        {
            // 힙의 맨 끝에 새로운 데이터를 삽입한다.
            _heap.Add(data);

            int now = _heap.Count - 1; // 배열 인덱스 이용

            // 도장깨기 시작 logN (밑2)
            while (now > 0)
            {
                int next = (now - 1) / 2; // 자신의 부모 인덱스
                if (_heap[now].CompareTo(_heap[next]) < 0)
                    break;

                // 두값을 교체한다.
                T temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                // 검사 위치를 이동한다.
                now = next;
            }
        }

        public T Pop()
        {
            // 반환할 데이터를 따로 저장
            T ret = _heap[0];

            // 마지막 데이터를 root로 옮긴다.
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);
            lastIndex--;

            int now = 0;
            while (true)
            {
                int left = 2 * now + 1;
                int right = 2 * now + 2;

                int next = now;
                // 왼쪽 하위노드가 현재값보다 크면 왼쪽으로 이동
                if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                    next = left;
                // 오른쪽 하위노드가 현재값보다 크면 오른쪽으로 이동
                if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                    next = right;

                // 왼쪽 오른쪽 모두 현재값보다 작으면 종료
                if (next == now)
                    break;

                // 두 값을 교체한다
                T temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                now = next;
            }
            return ret;
        }

        public int Count { get { return _heap.Count; } }
    }

    #endregion
}
