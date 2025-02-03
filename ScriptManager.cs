using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxtRPG
{
    public class ScriptManager : Program
    {
        public void InvalidInputScript()
        {
            Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
            Thread.Sleep(1000);
        }

        public void JoinLobbyScript()
        {
            Console.WriteLine($"잠시 후 로비로 이동합니다.");
            Thread.Sleep(1500);
        }
    }
}
