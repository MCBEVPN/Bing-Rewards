using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bing_Rewards.Utilities
{
    public static class QuestionUtility
    {
        public static string[] Questions
        {
            get
            {
                string[] questions = new string[50];
                questions[0] = "什么是 RPG 游戏的缩写？";
                questions[1] = "《马里奥》系列的主角是一个什么职业？";
                questions[2] = "《刺客信条》系列的故事背景涉及了一个叫什么名字的古代文明？";
                questions[3] = "《生化危机》系列中，导致僵尸病毒爆发的邪恶组织叫什么？";
                questions[4] = "《塞尔达传说》系列中，塞尔达公主的守护者是一只什么动物？";
                questions[5] = "《使命召唤》系列中，最受欢迎的多人游戏模式是什么？";
                questions[6] = "《守望先锋》中，有多少个英雄可以选择？";
                questions[7] = "《魔兽世界》中，有多少个种族可以选择？";
                questions[8] = "《英雄联盟》中，有多少个位置可以选择？";
                questions[9] = "《我的世界》中，可以用来制作火把的材料是什么？";
                questions[10] = "《俄罗斯方块》中，最高级别是多少？";
                questions[11] = "《贪吃蛇》中，蛇的身体由什么组成？";
                questions[12] = "《愤怒的小鸟》中，有多少种不同颜色和能力的小鸟？";
                questions[13] = "《植物大战僵尸》中，可以用来收集阳光的植物是什么？";
                questions[14] = "《超级马里奥制造》中，可以用来制作关卡的游戏风格有哪些？";
                questions[15] = "《辐射》系列中，可以用来测量辐射水平和其他信息的设备叫什么？";
                questions[16] = "《上古卷轴 V：天际》中，可以用来释放强大能力的语言叫什么？";
                questions[17] = "《侠盗猎车手 V》中，可以切换控制的三个主角分别叫什么名字？";
                questions[18] = "《神秘海域》系列中，主角是一个什么职业？";
                questions[19] = "《最后生还者》中，导致人类灭继危机的真正原因是什么？";
                questions[20] = "《合金装备》系列中，主角索利德·蛇是一个什么样的人物？";
                questions[21] = "《生死格斗》系列中，最具标志性的女性角色是谁？";
                questions[22] = "《街头霸王》系列中，春丽的国籍是哪个国家？";
                questions[23] = "《实况足球》系列中，最高评分的球员是谁？";
                questions[24] = "《NBA 2K》系列中，最高评分的球员是谁？";
                questions[25] = "《FIFA》系列中，最高评分的球员是谁？";
                questions[26] = "《极品飞车》系列中，最经典的赛车模式是什么？";
                questions[27] = "《极限竞速：地平线》系列中，最新一作的游戏地图是哪个国家或地区？";
                questions[28] = "《音速小子》系列中，音速小子最忠实的伙伴是谁？";
                questions[29] = "《洛克人》系列中，洛克人最强大的武器是什么？";
                questions[30] = "《魂斗罗》系列中，可以让玩家获得 30 条命的秘籍是什么？";
                questions[31] = "《忍者神龟》系列中，四个忍者神龟的名字分别是什么？";
                questions[32] = "《疯狂小鸡》系列中，小鸡们的目标是什么？";
                questions[33] = "《超级机器人大战》系列中，可以参战的机器人作品有哪些？";
                questions[34] = "《火焰纹章》系列中，可以让玩家控制角色的行动顺序的系统叫什么？";
                questions[35] = "《口袋妖怪》系列中，最初的三只宝可梦分别是什么？";
                questions[36] = "《最终幻想》系列中，最受欢迎的一作是哪一部？";
                questions[37] = "《王国之心》系列中，主角索拉的武器是什么？";
                questions[38] = "《塞尔达传说：旷野之息》中，可以让玩家探索的游戏世界叫什么名字？";
                questions[39] = "《黑暗之魂》系列中，玩家在死亡后会留下什么东西？";
                questions[40] = "《只狼：影逝二度》中，主角狼的左臂可以装备什么武器？";
                questions[41] = "《鬼泣》系列中，但丁和维吉尔的父亲是谁？";
                questions[42] = "《生化奇兵》系列中，第一作的游戏场景是一个叫什么名字的水下城市？";
                questions[43] = "《半条命》系列中，主角戈登·弗里曼的职业是什么？";
                questions[44] = "《传送门》系列中，玩家可以使用一个叫什么名字的装置来创造传送门？";
                questions[45] = "《边缘世界》中，玩家可以控制一个叫什么名字的殖民地？";
                questions[46] = "《文明》系列中，玩家可以选择多少种不同的胜利方式？";
                questions[47] = "《模拟人生》系列中，玩家可以用来控制模拟人生的语言叫什么？";
                questions[48] = "《我的世界：故事模式》中，主角杰西和他的朋友们想要参加一个叫什么名字的比赛？";
                questions[49] = "《杯头》中，杯头和他的弟弟为了还清债务而要挑战一些叫什么名字的敌人？";
                return questions;
            }
        }
        public static string GetRandomQuestion()
        {
            string[] questions = Questions;
            Random random = new Random();
            int index = random.Next(0, questions.Length);
            return questions[index];
        }
    }
}
