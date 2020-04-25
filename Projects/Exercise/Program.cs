using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    class Graph
    {
        // 1. adj 2차원 배열을 이용하는 방법(속도)
        int[,] adj = new int[6, 6]
        {
            {0,1,0,1,0,0 },
            {1,0,1,1,0,0 },
            {0,1,0,0,0,0 },
            {1,1,0,0,1,0 },
            {1,0,0,1,0,1 },
            {0,0,0,0,1,0 },
        };

        // 2. 리스트 배열을 이용
        List<int>[] adj2 = new List<int>[]
        {
            new List<int>() {1,3 },
            new List<int>() {0,2,3 },
            new List<int>() {1 },
            new List<int>() {0,1,4 },
            new List<int>() {3,5 },
            new List<int>() {4 },
        };

        #region DFS(깊이 우선 탐색)

        bool[] visited = new bool[6];

        // 이차원 배열을 이용한 방식
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

        // 동적배열을 이용한 방식
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

        public void BFS(int start)
        {
            bool[] found = new bool[6]; //false 자동초기화
            int[] parent = new int[6];
            int[] distance = new int[6];
            
            Queue<int> q = new Queue<int>();
            q.Enqueue(start);
            found[start] = true;

            /// 아래처럼 다양한 정보를 BFS에서는 얻을 수 있다.
            parent[start] = start; // 연결된 이전 부모
            distance[start] = 0; // 현재까지의 거리

            // 대기열 큐에 모두 없을때까지
            while(q.Count > 0)
            {
                int now = q.Dequeue();
                Console.WriteLine(now);

                for(int next = 0; next<6;next++)
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

    }

    class Program
    {
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
            graph.BFS(0);
        }
    }
}
