using CoreTweet;
using System.Text.RegularExpressions;
using System;
using CoreTweet.Streaming;
using System.Linq;
using System.Windows.Controls;
using GrazeroReliefTool.Entities;
using GrazeroReliefTool.Helpers;
using System.Windows;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Threading;
using GrazeroReliefTool.Scripts;

namespace GrazeroReliefTool.Program
{
    class Program
    {
        // 定数
        private const string GRANBLUE_URL = " http://game.granbluefantasy.jp/#mypage";
        private const string RESULT_URL = "http://game.granbluefantasy.jp/#quest";
        private const string MULTI_URL = "http://game.granbluefantasy.jp/#quest/assist";
        private const string USER_DATA_DIR ="user-data-dir=";
        private const string DISABLE_INFOBARS = "disable-infobars";
        private const string TWITTER_LINK_URL = "https://t.co/";

        // 環境変数
        public static ChromeDriver driver = null;
        private static string participationId = "";
        public static EnemyEntity EnemyEntity = new EnemyEntity();

        // Tokenセット
        public static Tokens tokens;

        public static void StreamingFindParticipationId(TextBlock selectMulti, TextBlock selectTime)
        {
            MainWindow.logger.Info("Filter検索を開始します。");
            // Filter検索開始
            foreach (var status in tokens.Streaming.Filter(track: ":参戦ID,参加者募集！").OfType<StatusMessage>().Select(x => x.Status))
            {
                if (status.Text.Contains(TWITTER_LINK_URL))
                {
                    try
                    {
                        MainWindow.logger.Debug(status.Text);

                        // 参戦ID生成
                        string resText = Regex.Replace(status.Text, "(.*) :参戦ID.*", "$1", RegexOptions.Singleline);
                        // 無駄な文字列の削除
                        if (resText.Length > 8)
                        {
                            participationId = resText.Substring(resText.Length - 8);
                        }
                        else
                        {
                            participationId = resText;
                        }
                        MainWindow.logger.Debug("参戦ID：" + participationId);

                        // 敵名生成(最後から2行目を取得)
                        int lineCount = status.Text.Length - status.Text.Replace("\n", "").Length;
                        string[] del = { "\n" };

                        string enemy = status.Text.Split(del, StringSplitOptions.None)[lineCount - 1];
                        MainWindow.logger.Debug("敵名：" + enemy);

                        // Entityに敵名と参戦IDを設定
                        SetEntity(enemy, participationId);

                        // 画面に時間を表示
                        selectTime.Dispatcher.BeginInvoke(new Action(() => SetTime(selectMulti.Text.ToString(), selectTime)));

                        // オートモード入力
                        selectTime.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (selectMulti.Text.ToString().Equals(enemy))
                            {
                                selectTime.Dispatcher.BeginInvoke(new Action(() => MultiBattleText(selectMulti)));
                            }
                        }));
                    }
                    catch (Exception e)
                    {
                        MainWindow.logger.Error("非同期部例外発生");
                        MainWindow.logger.Error("取得テキスト：" + status.Text);
                        MainWindow.logger.Error(e.ToString());
                        MainWindow.logger.Error(e.StackTrace);
                        StreamingFindParticipationId(selectMulti, selectTime);
                    }
                }
                // 画像が無かった場合、最終行から2行目にEnemyNameが取れないので・・・
                else
                {
                    try
                    {
                        MainWindow.logger.Debug(status.Text);

                        // 参戦ID生成
                        string resText = Regex.Replace(status.Text, "(.*) :参戦ID.*", "$1", RegexOptions.Singleline);
                        // 無駄な文字列の削除
                        if (resText.Length > 8)
                        {
                            participationId = resText.Substring(resText.Length - 8);
                        }
                        else
                        {
                            participationId = resText;
                        }
                        MainWindow.logger.Debug("参戦ID：" + participationId);

                        // 敵名生成(最後から2行目を取得)
                        int lineCount = status.Text.Length - status.Text.Replace("\n", "").Length;
                        string[] del = { "\n" };

                        string enemy = status.Text.Split(del, StringSplitOptions.None)[lineCount];
                        MainWindow.logger.Debug("敵名：" + enemy);

                        // Entityに敵名と参戦IDを設定
                        SetEntity(enemy, participationId);

                        // 画面に時間を表示
                        selectTime.Dispatcher.BeginInvoke(new Action(() => SetTime(selectMulti.Text.ToString(), selectTime)));

                        // オートモード入力
                        selectTime.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (selectMulti.Text.ToString().Equals(enemy))
                            {
                                selectTime.Dispatcher.BeginInvoke(new Action(() => MultiBattleText(selectMulti)));
                            }
                        }));
                    }
                    catch (Exception e)
                    {
                        MainWindow.logger.Error("非同期部例外発生");
                        MainWindow.logger.Error("取得テキスト：" + status.Text);
                        MainWindow.logger.Error(e.ToString());
                        MainWindow.logger.Error(e.StackTrace);
                        StreamingFindParticipationId(selectMulti, selectTime);
                    }
                }
            }
        }

        public static void SetTime(string enemy, TextBlock textBlock)
        {
            DateTime entityTime = GetEntityTime(enemy);
            if (entityTime != DateTime.MinValue && DateTime.Now >= entityTime)
            {
                TimeSpan timeSpan = DateTime.Now - entityTime;
                textBlock.Text = "最新:" + Math.Ceiling(timeSpan.TotalSeconds) + "秒前";
            }
            else
            {
                textBlock.Text = "最新はありません";
            }
        }

        public static void SetEntity(string enemy, string participationId)
        {
            switch (enemy)
            {
                case EnemyType.TiamatOmega:
                    EnemyEntity.TiamatOmega = participationId;
                    EnemyEntity.TiamatOmegaTime = DateTime.Now;
                    break;
                case EnemyType.ColossusOmega:
                    EnemyEntity.ColossusOmega = participationId;
                    EnemyEntity.ColossusOmegaTime = DateTime.Now;
                    break;
                case EnemyType.LeviathanOmega:
                    EnemyEntity.LeviathanOmega = participationId;
                    EnemyEntity.LeviathanOmegaTime = DateTime.Now;
                    break;
                case EnemyType.YggdrasilOmega:
                    EnemyEntity.YggdrasilOmega = participationId;
                    EnemyEntity.YggdrasilOmegaTime = DateTime.Now;
                    break;
                case EnemyType.LumminieraOmega:
                    EnemyEntity.LumminieraOmega = participationId;
                    EnemyEntity.LumminieraOmegaTime = DateTime.Now;
                    break;
                case EnemyType.CelesteOmega:
                    EnemyEntity.CelesteOmega = participationId;
                    EnemyEntity.CelesteOmegaTime = DateTime.Now;
                    break;
                case EnemyType.TwinElements:
                    EnemyEntity.TwinElements = participationId;
                    EnemyEntity.TwinElementsTime = DateTime.Now;
                    break;
                case EnemyType.MaculaMarius:
                    EnemyEntity.MaculaMarius = participationId;
                    EnemyEntity.MaculaMariusTime = DateTime.Now;
                    break;
                case EnemyType.Medusa:
                    EnemyEntity.Medusa = participationId;
                    EnemyEntity.MedusaTime = DateTime.Now;
                    break;
                case EnemyType.Nezha:
                    EnemyEntity.Nezha = participationId;
                    EnemyEntity.NezhaTime = DateTime.Now;
                    break;
                case EnemyType.Apollo:
                    EnemyEntity.Apollo = participationId;
                    EnemyEntity.ApolloTime = DateTime.Now;
                    break;
                case EnemyType.DarkAngelOlivia:
                    EnemyEntity.DarkAngelOlivia = participationId;
                    EnemyEntity.DarkAngelOliviaTime = DateTime.Now;
                    break;
                case EnemyType.Athena:
                    EnemyEntity.Athena = participationId;
                    EnemyEntity.AthenaTime = DateTime.Now;
                    break;
                case EnemyType.Grani:
                    EnemyEntity.Grani = participationId;
                    EnemyEntity.GraniTime = DateTime.Now;
                    break;
                case EnemyType.Baal:
                    EnemyEntity.Baal = participationId;
                    EnemyEntity.BaalTime = DateTime.Now;
                    break;
                case EnemyType.Garuda:
                    EnemyEntity.Garuda = participationId;
                    EnemyEntity.GarudaTime = DateTime.Now;
                    break;
                case EnemyType.Odin:
                    EnemyEntity.Odin = participationId;
                    EnemyEntity.OdinTime = DateTime.Now;
                    break;
                case EnemyType.Lich:
                    EnemyEntity.Lich = participationId;
                    EnemyEntity.LichTime = DateTime.Now;
                    break;
                case EnemyType.Michael:
                    EnemyEntity.Michael = participationId;
                    EnemyEntity.MichaelTime = DateTime.Now;
                    break;
                case EnemyType.Gabriel:
                    EnemyEntity.Gabriel = participationId;
                    EnemyEntity.GabrielTime = DateTime.Now;
                    break;
                case EnemyType.Uriel:
                    EnemyEntity.Uriel = participationId;
                    EnemyEntity.UrielTime = DateTime.Now;
                    break;
                case EnemyType.Raphael:
                    EnemyEntity.Raphael = participationId;
                    EnemyEntity.RaphaelTime = DateTime.Now;
                    break;
                case EnemyType.ProtoBahamut:
                    EnemyEntity.ProtoBahamut = participationId;
                    EnemyEntity.ProtoBahamutTime = DateTime.Now;
                    break;
                case EnemyType.GrandOrder:
                    EnemyEntity.GrandOrder = participationId;
                    EnemyEntity.GrandOrderTime = DateTime.Now;
                    break;
                case EnemyType.Huanglong:
                    EnemyEntity.Huanglong = participationId;
                    EnemyEntity.HuanglongTime = DateTime.Now;
                    break;
                case EnemyType.Qilin:
                    EnemyEntity.Qilin = participationId;
                    EnemyEntity.QilinTime = DateTime.Now;
                    break;
                case EnemyType.UltimateBahamut:
                    EnemyEntity.UltimateBahamut = participationId;
                    EnemyEntity.UltimateBahamutTime = DateTime.Now;
                    break;
                case EnemyType.RoseQueen:
                    EnemyEntity.RoseQueen = participationId;
                    EnemyEntity.RoseQueenTime = DateTime.Now;
                    break;
                case EnemyType.TiamatOmegaImpossible:
                    EnemyEntity.TiamatOmegaImpossible = participationId;
                    EnemyEntity.TiamatOmegaImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.ColossusOmegaImpossible:
                    EnemyEntity.ColossusOmegaImpossible = participationId;
                    EnemyEntity.ColossusOmegaImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.LeviathanOmegaImpossible:
                    EnemyEntity.LeviathanOmegaImpossible = participationId;
                    EnemyEntity.LeviathanOmegaImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.YggdrasilOmegaImpossible:
                    EnemyEntity.YggdrasilOmegaImpossible = participationId;
                    EnemyEntity.YggdrasilOmegaImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.LumminieraOmegaImpossible:
                    EnemyEntity.LumminieraOmegaImpossible = participationId;
                    EnemyEntity.LumminieraOmegaImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.CelesteOmegaImpossible:
                    EnemyEntity.CelesteOmegaImpossible = participationId;
                    EnemyEntity.CelesteOmegaImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.Shiva:
                    EnemyEntity.Shiva = participationId;
                    EnemyEntity.ShivaTime = DateTime.Now;
                    break;
                case EnemyType.Europa:
                    EnemyEntity.Europa = participationId;
                    EnemyEntity.EuropaTime = DateTime.Now;
                    break;
                case EnemyType.GodswornAlexiel:
                    EnemyEntity.GodswornAlexiel = participationId;
                    EnemyEntity.GodswornAlexielTime = DateTime.Now;
                    break;
                case EnemyType.Grimnir:
                    EnemyEntity.Grimnir = participationId;
                    EnemyEntity.GrimnirTime = DateTime.Now;
                    break;
                case EnemyType.Metatron:
                    EnemyEntity.Metatron = participationId;
                    EnemyEntity.MetatronTime = DateTime.Now;
                    break;
                case EnemyType.Avatar:
                    EnemyEntity.Avatar = participationId;
                    EnemyEntity.AvatarTime = DateTime.Now;
                    break;
                case EnemyType.TwinElementsImpossible:
                    EnemyEntity.TwinElementsImpossible = participationId;
                    EnemyEntity.TwinElementsImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.MaculaMariusImpossible:
                    EnemyEntity.MaculaMariusImpossible = participationId;
                    EnemyEntity.MaculaMariusImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.MedusaImpossible:
                    EnemyEntity.MedusaImpossible = participationId;
                    EnemyEntity.MedusaImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.NezhaImpossible:
                    EnemyEntity.NezhaImpossible = participationId;
                    EnemyEntity.NezhaImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.ApolloImpossible:
                    EnemyEntity.ApolloImpossible = participationId;
                    EnemyEntity.ApolloImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.DarkAngelOliviaImpossible:
                    EnemyEntity.DarkAngelOliviaImpossible = participationId;
                    EnemyEntity.DarkAngelOliviaImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.Prometheus:
                    EnemyEntity.Prometheus = participationId;
                    EnemyEntity.PrometheusTime = DateTime.Now;
                    break;
                case EnemyType.CaOng:
                    EnemyEntity.CaOng = participationId;
                    EnemyEntity.CaOngTime = DateTime.Now;
                    break;
                case EnemyType.Gilgamesh:
                    EnemyEntity.Gilgamesh = participationId;
                    EnemyEntity.GilgameshTime = DateTime.Now;
                    break;
                case EnemyType.Morrigna:
                    EnemyEntity.Morrigna = participationId;
                    EnemyEntity.MorrignaTime = DateTime.Now;
                    break;
                case EnemyType.Hector:
                    EnemyEntity.Hector = participationId;
                    EnemyEntity.HectorTime = DateTime.Now;
                    break;
                case EnemyType.Anubis:
                    EnemyEntity.Anubis = participationId;
                    EnemyEntity.AnubisTime = DateTime.Now;
                    break;
                case EnemyType.TiamatMalice:
                    EnemyEntity.TiamatMalice = participationId;
                    EnemyEntity.TiamatMaliceTime = DateTime.Now;
                    break;
                case EnemyType.LeviathanMalice:
                    EnemyEntity.LeviathanMalice = participationId;
                    EnemyEntity.LeviathanMaliceTime = DateTime.Now;
                    break;
                case EnemyType.Phronesis:
                    EnemyEntity.Phronesis = participationId;
                    EnemyEntity.PhronesisTime = DateTime.Now;
                    break;
                case EnemyType.ProtoBahamutImpossible:
                    EnemyEntity.ProtoBahamutImpossible = participationId;
                    EnemyEntity.ProtoBahamutImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.Akasha:
                    EnemyEntity.Akasha = participationId;
                    EnemyEntity.AkashaTime = DateTime.Now;
                    break;
                case EnemyType.Lucilius:
                    EnemyEntity.Lucilius = participationId;
                    EnemyEntity.LuciliusTime = DateTime.Now;
                    break;
                case EnemyType.UltimateBahamutImpossible:
                    EnemyEntity.UltimateBahamutImpossible = participationId;
                    EnemyEntity.UltimateBahamutImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.LisiliusImpossible:
                    EnemyEntity.LisiliusImpossible = participationId;
                    EnemyEntity.LisiliusImpossibleTime = DateTime.Now;
                    break;
                case EnemyType.HuanglongQilin:
                    EnemyEntity.HuanglongQilin = participationId;
                    EnemyEntity.HuanglongQilinTime = DateTime.Now;
                    break;
                case EnemyType.AllPrimarch:
                    EnemyEntity.AllPrimarch = participationId;
                    EnemyEntity.AllPrimarchTime = DateTime.Now;
                    break;
            }
        }

        public static string GetEntityParticipationId(string enemy)
        {
            switch (enemy)
            {
                case EnemyType.TiamatOmega:
                    return EnemyEntity.TiamatOmega;
                case EnemyType.ColossusOmega:
                    return EnemyEntity.ColossusOmega;
                case EnemyType.LeviathanOmega:
                    return EnemyEntity.LeviathanOmega;
                case EnemyType.YggdrasilOmega:
                    return EnemyEntity.YggdrasilOmega;
                case EnemyType.LumminieraOmega:
                    return EnemyEntity.LumminieraOmega;
                case EnemyType.CelesteOmega:
                    return EnemyEntity.CelesteOmega;
                case EnemyType.TwinElements:
                    return EnemyEntity.TwinElements;
                case EnemyType.MaculaMarius:
                    return EnemyEntity.MaculaMarius;
                case EnemyType.Medusa:
                    return EnemyEntity.Medusa;
                case EnemyType.Nezha:
                    return EnemyEntity.Nezha;
                case EnemyType.Apollo:
                    return EnemyEntity.Apollo;
                case EnemyType.DarkAngelOlivia:
                    return EnemyEntity.DarkAngelOlivia;
                case EnemyType.Athena:
                    return EnemyEntity.Athena;
                case EnemyType.Grani:
                    return EnemyEntity.Grani;
                case EnemyType.Baal:
                    return EnemyEntity.Baal;
                case EnemyType.Garuda:
                    return EnemyEntity.Garuda;
                case EnemyType.Odin:
                    return EnemyEntity.Odin;
                case EnemyType.Lich:
                    return EnemyEntity.Lich;
                case EnemyType.Michael:
                    return EnemyEntity.Michael;
                case EnemyType.Gabriel:
                    return EnemyEntity.Gabriel;
                case EnemyType.Uriel:
                    return EnemyEntity.Uriel;
                case EnemyType.Raphael:
                    return EnemyEntity.Raphael;
                case EnemyType.ProtoBahamut:
                    return EnemyEntity.ProtoBahamut;
                case EnemyType.GrandOrder:
                    return EnemyEntity.GrandOrder;
                case EnemyType.Huanglong:
                    return EnemyEntity.Huanglong;
                case EnemyType.Qilin:
                    return EnemyEntity.Qilin;
                case EnemyType.UltimateBahamut:
                    return EnemyEntity.UltimateBahamut;
                case EnemyType.RoseQueen:
                    return EnemyEntity.RoseQueen;
                case EnemyType.TiamatOmegaImpossible:
                    return EnemyEntity.TiamatOmegaImpossible;
                case EnemyType.ColossusOmegaImpossible:
                    return EnemyEntity.ColossusOmegaImpossible;
                case EnemyType.LeviathanOmegaImpossible:
                    return EnemyEntity.LeviathanOmegaImpossible;
                case EnemyType.YggdrasilOmegaImpossible:
                    return EnemyEntity.YggdrasilOmegaImpossible;
                case EnemyType.LumminieraOmegaImpossible:
                    return EnemyEntity.LumminieraOmegaImpossible;
                case EnemyType.CelesteOmegaImpossible:
                    return EnemyEntity.CelesteOmegaImpossible;
                case EnemyType.Shiva:
                    return EnemyEntity.Shiva;
                case EnemyType.Europa:
                    return EnemyEntity.Europa;
                case EnemyType.GodswornAlexiel:
                    return EnemyEntity.GodswornAlexiel;
                case EnemyType.Grimnir:
                    return EnemyEntity.Grimnir;
                case EnemyType.Metatron:
                    return EnemyEntity.Metatron;
                case EnemyType.Avatar:
                    return EnemyEntity.Avatar;
                case EnemyType.TwinElementsImpossible:
                    return EnemyEntity.TwinElementsImpossible;
                case EnemyType.MaculaMariusImpossible:
                    return EnemyEntity.MaculaMariusImpossible;
                case EnemyType.MedusaImpossible:
                    return EnemyEntity.MedusaImpossible;
                case EnemyType.NezhaImpossible:
                    return EnemyEntity.NezhaImpossible;
                case EnemyType.ApolloImpossible:
                    return EnemyEntity.ApolloImpossible;
                case EnemyType.DarkAngelOliviaImpossible:
                    return EnemyEntity.DarkAngelOliviaImpossible;
                case EnemyType.Prometheus:
                    return EnemyEntity.Prometheus;
                case EnemyType.CaOng:
                    return EnemyEntity.CaOng;
                case EnemyType.Gilgamesh:
                    return EnemyEntity.Gilgamesh;
                case EnemyType.Morrigna:
                    return EnemyEntity.Morrigna;
                case EnemyType.Hector:
                    return EnemyEntity.Hector;
                case EnemyType.Anubis:
                    return EnemyEntity.Anubis;
                case EnemyType.TiamatMalice:
                    return EnemyEntity.TiamatMalice;
                case EnemyType.LeviathanMalice:
                    return EnemyEntity.LeviathanMalice;
                case EnemyType.Phronesis:
                    return EnemyEntity.Phronesis;
                case EnemyType.ProtoBahamutImpossible:
                    return EnemyEntity.ProtoBahamutImpossible;
                case EnemyType.Akasha:
                    return EnemyEntity.Akasha;
                case EnemyType.Lucilius:
                    return EnemyEntity.Lucilius;
                case EnemyType.UltimateBahamutImpossible:
                    return EnemyEntity.UltimateBahamutImpossible;
                case EnemyType.LisiliusImpossible:
                    return EnemyEntity.LisiliusImpossible;
                case EnemyType.HuanglongQilin:
                    return EnemyEntity.HuanglongQilin;
                case EnemyType.AllPrimarch:
                    return EnemyEntity.AllPrimarch;
                default:
                    return "";
            }
        }

        public static DateTime GetEntityTime(string enemy)
        {
            switch (enemy)
            {
                case EnemyType.TiamatOmega:
                    return EnemyEntity.TiamatOmegaTime;
                case EnemyType.ColossusOmega:
                    return EnemyEntity.ColossusOmegaTime;
                case EnemyType.LeviathanOmega:
                    return EnemyEntity.LeviathanOmegaTime;
                case EnemyType.YggdrasilOmega:
                    return EnemyEntity.YggdrasilOmegaTime;
                case EnemyType.LumminieraOmega:
                    return EnemyEntity.LumminieraOmegaTime;
                case EnemyType.CelesteOmega:
                    return EnemyEntity.CelesteOmegaTime;
                case EnemyType.TwinElements:
                    return EnemyEntity.TwinElementsTime;
                case EnemyType.MaculaMarius:
                    return EnemyEntity.MaculaMariusTime;
                case EnemyType.Medusa:
                    return EnemyEntity.MedusaTime;
                case EnemyType.Nezha:
                    return EnemyEntity.NezhaTime;
                case EnemyType.Apollo:
                    return EnemyEntity.ApolloTime;
                case EnemyType.DarkAngelOlivia:
                    return EnemyEntity.DarkAngelOliviaTime;
                case EnemyType.Athena:
                    return EnemyEntity.AthenaTime;
                case EnemyType.Grani:
                    return EnemyEntity.GraniTime;
                case EnemyType.Baal:
                    return EnemyEntity.BaalTime;
                case EnemyType.Garuda:
                    return EnemyEntity.GarudaTime;
                case EnemyType.Odin:
                    return EnemyEntity.OdinTime;
                case EnemyType.Lich:
                    return EnemyEntity.LichTime;
                case EnemyType.Michael:
                    return EnemyEntity.MichaelTime;
                case EnemyType.Gabriel:
                    return EnemyEntity.GabrielTime;
                case EnemyType.Uriel:
                    return EnemyEntity.UrielTime;
                case EnemyType.Raphael:
                    return EnemyEntity.RaphaelTime;
                case EnemyType.ProtoBahamut:
                    return EnemyEntity.ProtoBahamutTime;
                case EnemyType.GrandOrder:
                    return EnemyEntity.GrandOrderTime;
                case EnemyType.Huanglong:
                    return EnemyEntity.HuanglongTime;
                case EnemyType.Qilin:
                    return EnemyEntity.QilinTime;
                case EnemyType.UltimateBahamut:
                    return EnemyEntity.UltimateBahamutTime;
                case EnemyType.RoseQueen:
                    return EnemyEntity.RoseQueenTime;
                case EnemyType.TiamatOmegaImpossible:
                    return EnemyEntity.TiamatOmegaImpossibleTime;
                case EnemyType.ColossusOmegaImpossible:
                    return EnemyEntity.ColossusOmegaImpossibleTime;
                case EnemyType.LeviathanOmegaImpossible:
                    return EnemyEntity.LeviathanOmegaImpossibleTime;
                case EnemyType.YggdrasilOmegaImpossible:
                    return EnemyEntity.YggdrasilOmegaImpossibleTime;
                case EnemyType.LumminieraOmegaImpossible:
                    return EnemyEntity.LumminieraOmegaImpossibleTime;
                case EnemyType.CelesteOmegaImpossible:
                    return EnemyEntity.CelesteOmegaImpossibleTime;
                case EnemyType.Shiva:
                    return EnemyEntity.ShivaTime;
                case EnemyType.Europa:
                    return EnemyEntity.EuropaTime;
                case EnemyType.GodswornAlexiel:
                    return EnemyEntity.GodswornAlexielTime;
                case EnemyType.Grimnir:
                    return EnemyEntity.GrimnirTime;
                case EnemyType.Metatron:
                    return EnemyEntity.MetatronTime;
                case EnemyType.Avatar:
                    return EnemyEntity.AvatarTime;
                case EnemyType.TwinElementsImpossible:
                    return EnemyEntity.TwinElementsImpossibleTime;
                case EnemyType.MaculaMariusImpossible:
                    return EnemyEntity.MaculaMariusImpossibleTime;
                case EnemyType.MedusaImpossible:
                    return EnemyEntity.MedusaImpossibleTime;
                case EnemyType.NezhaImpossible:
                    return EnemyEntity.NezhaImpossibleTime;
                case EnemyType.ApolloImpossible:
                    return EnemyEntity.ApolloImpossibleTime;
                case EnemyType.DarkAngelOliviaImpossible:
                    return EnemyEntity.DarkAngelOliviaImpossibleTime;
                case EnemyType.Prometheus:
                    return EnemyEntity.PrometheusTime;
                case EnemyType.CaOng:
                    return EnemyEntity.CaOngTime;
                case EnemyType.Gilgamesh:
                    return EnemyEntity.GilgameshTime;
                case EnemyType.Morrigna:
                    return EnemyEntity.MorrignaTime;
                case EnemyType.Hector:
                    return EnemyEntity.HectorTime;
                case EnemyType.Anubis:
                    return EnemyEntity.AnubisTime;
                case EnemyType.TiamatMalice:
                    return EnemyEntity.TiamatMaliceTime;
                case EnemyType.LeviathanMalice:
                    return EnemyEntity.LeviathanMaliceTime;
                case EnemyType.Phronesis:
                    return EnemyEntity.PhronesisTime;
                case EnemyType.ProtoBahamutImpossible:
                    return EnemyEntity.ProtoBahamutImpossibleTime;
                case EnemyType.Akasha:
                    return EnemyEntity.AkashaTime;
                case EnemyType.Lucilius:
                    return EnemyEntity.LuciliusTime;
                case EnemyType.UltimateBahamutImpossible:
                    return EnemyEntity.UltimateBahamutImpossibleTime;
                case EnemyType.LisiliusImpossible:
                    return EnemyEntity.LisiliusImpossibleTime;
                case EnemyType.HuanglongQilin:
                    return EnemyEntity.HuanglongQilinTime;
                case EnemyType.AllPrimarch:
                    return EnemyEntity.AllPrimarchTime;
                default:
                    return DateTime.MaxValue;
            }
        }

        public static void OpenChrome(string path)
        {
            MainWindow.logger.Info("Chromeを開きます");

            // ChromeOptions
            var options = new ChromeOptions();
            options.AddArgument(USER_DATA_DIR + path);
            options.AddArgument(DISABLE_INFOBARS);

            // ChromeDriver
            var driverService = ChromeDriverService.CreateDefaultService();
            // HideConsole
            driverService.HideCommandPromptWindow = true;

            // ChromeOpen
            try
            {
                driver = new ChromeDriver(driverService, options);
                MainWindow.logger.Info("Chrome出力成功");
            }
            catch (Exception e)
            {
                MessageBox.Show("Chromeを開いていれば閉じてください。\r\nもしくは、コマンドライン引数に不備があります。");
                MainWindow.logger.Error(e.ToString());
                MainWindow.logger.Error("プロフィールパス：" + USER_DATA_DIR + path);

                // kill all processes
                ChromeHelper.KillAllProcesses("chromedriver");
            }

            // グラブルに遷移
            driver.Url = GRANBLUE_URL;
            MainWindow.logger.Info("open the GranblueFantasy");
        }

        public static void MultiBattleText(TextBlock textBlock)
        {
            MainWindow.logger.Info("救援IDを入力します。");
            // マルチの画面開いてないと無効
            if (driver.Url.Equals(MULTI_URL))
            {
                try
                {
                    // テキストクリア=>テキスト入力
                    driver.FindElementByClassName("frm-battle-key").Clear();
                    driver.FindElementByClassName("frm-battle-key").SendKeys(GetEntityParticipationId(textBlock.Text.ToString()));
                    MainWindow.logger.Debug("救援ID：" + textBlock.Text.ToString() + "を入力しました。");
                }
                catch (Exception ignored)
                {
                }
            }
        }

        public static void UrlChecker(CancellationToken cancelToken)
        {
            MainWindow.logger.Info("URLチェック開始");
            try {
                string restartUrl = driver.Url;
                WebDriverWait webDriverWait = new WebDriverWait(driver, new TimeSpan(0, 0, 3));

                while (true)
                {
                    if (driver.Url.Contains("#raid") && driver.FindElementByClassName("txt-gauge-value").Text.Equals("0"))
                    {
                        try
                        {
                            driver.Url = RESULT_URL;
                            MainWindow.logger.Debug("リザルト遷移：" + RESULT_URL);
                            webDriverWait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("btn-usual-ok")));
                            MainWindow.logger.Debug("リザルト遷移確認");
                            driver.Url = restartUrl;
                            MainWindow.logger.Debug("召喚石選択画面遷移：" + restartUrl);

                        }
                        catch (Exception ignored) { }
                    }
                    if (cancelToken.IsCancellationRequested)
                    {
                        MainWindow.logger.Info("URLチェック終了");
                        return;
                    }
                }
            }
            catch(Exception e)
            {
                MainWindow.logger.Warn(e.ToString());
                UrlChecker(cancelToken);
            }
        }
    }
}
