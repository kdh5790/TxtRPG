using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxtRPG
{
    public class DataManager : Program
    {
        // 세이브관련 파일 위치, 파일명
        public const string folderPath = "./Save"; // 세이브 파일 저장 폴더
        public const string filePath = "./Save/Data.json"; // 플레이어 정보 세이브 파일명
        public const string itemFilePath = "./Save/ItemData.json"; // 아이템 정보 세이브 파일명

        // 리스트에 장비 추가
        public void InitItem()
        {
            items.Add(new Item("낡은 검        ", "쉽게 볼 수 있는 낡은 검 입니다.                   ", ItemType.Weapon, 2, 600));
            items.Add(new Item("청동 도끼      ", "어디선가 사용됐던거 같은 도끼입니다.              ", ItemType.Weapon, 5, 1500));
            items.Add(new Item("스파르타의 창  ", "스파르타의 전사들이 사용했다는 전설의 창입니다.   ", ItemType.Weapon, 7, 2500));
            items.Add(new Item("튜터의 검      ", "스파르타의 교관들이 사용했던 검 입니다.           ", ItemType.Weapon, 15, 7000));
            items.Add(new Item("수련자 갑옷    ", "수련에 도움을 주는 갑옷입니다.                   ", ItemType.Armor, 5, 1000));
            items.Add(new Item("무쇠갑옷       ", "무쇠로 만들어져 튼튼한 갑옷입니다.               ", ItemType.Armor, 9, 2000));
            items.Add(new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", ItemType.Armor, 15, 3500));
            items.Add(new Item("매니저의 갑옷  ", "스파르타의 관리자들이 사용했던 갑옷입니다.       ", ItemType.Armor, 30, 10000));
        }

        // 현재 플레이어 데이터 저장
        public void SaveData(Character characterData)
        {
            DirectoryInfo folder = new DirectoryInfo(folderPath);

            if (!folder.Exists)
                folder.Create();

            // 플레이어 정보 저장
            JObject data = new JObject(
                new JProperty("Name", characterData.Name),
                new JProperty("Job", characterData.Job),
                new JProperty("Level", characterData.Level),
                new JProperty("Attack", characterData.Attack),
                new JProperty("Defence", characterData.Defence),
                new JProperty("Health", characterData.Health),
                new JProperty("Gold", characterData.Gold),
                new JProperty("ClearCount", characterData.ClearCount),
                new JProperty("EquipWeapon", characterData.EquipWeapon.Name),
                new JProperty("EquipArmor", characterData.EquipArmor.Name)
                );

            File.WriteAllText(filePath, data.ToString());

            // 아이템 보유 정보 저장
            Dictionary<string, bool> itemsDic = new Dictionary<string, bool>();

            for (int i = 0; i < items.Count; i++)
            {
                itemsDic.Add(items[i].Name, items[i].IsBuy);
            }

            string json = JsonConvert.SerializeObject(itemsDic);
            File.WriteAllText(itemFilePath, json);
        }

        // 세이브 파일 불러오기
        public Character LoadData()
        {
            Character load = new Character();

            while (true)
            {
                if (File.Exists("./Save/Data.json"))
                {
                    Console.WriteLine("저장 데이터가 존재합니다. 불러오시겠습니까?");
                    Console.WriteLine();
                    Console.WriteLine("1. 불러오기");
                    Console.WriteLine("2. 처음부터 시작");
                    Console.Write(">> ");

                    if (int.TryParse(Console.ReadLine(), out int num))
                    {
                        switch (num)
                        {
                            case 1:
                                load = DataParsing(load);
                                return load;
                            case 2:
                                Start();
                                break;
                            default:
                                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                                Thread.Sleep(1000);
                                Console.Clear();
                                continue;
                        }
                    }
                    
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        continue;
                    }
                }

                else Start();
            }
        }

        // 불러온 데이터 파싱 후 게임 내 적용
        public Character DataParsing(Character load)
        {
            string data = File.ReadAllText(filePath);
            JObject playerData = JObject.Parse(data);

            load.Name = playerData["Name"].ToString();
            load.Job = playerData["Job"].ToString();
            load.Level = int.Parse(playerData["Level"].ToString());
            load.Attack = int.Parse(playerData["Attack"].ToString());
            load.Defence = int.Parse(playerData["Defence"].ToString());
            load.Health = int.Parse(playerData["Health"].ToString());
            load.Gold = int.Parse(playerData["Gold"].ToString());
            load.ClearCount = int.Parse(playerData["ClearCount"].ToString());

            string weaponName = playerData["EquipWeapon"].ToString();
            string armorName = playerData["EquipArmor"].ToString();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name == weaponName)
                {
                    load.EquipWeapon = items[i];
                }
                else if (items[i].Name == armorName)
                {
                    load.EquipArmor = items[i];
                }
            }

            data = File.ReadAllText(itemFilePath);

            JObject itemData = JObject.Parse(data);

            Dictionary<string, bool> itemDic = itemData.ToObject<Dictionary<string, bool>>();

            for (int i = 0; i < itemDic.Count; i++)
            {
                items[i].IsBuy = itemDic[items[i].Name];
            }

            return load;
        }
    }
}
