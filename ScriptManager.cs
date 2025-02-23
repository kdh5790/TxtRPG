﻿using System;
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
            Thread.Sleep(delay);
        }

        public void JoinLobbyScript()
        {
            Console.WriteLine($"잠시 후 로비로 이동합니다.");
            Thread.Sleep(delay);
        }

        public void SelectScript()
        {
            Console.WriteLine($"0. 나가기");
            Console.WriteLine($"");
            Console.WriteLine($"원하시는 행동을 입력해주세요");
            Console.Write($">> ");
        }

        public void ShopScript(SelectShopType shopType)
        {
            Console.Clear();

            // 현재 입장한 상점 상태에 따라 텍스트 출력
            switch (shopType)
            {
                case SelectShopType.Main:
                    Console.WriteLine($"상점");
                    Console.WriteLine($"던전에서 사용 가능한 장비를 구매하거나 판매 할 수 있는 상점입니다.");
                    Console.WriteLine($"");
                    Console.WriteLine($"[보유 골드]");
                    Console.WriteLine($"{player.Gold} Gold");
                    Console.WriteLine($"");
                    Console.WriteLine($"[판매 장비 목록]");
                    break;
                case SelectShopType.Buy:
                    Console.WriteLine($"장비 구매");
                    Console.WriteLine($"던전에서 사용 가능한 장비를 구매 할 수 있습니다.");
                    Console.WriteLine($"");
                    Console.WriteLine($"[보유 골드]");
                    Console.WriteLine($"{player.Gold} Gold");
                    Console.WriteLine($"");
                    Console.WriteLine($"[판매 장비 목록]");
                    break;
                case SelectShopType.Sell:
                    Console.WriteLine($"장비 판매");
                    Console.WriteLine($"현재 보유하고 있는 장비를 판매 할 수 있습니다.");
                    Console.WriteLine($"");
                    Console.WriteLine($"[보유 골드]");
                    Console.WriteLine($"{player.Gold} Gold");
                    Console.WriteLine($"");
                    Console.WriteLine($"[현재 보유 중인 장비 목록]");
                    break;
            }

            int selectNumber = 0;

            // 상점 타입이 메인 or 구매 일 때
            if (shopType == SelectShopType.Main || shopType == SelectShopType.Buy)
            {
                foreach (var item in items)
                {
                    selectNumber++;
                    // 무기 스크립트 출력 + 보유중이라면 가격 대신 보유중 텍스트 표시
                    if (item.Type == ItemType.Weapon)
                    {
                        Console.Write($"- ({selectNumber}){item.Name} | 공격력 +{item.Value,2}  |  {item.Info} |");
                        if (!item.IsBuy)
                            Console.WriteLine($" {item.Price,5} G");
                        else
                            Console.WriteLine($" 보유 중");
                    }
                    else
                    // 방어구 스크립트 출력 + 보유중이라면 가격 대신 보유중 텍스트 표시
                    {
                        Console.Write($"- ({selectNumber}){item.Name} | 방어력 +{item.Value,2}  |  {item.Info}  |");
                        if (!item.IsBuy)
                            Console.WriteLine($" {item.Price,5} G");
                        else
                            Console.WriteLine($" 보유 중");
                    }
                    Console.WriteLine();
                }
            }
            // 상점 타입이 판매 일 때
            else
            {
                foreach (var item in items)
                {
                    // 보유 중인 장비 스크립트 출력
                    if (item.IsBuy)
                    {
                        selectNumber++;
                        Console.WriteLine($"- ({selectNumber}){item.Name} | 공격력 +{item.Value,2}  |  {item.Info} | {item.Price,5} G");
                    }
                }

                // 보유 중인 장비가 없을 때
                if (selectNumber == 0)
                {
                    Console.WriteLine($"");
                    Console.WriteLine($"현재 보유 중인 장비가 없습니다.");
                    Console.WriteLine($"");
                    JoinLobbyScript();
                    return;
                }

            }
        }
    }
}
