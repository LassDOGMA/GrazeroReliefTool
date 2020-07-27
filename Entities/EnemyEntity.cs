using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrazeroReliefTool.Entities
{
    class EnemyEntity
    {
        // Omega Raids
        public string TiamatOmega { get; set; }
        public DateTime TiamatOmegaTime { get; set; }
        public string ColossusOmega { get; set; }
        public DateTime ColossusOmegaTime { get; set; }
        public string LeviathanOmega { get; set; }
        public DateTime LeviathanOmegaTime { get; set; }
        public string YggdrasilOmega { get; set; }
        public DateTime YggdrasilOmegaTime { get; set; }
        public string LumminieraOmega { get; set; }
        public DateTime LumminieraOmegaTime { get; set; }
        public string CelesteOmega { get; set; }
        public DateTime CelesteOmegaTime { get; set; }
        // Tier 1 Summon Raids
        public string TwinElements { get; set; }
        public DateTime TwinElementsTime { get; set; }
        public string MaculaMarius { get; set; }
        public DateTime MaculaMariusTime { get; set; }
        public string Medusa { get; set; }
        public DateTime MedusaTime { get; set; }
        public string Nezha { get; set; }
        public DateTime NezhaTime { get; set; }
        public string Apollo { get; set; }
        public DateTime ApolloTime { get; set; }
        public string DarkAngelOlivia { get; set; }
        public DateTime DarkAngelOliviaTime { get; set; }
        // Tier 2 Summon Raids
        public string Athena { get; set; }
        public DateTime AthenaTime { get; set; }
        public string Grani { get; set; }
        public DateTime GraniTime { get; set; }
        public string Baal { get; set; }
        public DateTime BaalTime { get; set; }
        public string Garuda { get; set; }
        public DateTime GarudaTime { get; set; }
        public string Odin { get; set; }
        public DateTime OdinTime { get; set; }
        public string Lich { get; set; }
        public DateTime LichTime { get; set; }
        // Primarch Raids
        public string Michael { get; set; }
        public DateTime MichaelTime { get; set; }
        public string Gabriel { get; set; }
        public DateTime GabrielTime { get; set; }
        public string Uriel { get; set; }
        public DateTime UrielTime { get; set; }
        public string Raphael { get; set; }
        public DateTime RaphaelTime { get; set; }
        // Nightmare Raids
        public string ProtoBahamut { get; set; }
        public DateTime ProtoBahamutTime { get; set; }
        public string GrandOrder { get; set; }
        public DateTime GrandOrderTime { get; set; }
        public string Huanglong { get; set; }
        public DateTime HuanglongTime { get; set; }
        public string Qilin { get; set; }
        public DateTime QilinTime { get; set; }
        public string UltimateBahamut { get; set; }
        public DateTime UltimateBahamutTime { get; set; }
        // Story Raids
        public string RoseQueen { get; set; }
        public DateTime RoseQueenTime { get; set; }

        // Impossible Omega raids
        // Omega Raids
        public string TiamatOmegaImpossible { get; set; }
        public DateTime TiamatOmegaImpossibleTime { get; set; }
        public string ColossusOmegaImpossible { get; set; }
        public DateTime ColossusOmegaImpossibleTime { get; set; }
        public string LeviathanOmegaImpossible { get; set; }
        public DateTime LeviathanOmegaImpossibleTime { get; set; }
        public string YggdrasilOmegaImpossible { get; set; }
        public DateTime YggdrasilOmegaImpossibleTime { get; set; }
        public string LumminieraOmegaImpossible { get; set; }
        public DateTime LumminieraOmegaImpossibleTime { get; set; }
        public string CelesteOmegaImpossible { get; set; }
        public DateTime CelesteOmegaImpossibleTime { get; set; }
        // Omega II Raids
        public string Shiva { get; set; }
        public DateTime ShivaTime { get; set; }
        public string Europa { get; set; }
        public DateTime EuropaTime { get; set; }
        public string GodswornAlexiel { get; set; }
        public DateTime GodswornAlexielTime { get; set; }
        public string Grimnir { get; set; }
        public DateTime GrimnirTime { get; set; }
        public string Metatron { get; set; }
        public DateTime MetatronTime { get; set; }
        public string Avatar { get; set; }
        public DateTime AvatarTime { get; set; }
        // Tier 1 Summon Raids
        public string TwinElementsImpossible { get; set; }
        public DateTime TwinElementsImpossibleTime { get; set; }
        public string MaculaMariusImpossible { get; set; }
        public DateTime MaculaMariusImpossibleTime { get; set; }
        public string MedusaImpossible { get; set; }
        public DateTime MedusaImpossibleTime { get; set; }
        public string NezhaImpossible { get; set; }
        public DateTime NezhaImpossibleTime { get; set; }
        public string ApolloImpossible { get; set; }
        public DateTime ApolloImpossibleTime { get; set; }
        public string DarkAngelOliviaImpossible { get; set; }
        public DateTime DarkAngelOliviaImpossibleTime { get; set; }
        // Tier 3 Summon Raids
        public string Prometheus { get; set; }
        public DateTime PrometheusTime { get; set; }
        public string CaOng { get; set; }
        public DateTime CaOngTime { get; set; }
        public string Gilgamesh { get; set; }
        public DateTime GilgameshTime { get; set; }
        public string Morrigna { get; set; }
        public DateTime MorrignaTime { get; set; }
        public string Hector { get; set; }
        public DateTime HectorTime { get; set; }
        public string Anubis { get; set; }
        public DateTime AnubisTime { get; set; }
        // Impossible Malice
        public string TiamatMalice { get; set; }
        public DateTime TiamatMaliceTime { get; set; }
        public string LeviathanMalice { get; set; }
        public DateTime LeviathanMaliceTime { get; set; }
        public string Phronesis { get; set; }
        public DateTime PhronesisTime { get; set; }
        // Nightmare Raids
        public string ProtoBahamutImpossible { get; set; }
        public DateTime ProtoBahamutImpossibleTime { get; set; }
        public string Akasha { get; set; }
        public DateTime AkashaTime { get; set; }
        public string Lucilius { get; set; }
        public DateTime LuciliusTime { get; set; }
        public string GrandOrderImpossible { get; set; }
        public DateTime GrandOrderImpossibleTime { get; set; }
        // Ultimate Raids
        public string UltimateBahamutImpossible { get; set; }
        public DateTime UltimateBahamutImpossibleTime { get; set; }
        // Rapture Raids
        public string LuciliusHard { get; set; }
        public DateTime LuciliusHardTime { get; set; }
        public string Beelzebub { get; set; }
        public DateTime BeelzebubTime { get; set; }
        // Impossible Beasts
        public string HuanglongAndQilin { get; set; }
        public DateTime HuanglongAndQilinTime { get; set; }
        public string TheFourPrimarchs { get; set; }
        public DateTime TheFourPrimarchsTime { get; set; }
        // 六竜
        public string Wilnas { get; set; }
        public DateTime WilnasTime { get; set; }
        public string Wamdus { get; set; }
        public DateTime WamdusTime { get; set; }
        public string Galleon { get; set; }
        public DateTime GalleonTime { get; set; }
        public string Ewiyar { get; set; }
        public DateTime EwiyarTime { get; set; }
        public string LuWoh { get; set; }
        public DateTime LuWohTime { get; set; }
        public string Fediel { get; set; }
        public DateTime FedielTime { get; set; }
    }
}
