using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxtRPG
{
    public class DungeonManager : Program
    {
        private ScriptManager scriptManager = new ScriptManager();
        Random random = new Random();

        // 던전 로비
        public void DungeonLobby()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine($"던전 입장");
                Console.WriteLine($"던전의 난이도와 권장 방어력을 확인 후 입장 할 난이도를 선택 해주세요.");
                Console.WriteLine($"");
                Console.WriteLine($"1. 쉬운 던전   | 방어력 5 이상 권장");
                Console.WriteLine($"2. 일반 던전   | 방어력 11 이상 권장");
                Console.WriteLine($"3. 어려운 던전 | 방어력 17 이상 권장");
                scriptManager.SelectScript();

                if (int.TryParse(Console.ReadLine(), out int num))
                {
                    switch (num)
                    {
                        case 0:
                            return;
                        case 1:
                            Dungeon(Difficulty.Easy);
                            break;
                        case 2:
                            Dungeon(Difficulty.Normal);
                            break;
                        case 3:
                            Dungeon(Difficulty.Hard);
                            break;
                        default:
                            continue;
                    }
                }

                else
                {
                    scriptManager.InvalidInputScript();
                    continue;
                }

                return;
            }
        }

        // 던전 실행
        public void Dungeon(Difficulty difficulty)
        {
            Console.Clear();

            int reward = 0;
            string difficultyStr = "";

            // 난이도에 따라 권장 방어력 설정
            switch (difficulty)
            {
                case Difficulty.Easy:
                    reward = 1000;
                    difficultyStr = "쉬운";
                    break;

                case Difficulty.Normal:
                    reward = 1700;
                    difficultyStr = "일반";
                    break;

                case Difficulty.Hard:
                    reward = 2500;
                    difficultyStr = "어려운";
                    break;
            }

            // 확률 확인용 변수 생성
            int rnd = random.Next(0, 100);

            // 결과 확인 전 진행 중 텍스트 출력
            Console.Clear();
            Console.WriteLine($"ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ");
            Console.WriteLine($"");
            Console.WriteLine($"         던전 진행 중...");
            Console.WriteLine($"");
            Console.WriteLine($"ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ");

            Thread.Sleep(delay);

            Console.Clear();

            // 던전 공략 실패
            // 40%에 걸리고 권장 방어력보다 낮을 경우 던전 실패
            if (rnd < 40 && player.Defence + player.EquipArmor.Value < (int)difficulty)
            {
                Console.WriteLine($"던전 공략에 실패했습니다.");
                Console.WriteLine($"던전 공략 실패로 인해 체력이 현재 체력의 절반으로 감소합니다.");
                Console.WriteLine($"HP : {player.Health} => HP : {player.Health / 2}");
                player.Health = player.Health / 2;
                scriptManager.JoinLobbyScript();
                return;
            }

            // 던전 공략 성공
            else
            {
                // 체력 감소량 랜덤값 가져오기
                // 권장 방어력보다 높을 시 : (20 - (플레이어 방어력 - 권장 방어력)) ~ (35 - (플레이어 방어력 - 권장 방어력))
                if (player.Defence >= (int)difficulty)
                    rnd = random.Next(20 - (player.Defence - (int)difficulty), 36 - (player.Defence - (int)difficulty));

                // 권장 방어력보다 낮을 시 : (20 - (권장 방어력 - 플레이어 방어력)) ~ (35 - (권장 방어력 - 플레이어 방어력))
                else if (player.Defence < (int)difficulty)
                    rnd = random.Next(20 - ((int)difficulty - player.Defence), 36 - ((int)difficulty - player.Defence));

                // 감소 전 체력 출력 용 임시변수
                int originalHealth = player.Health;

                player.Health -= rnd;

                // 공격력 ~ 공격력 * 2의 %만큼 추가 골드 획득
                float fRnd = random.Next((int)player.Attack + player.EquipWeapon.Value, ((int)player.Attack + player.EquipWeapon.Value) * 2);
                fRnd = fRnd / 100;
                reward = reward + (int)(reward * fRnd);

                // 소지했던 골드 출력 용 임시변수
                int originalGold = player.Gold;

                player.Gold += reward;

                player.ClearCount += 1;

                // 플레이어 레벨이 5보다 낮고 클리어 횟수가 레벨과 같다면 레벨업
                if (player.Level < 5 && player.ClearCount == player.Level)
                {
                    LevelUp();
                }

                DungeonClearScript(originalHealth, originalGold, difficultyStr);
            }
        }

        // originalHealth = 감소 전 체력 // originalGold = 보상 획득 전 골드 // difficultyStr = 난이도 안내 문자
        // 던전 클리어 스크립트 출력
        public void DungeonClearScript(int originalHealth, int originalGold, string difficultyStr)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine($"던전 클리어");
                Console.WriteLine($"축하합니다!!");
                Console.WriteLine($"{difficultyStr}던전 공략에 성공했습니다.");
                Console.WriteLine($"");
                Console.WriteLine($"[공략 결과]");
                Console.WriteLine($"체력 {originalHealth} => {player.Health}");
                Console.WriteLine($"골드 {originalGold} => {player.Gold}");
                scriptManager.SelectScript();

                if (int.TryParse(Console.ReadLine(), out int selectNumber))
                {
                    switch (selectNumber)
                    {
                        case 0:
                            scriptManager.JoinLobbyScript();
                            return;

                        default:
                            scriptManager.InvalidInputScript();
                            continue;
                    }
                }
                else
                {
                    scriptManager.InvalidInputScript();
                    continue;
                }
            }
        }

        // 레벨업
        static public void LevelUp()
        {
            player.Level += 1;

            player.ClearCount = 0;

            player.Attack = player.Attack + (0.5f * (player.Level - 1));
            player.Defence = player.Defence + (1 + (player.Level - 1));
        }
    }
}
