﻿using System;
using System.Collections.Generic;

namespace Exercise
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

        public int Count()
        {
            return _heap.Count;
        }
    }

    class Knight : IComparable<Knight>
    {
        public int Id { get; set; }

        public int CompareTo(Knight other)
        {
            if (Id == other.Id)
                return 0;
            return Id > other.Id ? 1 : -1;
        }
    }

    #endregion

    class TreeNode<T>
    {
        public T Data { get; set; }
        public List<TreeNode<T>> Children { get; set; } = new List<TreeNode<T>>();
    }

    class Graph
    {
        // 1. 인접 행렬 이용
        int[,] adj = new int[6, 6]
        {
            { 0, 1, 0, 1, 0, 0 },
            { 1, 0, 1, 1, 0, 0 },
            { 0, 1, 0, 0, 0, 0 },
            { 1, 1, 0, 0, 1, 0 },
            { 0, 0, 0, 1, 0, 1 },
            { 0, 0, 0, 0, 1, 0 },
        };

        // 2. 인접 리스트 이용
        List<int>[] adj2 = new List<int>[]
        {
            new List<int>() { 1, 3 },
            new List<int>() { 0, 2, 3 },
            new List<int>() { 1 },
            new List<int>() { 0, 1, 4 },
            new List<int>() { 3, 5 },
            new List<int>() { 4 },
        };

        // 3. 가중치 그래프일때
        int[,] adj3 = new int[6, 6]
        {
            { -1, 15, -1, 35, -1, -1 },
            { 15, -1,  5, 10, -1, -1 },
            { -1,  5, -1, -1, -1, -1 },
            { 35, 10, -1, -1,  5, -1 },
            { -1, -1, -1,  5, -1,  5 },
            { -1, -1, -1, -1,  5, -1 },
        };

        #region DFS(깊이 우선 탐색)

        /*
         * 우수법에서 더 좋은 방법을 구안해낸 방법
         */

        bool[] visited = new bool[6];

        // 인접 행렬을 사용할때
        // 1) 우선 시작부터 방문하고
        // 2) 시작지점과 연결된 정점들을 하나씩 확인해서, [아직 미발견(미방문) 상태라면] 방문한다.
        public void DFS(int now)
        {
            Console.WriteLine(now);
            visited[now] = true;

            for (int next = 0; next < adj.GetLength(0); next++)
            {
                // 연결된 선이 없다면
                if (adj[now, next] == 0)
                    continue;
                // 다음 방문지가 이미 방문한 곳이라면
                if (visited[next])
                    continue;
                // 재귀함수를 이용.
                DFS(next);
            }
        }

        // 인접 리스트를 이용한 방식
        public void DFS2(int now)
        {
            Console.WriteLine(now);
            visited[now] = true;

            foreach (int next in adj2[now])
            {
                // 이미 방문하면 넘어가기
                if (visited[next])
                    continue;
                // 재귀함수를 이용.
                DFS2(next);
            }
        }

        public void SearchAll()
        {
            visited = new bool[6]; // 모두 false로 자동 초기화됨.
            for (int now = 0; now < 6; now++)
                if (visited[now] == false)
                    DFS(now);
        }
        #endregion

        #region BFS(너비 우선 탐색)
        // 인접행렬 방식으로 구현
        public void BFS(int start)
        {
            bool[] found = new bool[6]; // false 자동초기화
            int[] parent = new int[6]; // 이전 경로를 기록
            int[] distance = new int[6]; // 몇번만에 갔는지를 기록

            // 예약 큐를 만듦
            Queue<int> q = new Queue<int>();
            q.Enqueue(start);
            found[start] = true;

            /// 아래처럼 다양한 정보를 BFS에서는 얻을 수 있다.
            parent[start] = start; // 연결된 이전 부모
            distance[start] = 0; // 현재까지의 거리

            // 대기열 큐에 모두 없을때까지
            while (q.Count > 0)
            {
                int now = q.Dequeue();
                Console.WriteLine(now);

                for (int next = 0; next < 6; next++)
                {
                    // 인접한 경로가 없다면 넘어감
                    if (adj[now, next] == 0)
                        continue;
                    // 이미 지나온 경로도 무시함
                    if (found[next])
                        continue;
                    q.Enqueue(next); // 큐에 넣어줌
                    found[next] = true; // 찾았다는걸 알림
                    parent[next] = now;
                    distance[next] = distance[now] + 1;
                }
            }
        }
        #endregion

        #region Dijikstra(가중치 최단경로)

        public void Dijikstra(int start)
        { 
            // 찾았냐가 중요한게 아니라, 최종적으로 방문했는지가 중요하다.
            bool[] visited = new bool[6]; // 방문기록(기본 false 초기화)
            int[] distance = new int[6]; // 거리값 기록, 발견 여부도 동시에 판별 예정
            int[] parent = new int[6]; // 이전 경로 기록 

            // 가장 큰 int형 변수를 넣음(21억), 이유는 실제 길이가 0인지 아니면 초기화값으로 인해 0인지 구분이 안되기 때문. 아무숫자나 상관없음
            for (int i = 0; i < distance.Length; ++i)
                distance[i] = Int32.MaxValue; //Int32 최고값을 넣어줌.

            // 초기화
            distance[start] = 0; // 시작 지점의 거리값은 0으로 지정, 발견 여부도 동시에 알수 있음
            parent[start] = start; // 시작지점의 이전 경로는 '본인'으로 기록

            // ※다익스트라는 예약 큐가 없다.
            while (true)
            {
                #region ★많이쓰는 패턴, 어떤 배열중 가장 큰 값을 찾기 위할때★
                
                // 가장 가까이에 있는 제일 좋은 후보를 찾는다.

                // 가장 유력한 후보의 거리와 번호를 저장
                int closest = Int32.MaxValue; // 거리 비교 변수
                int now = -1; // 

                /// ※ 거의 2중 for문 수준으로 비효율적임, 1만개가 넘어가면 for문도 1만개 조사해야함 -> 우선순위큐로 해결가능
                for (int i = 0; i < 6; ++i)
                {
                    /// 후보에서 걸러지는 것들
                    // 1.이미 방문한 정점은 스킵.
                    if (visited[i])
                        continue;
                    // 2.아직 발견된 적이 없거나, 3.발견은 했지만 기존 후보보다 멀리 있으면 스킵
                    if (distance[i] == Int32.MaxValue || distance[i] >= closest)
                        continue;

                    // 여태껏 발견한 가장 좋은 후보이므로 갱신
                    closest = distance[i];
                    now = i;
                }
                #endregion

                // 만약 다음 후보가 하나도 없다 -> 종료 (모든 경로를 갔거나, 선이 단절되어 더이상 갈곳이 없다는 의미)
                if (now == -1)
                    break;

                // 제일 좋은 후보를 찾았으니까 방문한다.
                visited[now] = true;

                // 방문한 정점과 인접한 정점들을 조사해서 상황에 따라 발견한 최단거리를 갱신한다.
                for (int next = 0; next < 6; next++)
                {
                    // 연결되지 않은 정점 스킵
                    if (adj3[now, next] == -1)
                        continue;
                    // 이미 방문한 정점은 스킵
                    if (visited[next])
                        continue;

                    // 새로 조사된 정점의 최단거리를 계산한다.
                    int nextDist = distance[now] + adj3[now, next];

                    // 만약 기존에 발견한 최단거리가 새로 조사된 최단거리보다 크면 정보를 갱신
                    if (nextDist < distance[next])
                    {
                        distance[next] = nextDist; // 새롭게 갱신
                        parent[next] = now; // next는 now로 출발한 경로이다
                    }
                }
            }
        }
        #endregion

    }

    class Exercise
    {
        #region 트리관련 기본 함수

        static TreeNode<string> MakeTree()
        {
            TreeNode<string> root = new TreeNode<string>() { Data = "R1 개발실" };
            {
                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "디자인팀" };
                    node.Children.Add(new TreeNode<string>() { Data = "전투" });
                    node.Children.Add(new TreeNode<string>() { Data = "경제" });
                    node.Children.Add(new TreeNode<string>() { Data = "스토리" });
                    root.Children.Add(node);
                }

                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "프로그래밍팀" };
                    node.Children.Add(new TreeNode<string>() { Data = "서버" });
                    node.Children.Add(new TreeNode<string>() { Data = "클라" });
                    node.Children.Add(new TreeNode<string>() { Data = "엔진" });
                    root.Children.Add(node);
                }

                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "아트팀" };
                    node.Children.Add(new TreeNode<string>() { Data = "배경" });
                    node.Children.Add(new TreeNode<string>() { Data = "캐릭터" });
                    root.Children.Add(node);
                }
            }
            return root;
        }

        // 트리 순회(출력)
        static void PrintTree(TreeNode<string> root)
        {
            // 현재 노드 출력
            Console.WriteLine(root.Data);

            // 재귀를 이용해서 출력
            foreach (TreeNode<string> child in root.Children)
                PrintTree(child);
        }

        // 트리의 높이를 구하는 함수 (면접에 은근히 많이 나옴)
        static int GetHeight(TreeNode<string> root)
        {
            int height = 0;

            foreach (TreeNode<string> child in root.Children)
            {
                // 재귀
                int newHeight = GetHeight(child) + 1;
                if (height < newHeight)
                    height = newHeight;

                // height = Math.Max(height, newHeight); // 위를 이렇게 코딩할수도 있음.
            }

            return height;
        }

        #endregion

        static void Main(string[] args)
        {
            //Stack<int> stack = new Stack<int>();
            //Queue<int> queue = new Queue<int>();

            Graph graph = new Graph();

            /// DFS (Depth First Search 깊이 우선 탐색)
            /// 용도 : 다양함
            //graph.DFS(0);

            /// BFS (Breadth First Search 너비 우선 탐색)
            /// 용도 : 최단거리(길찾기)에서만 주로 사용됨.
            //graph.BFS(0);

            /// Dijikstra 
            graph.Dijikstra(3);

            /// Tree
            //TreeNode<string> root = MakeTree();
            //PrintTree(root);
            //Console.WriteLine(GetHeight(root));

            PriorityQueue<int> q = new PriorityQueue<int>();
            q.Push(20);
            q.Push(10);
            q.Push(30);
            q.Push(90);
            q.Push(40);

            /*
             *  ※ 만약에 최소값 형태의 우선순위 큐를 구현하고 싶을때, 
             *  꼼수중 하나는 값을 넣을때 음수(-1을 곱한형태)로 넣으면 그대로 사용 가능.
             */
            while (q.Count() > 0)
                Console.WriteLine(q.Pop());

            PriorityQueue<Knight> q2 = new PriorityQueue<Knight>();
            q2.Push(new Knight() { Id = 20 });
            q2.Push(new Knight() { Id = 30 });
            q2.Push(new Knight() { Id = 40 });
            q2.Push(new Knight() { Id = 10 });
            q2.Push(new Knight() { Id = 5 });

            Console.WriteLine();

            while (q2.Count() > 0)
                Console.WriteLine(q2.Pop().Id);

        }
    }
}
