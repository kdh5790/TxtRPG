using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;

namespace TxtRPG
{
    public class Program
    {
        static private DataManager dataManager = new DataManager();
        static private DungeonManager dungeonManager = new DungeonManager();
        static private ItemManager itemManager = new ItemManager();
        static private ScriptManager scriptManager = new ScriptManager();

        public enum ItemType
        {
            Weapon,
            Armor
        }

        public enum SelectShopType
        {
            Main,
            Buy,
            Sell
        }

        public enum Difficulty
        {
            Easy = 5,
            Normal = 7,
            Hard = 11
        }

        public class Character
        {
            private string name = "스파르타";
            private string job = "전사";
            private int level = 1;
            private float attack = 10;
            private int defence = 5;
            private int health = 100;
            private int gold = 1500;
            private int clearCount = 0;
            private Item equipWeapon = new Item();
            private Item equipArmor = new Item();

            public string Name { get { return name; } set { name = value; } }
            public string Job { get { return job; } set { job = value; } }
            public int Level { get { return level; } set { level = value; } }
            public float Attack { get { return attack; } set { attack = value; } }
            public int Defence { get { return defence; } set { defence = value; } }
            public int Health { get { return health; }  set { health = Math.Max(0, value); } }
            public int Gold { get { return gold; } set { gold = Math.Max(0, value); } }
            public int ClearCount { get { return clearCount; } set { clearCount = value; } }
            public Item EquipWeapon { get { return new Item(equipWeapon); } set { equipWeapon = new Item(value); } }
            public Item EquipArmor { get { return new Item(equipArmor); } set { equipArmor = new Item(value); } }
        }

        public class Item
        {
            public string Name { get; set; }
            public string Info { get; set; }
            public ItemType Type { get; set; }
            public int Value { get; set; }
            public int Price { get; set; }
            public bool IsBuy { get; set; }

            public Item()
            {
                Name = " ";
                Info = " ";
                Type = ItemType.Weapon;
                Value = 0;
                Price = 0;
                IsBuy = false;
            }

            // 일반 생성자
            public Item(string n, string i, ItemType t, int v, int p, bool b = false)
            {
                Name = n; Info = i; Type = t; Value = v; Price = p; IsBuy = b;
            }

            // 복사 생성자
            public Item(Item other)
            {
                Name = other.Name; Info = other.Info; Type = other.Type; Value = other.Value; Price = other.Price; IsBuy = other.IsBuy;
            }
        }

        static public List<Item> items = new List<Item>();
        static public Character player = new Character();

        static void Main()
        {
            dataManager.InitItem();

            player = dataManager.LoadData();

            MainLobby();
        }

        // 처음부터 시작(세이브 파일이 없거나 처음부터 시작을 선택 할 시)
        static public void Start()
        {
            Console.Clear();

            player = new Character();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.Write("캐릭터의 닉네임을 설정 해주세요.\n>> ");

            player.Name = Console.ReadLine();

            MainLobby();
        }

        // 메인 로비
        static public void MainLobby()
        {
            dataManager.SaveData(player);

            while (true)
            {
                Console.Clear();

                Console.WriteLine($"스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine($"이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine($"");
                Console.WriteLine($"1. 캐릭터 정보");
                Console.WriteLine($"2. 인벤토리");
                Console.WriteLine($"3. 상점");
                Console.WriteLine($"4. 던전 입장");
                Console.WriteLine($"5. 휴식하기");
                Console.WriteLine($"");
                Console.WriteLine($"원하시는 행동을 입력해주세요.");
                Console.Write($">> ");

                if (int.TryParse(Console.ReadLine(), out int selectNumber))
                {
                    switch (selectNumber)
                    {
                        case 1:
                            PlayerInfo();
                            break;
                        case 2:
                            itemManager.Inventory();
                            break;
                        case 3:
                            itemManager.Shop();
                            break;
                        case 4:
                            dungeonManager.DungeonLobby();
                            break;
                        case 5:
                            Rest();
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
        
        // 플레이어 정보 확인
        static public void PlayerInfo()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine($"닉네임 : {player.Name}");
                Console.WriteLine($"레벨 : {player.Level}");
                Console.WriteLine($"직업 : {player.Job}");

                // 장비 장착 중인지 확인 후 해당 스탯 적용
                if (player.EquipWeapon.Value == 0)
                {
                    Console.WriteLine($"공격력 : {player.Attack:F1}");
                }
                else
                {
                    Console.WriteLine($"공격력 : {player.Attack:F1} + {player.EquipWeapon.Value}(무기 공격력)");

                }

                if (player.EquipArmor.Value == 0)
                    Console.WriteLine($"방어력 : {player.Defence}");
                else
                    Console.WriteLine($"방어력 : {player.Defence} + {player.EquipArmor.Value}(방어구 방어력)");

                Console.WriteLine($"체력 : {player.Health}");
                Console.WriteLine($"소지 골드 : {player.Gold}");
                Console.WriteLine($"");
                Console.WriteLine($"0. 나가기");
                Console.WriteLine($"");
                Console.WriteLine($"원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                if (int.TryParse(Console.ReadLine(), out int selectNumber))
                {
                    switch (selectNumber)
                    {
                        case 0:
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

        // 휴식
        static public void Rest()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine($"휴식하기");
                Console.WriteLine($"500G를 소모해 체력을 회복 할 수 있습니다.");
                Console.WriteLine($"체력은 100을 초과 할 수 없습니다.");
                Console.WriteLine($"");
                Console.WriteLine($"보유 골드 : {player.Gold}G");
                Console.WriteLine($"");
                Console.WriteLine($"1. 휴식하기");
                Console.WriteLine($"0. 나가기");
                Console.WriteLine($"");
                Console.WriteLine($"원하시는 행동을 입력해주세요.");
                Console.Write($">> ");

                if (int.TryParse(Console.ReadLine(), out int selectNumber))
                {
                    switch (selectNumber)
                    {
                        case 0:
                            return;

                        case 1:
                            Console.Clear();
                            // 플레이어 골드가 충분하고 HP가 100보다 낮다면 휴식을 취하고 로비로 이동
                            if (player.Gold >= 500 && player.Health < 100)
                            {
                                int originalHealth = player.Health;
                                int originalGold = player.Gold;
                                player.Health = 100;
                                player.Gold -= 500;


                                Console.WriteLine($"충분한 휴식을 취했습니다.");
                                Console.WriteLine($"체력이 회복 됐습니다.");
                                Console.WriteLine($"HP : {originalHealth} => HP : {player.Health}");
                                Console.WriteLine($"");
                                Console.WriteLine($"Gold : {originalGold} => Gold : {player.Gold}");
                                Console.WriteLine($"");
                                scriptManager.JoinLobbyScript();
                                return;
                            }

                            // 플레이어 골드가 충분하고 HP가 100이거나 그보다 크다면 휴식을 취하지 않고 로비로 이동
                            else if (player.Gold >= 500 || player.Health >= 100)
                            {
                                Console.WriteLine($"이미 체력이 최대치로 회복되어 있습니다.");
                                Console.WriteLine($"휴식을 취하지 않습니다.");
                                scriptManager.JoinLobbyScript();
                                return;
                            }

                            // 플레이어 골드가 부족하다면 로비로 이동
                            else
                            {
                                Console.WriteLine($"골드가 부족합니다.");
                                Console.WriteLine($"보유 골드 : {player.Gold}G");
                                Console.WriteLine($"필요 골드 : 500G");
                                scriptManager.JoinLobbyScript();
                                return;
                            }

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
    }
}
