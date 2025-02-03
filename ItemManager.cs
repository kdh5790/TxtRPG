using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxtRPG
{
    public class ItemManager : Program
    {
        private ScriptManager scriptManager = new ScriptManager();
        private const float sellPrice = 0.85f;
        
        // 인벤토리
        public void Inventory()
        {
            Console.Clear();
            Console.WriteLine($"인벤토리");
            Console.WriteLine($"보유 중인 장비를 관리 할 수 있습니다.");
            Console.WriteLine($"");
            Console.WriteLine($"[아이템 목록]");

            int selectNumber = 0;

            while (true)
            {
                // 보유 중인 장비 저장용 리스트
                List<Item> tempItemList = new List<Item>();

                // 보유 중인 장비 표시 및 리스트에 추가
                foreach (var i in items)
                {
                    if (i.IsBuy)
                    {
                        selectNumber++;

                        if (i.Type == ItemType.Weapon)
                        {
                            if (i.Name == player.EquipWeapon.Name)
                                Console.WriteLine($"- ({selectNumber})[E]{i.Name} | 공격력 +{i.Value,2}  |  {i.Info} ");
                            else
                                Console.WriteLine($"- ({selectNumber})   {i.Name} | 공격력 +{i.Value,2}  |  {i.Info} ");

                        }
                        else if (i.Type == ItemType.Armor)
                        {
                            if (i.Name == player.EquipArmor.Name)
                                Console.WriteLine($"- ({selectNumber})[E]{i.Name} | 방어력 +{i.Value,2}  |  {i.Info} ");
                            else
                                Console.WriteLine($"- ({selectNumber})   {i.Name} | 방어력 +{i.Value,2}  |  {i.Info} ");
                        }
                        tempItemList.Add(i);
                    }
                }

                // 보유중인 장비가 없다면 로비로 이동
                if (selectNumber == 0)
                {
                    Console.WriteLine($"");

                    Console.WriteLine($"");
                    Console.WriteLine($"보유 중인 장비가 없습니다.");
                    Console.WriteLine($"");
                    scriptManager.JoinLobbyScript();
                }

                Console.WriteLine($"");
                Console.WriteLine($"장착 할 장비를 선택해주세요.");
                Console.WriteLine($"현재 장착 중인 장비를 선택 시 장비를 해제합니다.");
                scriptManager.SelectScript();

                selectNumber = 0;

                if (int.TryParse(Console.ReadLine(), out selectNumber))
                {
                    if (selectNumber == 0)
                        MainLobby();

                    else if (selectNumber > tempItemList.Count)
                    {
                        scriptManager.InvalidInputScript();
                        continue;
                    }

                    else
                    {
                        // 선택한 장비에 해당하는 인덱스 찾기
                        int index = items.FindIndex(i => i.Name == tempItemList[selectNumber - 1].Name);

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
                        scriptManager.JoinLobbyScript();
                        return;
                    }
                }
                else
                {
                    scriptManager.InvalidInputScript();
                    continue;
                }
            }
        }

        // 상점 메인
        public void Shop()
        {
            while (true)
            {
                scriptManager.ShopScript(SelectShopType.Main);

                Console.WriteLine($"");
                Console.WriteLine($"1. 장비 구매");
                Console.WriteLine($"2. 장비 판매");
                scriptManager.SelectScript();

                if (int.TryParse(Console.ReadLine(), out int selectNumber))
                {
                    switch (selectNumber)
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

        // 장비 구매
        public void BuyItem()
        {
            while (true)
            {
                scriptManager.ShopScript(SelectShopType.Buy);

                Console.WriteLine($"");
                Console.WriteLine($"구매하실 장비의 번호를 입력해주세요.");
                scriptManager.SelectScript();

                if (int.TryParse(Console.ReadLine(), out int selectNumber))
                {
                    if (selectNumber == 0)
                        MainLobby();

                    else if (selectNumber > items.Count)
                    {
                        scriptManager.InvalidInputScript();
                        continue;
                    }

                    if (items[selectNumber - 1].IsBuy)
                    {
                        Console.WriteLine($"이미 구매한 아이템입니다. 다시 입력해주세요.");
                        Thread.Sleep(1000);
                        continue;
                    }

                    if (items[selectNumber - 1].Price <= player.Gold)
                    {
                        Console.Clear();

                        items[selectNumber - 1].IsBuy = true;
                        player.Gold -= items[selectNumber - 1].Price;

                        Console.WriteLine($"");
                        Console.WriteLine($"");
                        Console.WriteLine($"장비 {items[selectNumber - 1].Name}을(를) 구매하였습니다.");
                        scriptManager.JoinLobbyScript();
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"");
                        Console.WriteLine($"골드가 부족합니다.");
                        scriptManager.JoinLobbyScript();
                        return;
                    }
                }

                else
                {
                    scriptManager.InvalidInputScript();
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
                scriptManager.ShopScript(SelectShopType.Sell);

                Console.WriteLine($"");
                Console.WriteLine($"");
                Console.WriteLine($"장비 판매 시 구매 가격의 85% 가격으로 판매하게 됩니다.");
                Console.WriteLine($"");
                Console.WriteLine($"판매하실 장비의 번호를 입력해주세요.");
                scriptManager.SelectScript();

                if (int.TryParse(Console.ReadLine(), out int selectNumber))
                {
                    if (selectNumber == 0)
                        MainLobby();

                    else if (selectNumber > tempItemList.Count)
                    {
                        scriptManager.InvalidInputScript();
                        continue;
                    }
                    else
                    {
                        // 판매 전 플레이어 골드
                        int temp = player.Gold;

                        // 판매하려는 장비의 이름과 일치하는 인덱스 찾기
                        int index = items.FindIndex(i => i.Name == tempItemList[selectNumber - 1].Name);

                        // 플레이어 및 아이템 보유 상태 변경
                        player.Gold += (int)(items[index].Price * sellPrice);
                        items[index].IsBuy = false;

                        Console.Clear();

                        Console.WriteLine($"장비 {items[index].Name}을/를 {(int)(items[index].Price * sellPrice)}G에 판매하였습니다.");
                        Console.WriteLine($"[소지 골드]");
                        Console.WriteLine($"{temp}G => {player.Gold}G");
                        Console.WriteLine($"");
                        scriptManager.JoinLobbyScript();
                        return;
                    }
                }

                else
                {
                    scriptManager.InvalidInputScript();
                    continue;
                }
            }
        }
    }
}
