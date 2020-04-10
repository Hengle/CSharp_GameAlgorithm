using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmCshap
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Player player = new Player();
            board.Initialize(25, player); //짝수 들어가면 return됨.
            player.Initialize(1, 1, board);

            Console.CursorVisible = false;
            const int WAIT_TICK = 1000 / 30; // 30분의 1초마다 아래 while문 로직이 돌아갈거임

            int lastTick = 0;
            int deltaTick = 0;
            while (true)
            {
                #region 프레임 관리
                // 경과시간이 1/30보다 작으면 
                int currentTick = System.Environment.TickCount;
                if (currentTick - lastTick < WAIT_TICK)
                    continue;
                deltaTick = currentTick - lastTick; // 한 프레임의 시간기록. 업데이트에 넘겨줄예정
                lastTick = currentTick;
                #endregion

                // 입력

                // 로직 (데이터가 변하는 부분)
                player.Update(deltaTick);

                // 렌더링 (최종적으로 그리는 부분)
                Console.SetCursorPosition(0, 0);

                board.Render();
            }
        }
    }
}
