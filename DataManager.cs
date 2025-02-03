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
        private ScriptManager scriptManager = new ScriptManager();

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

            string playerDataString = JsonConvert.SerializeObject(characterData);
            File.WriteAllText(filePath, playerDataString);

            // 아이템 보유 정보 저장
            Dictionary<string, bool> itemsDict = new Dictionary<string, bool>();

            for (int i = 0; i < items.Count; i++)
            {
                itemsDict.Add(items[i].Name, items[i].IsBuy);
            }
            string ItemDataString = JsonConvert.SerializeObject(itemsDict);
            File.WriteAllText(itemFilePath, ItemDataString);
        }

        // 세이브 파일 불러오기
        public Character LoadData()
        {
            Character loadCharacterData = new Character();

            while (true)
            {
                if (File.Exists("./Save/Data.json"))
                {
                    Console.Clear();
                    Console.WriteLine("저장 데이터가 존재합니다. 불러오시겠습니까?");
                    Console.WriteLine();
                    Console.WriteLine("1. 불러오기");
                    Console.WriteLine("2. 처음부터 시작");
                    Console.Write(">> ");

                    if (int.TryParse(Console.ReadLine(), out int selectNumber))
                    {
                        switch (selectNumber)
                        {
                            case 1:
                                loadCharacterData = DataParsing(loadCharacterData);
                                return loadCharacterData;
                            case 2:
                                Start();
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

                else Start();
            }
        }

        // 불러온 데이터 파싱 후 게임 내 적용
        public Character DataParsing(Character loadCharacterData)
        {
            string data = File.ReadAllText(filePath);
            JObject playerData = JObject.Parse(data);

            loadCharacterData.Name = playerData["Name"].ToString();
            loadCharacterData.Job = playerData["Job"].ToString();
            loadCharacterData.Level = int.Parse(playerData["Level"].ToString());
            loadCharacterData.Attack = float.Parse(playerData["Attack"].ToString());
            loadCharacterData.Defence = int.Parse(playerData["Defence"].ToString());
            loadCharacterData.Health = int.Parse(playerData["Health"].ToString());
            loadCharacterData.Gold = int.Parse(playerData["Gold"].ToString());
            loadCharacterData.ClearCount = int.Parse(playerData["ClearCount"].ToString());

            string weaponName = playerData["EquipWeapon"].ToString();
            string armorName = playerData["EquipArmor"].ToString();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name == weaponName)
                {
                    loadCharacterData.EquipWeapon = items[i];
                }
                else if (items[i].Name == armorName)
                {
                    loadCharacterData.EquipArmor = items[i];
                }
            }

            data = File.ReadAllText(itemFilePath);

            JObject itemData = JObject.Parse(data);

            Dictionary<string, bool> itemDic = itemData.ToObject<Dictionary<string, bool>>();

            for (int i = 0; i < itemDic.Count; i++)
            {
                items[i].IsBuy = itemDic[items[i].Name];
            }

            return loadCharacterData;
        }
    }
}
