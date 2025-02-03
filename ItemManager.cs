using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxtRPG
{
    public class ItemManager : Program
    {
        public void Inventory()
        {
            Console.Clear();

            Console.WriteLine($"인벤토리");
            Console.WriteLine($"보유 중인 장비를 관리 할 수 있습니다.");
            Console.WriteLine($"");
            Console.WriteLine($"[아이템 목록]");

            int num = 0;

            while (true)
            {
                // 보유 중인 장비 저장용 리스트
                List<Item> tempItemList = new List<Item>();

                // 보유 중인 장비 표시 및 리스트에 추가
                foreach (var i in items)
                {
                    if (i.IsBuy)
                    {
                        num++;

                        if (i.Type == ItemType.Weapon)
                        {
                            if (i.Name == player.EquipWeapon.Name)
                                Console.WriteLine($"- ({num})[E]{i.Name} | 공격력 +{i.Value,2}  |  {i.Info} ");
                            else
                                Console.WriteLine($"- ({num})   {i.Name} | 공격력 +{i.Value,2}  |  {i.Info} ");

                        }
                        else if (i.Type == ItemType.Armor)
                        {
                            if (i.Name == player.EquipArmor.Name)
                                Console.WriteLine($"- ({num})[E]{i.Name} | 방어력 +{i.Value,2}  |  {i.Info} ");
                            else
                                Console.WriteLine($"- ({num})   {i.Name} | 방어력 +{i.Value,2}  |  {i.Info} ");
                        }
                        tempItemList.Add(i);
                    }
                }

                // 보유중인 장비가 없다면 로비로 이동
                if (num == 0)
                {
                    Console.WriteLine($"");

                    Console.WriteLine($"");
                    Console.WriteLine($"보유 중인 장비가 없습니다.");
                    Console.WriteLine($"");
                    Console.WriteLine($"잠시 후 로비로 돌아갑니다.");

                    Thread.Sleep(1000);
                    MainLobby();
                }

                Console.WriteLine($"");
                Console.WriteLine($"장착 할 장비를 선택해주세요.");
                Console.WriteLine($"현재 장착 중인 장비를 선택 시 장비를 해제합니다.");
                Console.WriteLine($"0. 나가기");
                Console.WriteLine($"");
                Console.WriteLine($"원하시는 행동을 입력해주세요");
                Console.Write($">> ");

                num = 0;

                if (int.TryParse(Console.ReadLine(), out num))
                {

                    if (num == 0)
                        MainLobby();

                    else if (num > tempItemList.Count)
                    {
                        Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                        Thread.Sleep(1000);
                        continue;
                    }

                    else
                    {
                        // 선택한 장비에 해당하는 인덱스 찾기
                        int index = items.FindIndex(i => i.Name == tempItemList[num - 1].Name);

                        // 장비 타입 확인 후 장착 및 해제 실행
                        switch (items[index].Type)
                        {
                            case ItemType.Weapon:
                                if (player.EquipWeapon.Name == items[index].Name)
                                {
                                    player.EquipWeapon = new Item();
                                }
                                else
                                {
                                    player.EquipWeapon = new Item(items[index]);
                                }
                                break;
                            case ItemType.Armor:
                                if (player.EquipArmor.Name == items[index].Name)
                                {
                                    player.EquipArmor = new Item();
                                }
                                else
                                {
                                    player.EquipArmor = new Item(items[index]);
                                }
                                break;
                        }

                        Console.Clear();
                        Console.WriteLine($"");
                        Console.WriteLine($"장비 장착 및 해제를 완료했습니다.");
                        Console.WriteLine($"");
                        Console.WriteLine($"잠시 후 로비로 이동합니다.");
                        Console.WriteLine($"");

                        Thread.Sleep(1000);
                        MainLobby();
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Thread.Sleep(1000);
                    continue;
                }
            }
        }

        // 상점 메인
        public void Shop()
        {
            while (true)
            {
                ShopScript(SelectShopType.Main);

                Console.WriteLine($"");
                Console.WriteLine($"1. 장비 구매");
                Console.WriteLine($"2. 장비 판매");
                Console.WriteLine($"0. 나가기");
                Console.WriteLine($"");
                Console.WriteLine($"원하시는 행동을 입력해주세요");
                Console.Write($">> ");

                if (int.TryParse(Console.ReadLine(), out int num))
                {
                    switch (num)
                    {
                        case 0:
                            return;

                        case 1:
                            BuyItem();
                            break;

                        case 2:
                            SellItem();
                            break;

                        default:
                            Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                            Thread.Sleep(1000);
                            continue;
                    }
                }

                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Thread.Sleep(1000);
                    continue;
                }
            }
        }


        // 장비 구매
        public void BuyItem()
        {
            while (true)
            {
                ShopScript(SelectShopType.Buy);

                Console.WriteLine($"");
                Console.WriteLine($"구매하실 장비의 번호를 입력해주세요.");
                Console.WriteLine($"0. 나가기");
                Console.WriteLine($"");
                Console.WriteLine($"원하시는 행동을 입력해주세요.");
                Console.Write($">> ");

                if (int.TryParse(Console.ReadLine(), out int num))
                {
                    if (num == 0)
                        MainLobby();

                    else if (num > items.Count)
                    {
                        Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                        Thread.Sleep(1000);
                        continue;
                    }


                    if (items[num - 1].IsBuy)
                    {
                        Console.WriteLine($"이미 구매한 아이템입니다. 다시 입력해주세요.");
                        Thread.Sleep(1000);
                        continue;
                    }


                    if (items[num - 1].Price <= player.Gold)
                    {
                        Console.Clear();

                        items[num - 1].IsBuy = true;
                        player.Gold -= items[num - 1].Price;

                        Console.WriteLine($"");
                        Console.WriteLine($"");
                        Console.WriteLine($"장비 {items[num - 1].Name}을(를) 구매하였습니다.");
                        Console.WriteLine($"잠시 후 로비로 돌아갑니다.");
                        Thread.Sleep(2000);
                        MainLobby();
                    }
                    else
                    {
                        Console.WriteLine($"");
                        Console.WriteLine($"골드가 부족합니다.");
                        Console.WriteLine($"잠시 후 로비로 돌아갑니다.");
                        Thread.Sleep(2000);
                        MainLobby();
                    }
                }

                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Thread.Sleep(1000);
                    continue;
                }
            }
        }

        // 장비 판매
        public void SellItem()
        {

            // 판매 가능한 장비 저장 리스트
            List<Item> tempItemList = new List<Item>();


            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].IsBuy)
                {
                    tempItemList.Add(items[i]);
                }
            }

            while (true)
            {
                ShopScript(SelectShopType.Sell);

                Console.WriteLine($"");
                Console.WriteLine($"");
                Console.WriteLine($"");
                Console.WriteLine($"장비 판매 시 구매 가격의 85% 가격으로 판매하게 됩니다.");
                Console.WriteLine($"");
                Console.WriteLine($"판매하실 장비의 번호를 입력해주세요.");
                Console.WriteLine($"0. 나가기");
                Console.WriteLine($"");
                Console.WriteLine($"원하시는 행동을 입력해주세요.");
                Console.Write($">> ");

                if (int.TryParse(Console.ReadLine(), out int num))
                {
                    if (num == 0)
                        MainLobby();

                    else if (num > tempItemList.Count)
                    {
                        Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요. fdsafdas");
                        Thread.Sleep(1000);
                        continue;
                    }
                    else
                    {
                        // 판매 전 플레이어 골드
                        int temp = player.Gold;

                        // 판매하려는 장비의 이름과 일치하는 인덱스 찾기
                        int index = items.FindIndex(i => i.Name == tempItemList[num - 1].Name);

                        // 플레이어 및 아이템 보유 상태 변경
                        player.Gold += (int)(items[index].Price * 0.85f);
                        items[index].IsBuy = false;

                        Console.Clear();

                        Console.WriteLine($"장비 {items[index].Name}을/를 {(int)(items[index].Price * 0.85f)}G에 판매하였습니다.");
                        Console.WriteLine($"[소지 골드]");
                        Console.WriteLine($"{temp}G => {player.Gold}G");
                        Console.WriteLine($"");
                        Console.WriteLine($"잠시 후 로비로 이동합니다.");

                        Thread.Sleep(2500);

                        MainLobby();
                    }
                }

                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Thread.Sleep(1000);
                    continue;
                }
            }
        }

        // 선택한 번호에 따라 상점 스크립트 출력 (메인, 구매, 판매)
        public void ShopScript(SelectShopType type)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            switch (type)
            {
                case SelectShopType.Main:
                    Console.WriteLine($"상점");
                    Console.ResetColor();
                    Console.WriteLine($"던전에서 사용 가능한 장비를 구매하거나 판매 할 수 있는 상점입니다.");
                    Console.WriteLine($"");
                    Console.WriteLine($"[보유 골드]");
                    Console.WriteLine($"{player.Gold} Gold");
                    Console.WriteLine($"");
                    Console.WriteLine($"[판매 장비 목록]");
                    break;
                case SelectShopType.Buy:
                    Console.WriteLine($"장비 구매");
                    Console.ResetColor();
                    Console.WriteLine($"던전에서 사용 가능한 장비를 구매 할 수 있습니다.");
                    Console.WriteLine($"");
                    Console.WriteLine($"[보유 골드]");
                    Console.WriteLine($"{player.Gold} Gold");
                    Console.WriteLine($"");
                    Console.WriteLine($"[판매 장비 목록]");
                    break;
                case SelectShopType.Sell:
                    Console.WriteLine($"장비 판매");
                    Console.ResetColor();
                    Console.WriteLine($"현재 보유하고 있는 장비를 판매 할 수 있습니다.");
                    Console.WriteLine($"");
                    Console.WriteLine($"[보유 골드]");
                    Console.WriteLine($"{player.Gold} Gold");
                    Console.WriteLine($"");
                    Console.WriteLine($"[현재 보유 중인 장비 목록]");
                    break;
            }


            int num = 0;

            if (type == SelectShopType.Main || type == SelectShopType.Buy)
            {
                foreach (var i in items)
                {
                    num++;
                    if (i.Type == ItemType.Weapon)
                    {
                        Console.Write($"- ({num}){i.Name} | 공격력 +{i.Value,2}  |  {i.Info} |");
                        if (!i.IsBuy)
                            Console.WriteLine($" {i.Price,5} G");
                        else
                            Console.WriteLine($" 보유 중");

                    }
                    else
                    {
                        Console.Write($"- ({num}){i.Name} | 방어력 +{i.Value,2}  |  {i.Info}  |");
                        if (!i.IsBuy)
                            Console.WriteLine($" {i.Price,5} G");
                        else
                            Console.WriteLine($" 보유 중");
                    }
                    Console.WriteLine();
                }
            }
            else
            {

                foreach (var i in items)
                {
                    if (i.IsBuy)
                    {
                        num++;
                        Console.WriteLine($"- ({num}){i.Name} | 공격력 +{i.Value,2}  |  {i.Info} | {i.Price,5} G");
                    }
                }

                if (num == 0)
                {
                    Console.WriteLine($"");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"현재 보유 중인 장비가 없습니다.");
                    Console.WriteLine($"");
                    Console.WriteLine($"");
                    Console.WriteLine($"잠시 후 상점 메인으로 돌아갑니다.");
                    Console.ResetColor();
                    Thread.Sleep(2000);
                    Shop();
                }

            }
        }
    }
}
