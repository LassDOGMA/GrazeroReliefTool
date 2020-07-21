using System.Windows.Controls;
using GrazeroReliefTool.Helpers;

namespace GrazeroReliefTool.Scripts
{
    class ButtonAction
    {
        public static void ChangeMulti(ListView target, TextBlock text)
        {
            MainWindow.logger.Info("画面テキストを切り替えます。");
            // TextBlockを切り替え
            text.Text = target.SelectedValue.ToString();
            MainWindow.logger.Debug(target.SelectedValue.ToString());

        }

        public static void SetListView(Button target, ListView multiList)
        {
            MainWindow.logger.Info("救援対象リストを変更します。");
            multiList.Items.Clear();


            MainWindow.logger.Debug(target.Content.ToString() + "が選ばれました。");
            switch (target.Content.ToString())
            {
                case "マグナ":
                    SetMagna(multiList);
                    break;
                case "マグナⅡ":
                    SetMagna2(multiList);
                    break;
                case "天司":
                    SetTensi(multiList);
                    break;
                case "旧石":
                    SetOldStone(multiList);
                    break;
                case "新石":
                    SetNewStone(multiList);
                    break;
                case "高級鞄":
                    SetKaban(multiList);
                    break;
                case "その他":
                    SetOther(multiList);
                    break;
            }
        }

        private static void SetMagna(ListView multiList)
        {
            multiList.Items.Add(EnemyType.TiamatOmega);
            multiList.Items.Add(EnemyType.ColossusOmega);
            multiList.Items.Add(EnemyType.LeviathanOmega);
            multiList.Items.Add(EnemyType.YggdrasilOmega);
            multiList.Items.Add(EnemyType.LumminieraOmega);
            multiList.Items.Add(EnemyType.CelesteOmega);
            multiList.Items.Add(EnemyType.TiamatOmegaImpossible);
            multiList.Items.Add(EnemyType.ColossusOmegaImpossible);
            multiList.Items.Add(EnemyType.LeviathanOmegaImpossible);
            multiList.Items.Add(EnemyType.YggdrasilOmegaImpossible);
            multiList.Items.Add(EnemyType.LumminieraOmegaImpossible);
            multiList.Items.Add(EnemyType.CelesteOmegaImpossible);
        }
        private static void SetMagna2(ListView multiList)
        {
            multiList.Items.Add(EnemyType.Shiva);
            multiList.Items.Add(EnemyType.Europa);
            multiList.Items.Add(EnemyType.GodswornAlexiel);
            multiList.Items.Add(EnemyType.Grimnir);
            multiList.Items.Add(EnemyType.Metatron);
            multiList.Items.Add(EnemyType.Avatar);
        }
        private static void SetTensi(ListView multiList)
        {
            multiList.Items.Add(EnemyType.Michael);
            multiList.Items.Add(EnemyType.Gabriel);
            multiList.Items.Add(EnemyType.Uriel);
            multiList.Items.Add(EnemyType.Raphael);
        }
        private static void SetOldStone(ListView multiList)
        {
            multiList.Items.Add(EnemyType.TwinElements);
            multiList.Items.Add(EnemyType.MaculaMarius);
            multiList.Items.Add(EnemyType.Medusa);
            multiList.Items.Add(EnemyType.Nezha);
            multiList.Items.Add(EnemyType.Apollo);
            multiList.Items.Add(EnemyType.TwinElementsImpossible);
            multiList.Items.Add(EnemyType.MaculaMariusImpossible);
            multiList.Items.Add(EnemyType.MedusaImpossible);
            multiList.Items.Add(EnemyType.NezhaImpossible);
            multiList.Items.Add(EnemyType.ApolloImpossible);
            multiList.Items.Add(EnemyType.DarkAngelOliviaImpossible);
        }
        private static void SetNewStone(ListView multiList)
        {
            multiList.Items.Add(EnemyType.Athena);
            multiList.Items.Add(EnemyType.Grani);
            multiList.Items.Add(EnemyType.Baal);
            multiList.Items.Add(EnemyType.Garuda);
            multiList.Items.Add(EnemyType.Odin);
            multiList.Items.Add(EnemyType.Lich);
        }
        private static void SetKaban(ListView multiList)
        {
            multiList.Items.Add(EnemyType.Prometheus);
            multiList.Items.Add(EnemyType.CaOng);
            multiList.Items.Add(EnemyType.Gilgamesh);
            multiList.Items.Add(EnemyType.Morrigna);
            multiList.Items.Add(EnemyType.Hector);
            multiList.Items.Add(EnemyType.Anubis);
        }
        private static void SetOther(ListView multiList)
        {
            multiList.Items.Add(EnemyType.ProtoBahamut);
            multiList.Items.Add(EnemyType.GrandOrder);
            multiList.Items.Add(EnemyType.RoseQueen);
            multiList.Items.Add(EnemyType.UltimateBahamut);
            multiList.Items.Add(EnemyType.TiamatMalice);
            multiList.Items.Add(EnemyType.LeviathanMalice);
            multiList.Items.Add(EnemyType.Phronesis);
            multiList.Items.Add(EnemyType.ProtoBahamutImpossible);
            multiList.Items.Add(EnemyType.Lucilius);
            multiList.Items.Add(EnemyType.Akasha);
            multiList.Items.Add(EnemyType.UltimateBahamutImpossible);
            multiList.Items.Add(EnemyType.LisiliusImpossible);
            multiList.Items.Add(EnemyType.Huanglong);
            multiList.Items.Add(EnemyType.Qilin);
            multiList.Items.Add(EnemyType.HuanglongQilin);
            multiList.Items.Add(EnemyType.AllPrimarch);
        }
    }
}
