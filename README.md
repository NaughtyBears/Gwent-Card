# Gwent-Card
<div style="text-align:center"><img src="../master/Assets/Images/screen.PNG"></div>


## Bug
Bug: 
⑴ 无法间谍牌并未受到天气牌的影响！解决！！

⑵ 第二回发牌,如果当前玩家手牌为空进入次回合，会出现Big Bug  解决！！

⑶ 多重天气牌效果影响的梳理   解决！！ UI问题，显示叠加

⑷ 当手牌为0和10的时候触发BUG直接交换了双方手牌位置具体请自行实现 

⑸ 灼烧应该烧下去对方战场上最大的且点数相同的全部卡片且如果场上没有最大点数的牌，会报错越界异常	解决！！

⑹ 天气牌影响后通过稻草人撤回被影响的牌后，此牌再次打 weatherEffect依旧为true	解决！！

⑺ 假如一方手牌打完后，次回合另一方开局点击的第一张牌会被当做已被选中的牌，点击其他牌的时候就会被浮动下去 

⑻ 一定情况下，会出现手牌为互换的情况

⑼ 烧灼使用后，应该直接去往墓地，而不是停留在特殊卡牌框组中 解决！！

⑽ 晴天使用后，应该直接去往自加墓地，而不是通过sendCardToDeathList函数是它位置去往对方墓地，而当前使用晴天的玩家的deathList中包含晴天牌 解决！！

⑾ player2墓地卡牌初始位置	解决！！

⑿ 烧灼之后未更新位置		解决！！

⒀ 单独烧灼的时候卡牌的位置(细节)  
如下问题:

8.63 4.48 -0.16

8.59 4.52 -0.14

8.55 4.56 -0.12

8.51 4.52 -0.09999999

y轴出现问题

⒁ 双方墓地在次回合交换位置时发生位置上了颠倒 解决！！

⒂ 有时会出现一种BUG，平局，游戏结束之后，游戏没有退出依然处于出牌阶段
 
⒃ 一切围绕墓地问题开展，首先是由这个moveCardsFromDeskToDeathArea函数，进行了回合时的墓地清洗(理清楚情况)，然后就是每次交换玩家出牌时，函数changeDeathPos 解决！！

理解：

moveCardsFromDeskToDeathArea函数，用于玩家在次回合的卡牌清理至墓地，此时应该判断哈上方玩家的y坐标不应该是下方玩家的y坐标的负值！

changeDeathPos函数用于每次玩家交换位置时的坐标替换，同理上方玩家的y坐标不应该是下方玩家的y坐标的负值！

下方：8.51f, -4.57f, -0.1f，y轴的绝对值的值逐渐变小

上方：8.51f, 4.57f, -0.1f，y轴的绝对值的值逐渐变大

⒄玩家二场地场地上为空时，次回合会出现越界问题 解决！！


