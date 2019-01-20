天气牌+烧灼 4group
近战 1group
远程 2group
攻城 3group
诱饵 0group

优化 area类与desk类


在我的card.cs中大量并重复使用了成员变量：
	private GameObject cardModelGameObject
	private CardModel cardModel
是否可以使用单列模式



整个程序运行的主线程与协程之间的关系，及如何做到主线程这边放置一张间谍卡，协程那边能够检测到并且让打出间谍卡这边的玩家能够抽取2张卡

思路1：在card上面设置一个active变量，当被选中的时候，active赋值为true，如果选中间谍牌的同时(state == (int)Status.ACTIVE_CARD || (activeCard.getIsSpecial() == (int)TypeOfCard.SPY || 
	 activeCard.getIsSpecial() == (int)TypeOfCard.GOLD_SPY))，点击的是对面玩家的近战组button(activeCard.getGroup() == (int)CardGroup.SWORD)，即刻出发间谍牌技能

activePlayerNumber	1	System.Int32
gameStatus	1	System.Int32
state	1	System.Int32
以上三个变量的意义
	 
	 

需要变量: state(状态) / Status.ACTIVE_CARD / activeCard.getIsSpecial() / TypeOfCard.SPY / activeCard.getIsSpecial() / TypeOfCard.GOLD_SPY  activeCard.getGroup() / (int)CardGroup.SWORD
 
 
if (state == (int)Status.ACTIVE_CARD && 
	(activeCard.getIsSpecial() == (int)TypeOfCard.SPY || 
	activeCard.getIsSpecial() == (int)TypeOfCard.GOLD_SPY) && 
	activeCard.getGroup() == (int)CardGroup.SWORD
	)


昆特牌：
1. 手牌选中的浮动，以及如何显示big card在右边框的
2. play1 -> play2 的视角转换
3. 点数计算以及天气牌的影响


deckCollider.center = "(1.3, -5.2, 0.0) + + new Vector3(0, -0.1456f, -0.1f)"
centerVector = "(1.3, -5.4, -0.1)"


如果失败的哪一方手牌为空并不会出现BUG


功能：
墓地牌的3D效果 完成!!!
x: 每张牌相差0.04,	y: 每张牌相差0.04, z: 每张相差0.2
bug,需要明天多次测试

能够通过滑动观察墓地卡牌

Bug: 
① 无法间谍牌并未受到天气牌的影响！解决！！
② 第二回发牌,如果当前玩家手牌为空进入次回合，会出现Big Bug  解决！！
③ 多重天气牌效果影响的梳理   解决！！ UI问题，显示叠加
④ 当手牌为0和10的时候触发终极大BUG直接交换了双方手牌位置具体请自行实现 
⑤ 灼烧应该烧下去对方战场上最大的且点数相同的全部卡片且如果场上没有最大点数的牌，会报错越界异常	解决！！
⑥ 天气牌影响后通过稻草人撤回被影响的牌后，此牌再次打 weatherEffect依旧为true	解决！！
⑦ 假如一方手牌打完后，次回合另一方开局点击的第一张牌会被当做已被选中的牌，点击其他牌的时候就会被浮动下去 
⑧ 一定情况下，会出现手牌为互换的情况
⑨ 烧灼使用后，应该直接去往墓地，而不是停留在特殊卡牌框组中 解决！！
⑩ 晴天使用后，应该直接去往自加墓地，而不是通过sendCardToDeathList函数是它位置去往对方墓地，而当前使用晴天的玩家的deathList中包含晴天牌 解决！！
⑾ player2墓地卡牌初始位置	解决！！
⑾ 烧灼之后未更新位置		解决！！
⑾ 单独烧灼的时候卡牌的位置(细节)  
⑾ 如下问题:
8.63 4.48 -0.16
8.59 4.52 -0.14
8.55 4.56 -0.12
8.51 4.52 -0.09999999
y轴出现问题
⑾ 双方墓地在次回合交换位置时发生位置上了颠倒
⑾ 有时会出现一种BUG，平局，游戏结束之后，游戏没有退出依然处于出牌阶段
 
⑾ 一切围绕墓地问题开展，首先是由这个moveCardsFromDeskToDeathArea函数，进行了回合时的墓地清洗(理清楚情况)，然后就是每次交换玩家出牌时，函数changeDeathPos
理解：
moveCardsFromDeskToDeathArea函数，用于玩家在次回合的卡牌清理至墓地，此时应该判断哈上方玩家的y坐标不应该是下方玩家的y坐标的负值！
changeDeathPos函数用于每次玩家交换位置时的坐标替换，同理上方玩家的y坐标不应该是下方玩家的y坐标的负值！
下方：8.51f, -4.57f, -0.1f，y轴的绝对值的值逐渐变小
上方：8.51f, 4.57f, -0.1f，y轴的绝对值的值逐渐变大

测试：
⑤ 灼烧应该烧下去对方战场上最大的且点数相同的全部卡片
测试1:
能够将跨组的相同最大点数烧掉
测试2：
因为if嵌套太深，需要预防跨组 1 2 3的问题，及1与2为1 3的power > 1然后都被烧下去了
测试3：
多重天气影响

搞清楚手牌互换问题，因为天气牌遗留效果引起手牌未发生互换
理清楚  Deck类中cardsInDeaths 成员变量值的情况


