# VarietyMattersImproved

![Image](https://i.imgur.com/buuPQel.png)

Update of Cozarkians mod
https://steamcommunity.com/sharedfiles/filedetails/?id=2353847457

- Added french translation, thanks qux!
- Set a maximum of low variety for prisoners, slaves and ascetic pawns

![Image](https://i.imgur.com/CN9Rs5X.png)

	
![Image](https://i.imgur.com/Z4GOv8H.png)

# What Does This Mod Do?


Pawns now have a variety need. Pawns satisfy the need by eating different varieties of foods. 

# Basic Guide

Open a pawn's need tab and hover over variety. The tooltip will display Varieties Expected and Recent Varieties. Every time your pawn eats, these numbers change and you gain/lose variety. 

Recent  Expected = Gain variety
Recent  Expected = Lose variety
Recent = Expected = Move toward 50%

# What Counts as a Variety?


To count as a variety, a food must provide nutrition. Thus, for example, ambrosia is a variety while other drugs are not. Second, a food must either provide joy or be at least raw tasty. Thus, beer, chocolate, berries, and cooked meat are varieties, raw meat is not. Unless your pawn happens to be a cannibal, in which case they will treat raw humanlike flesh (and corpses!) as a variety. Finally, rotten foods do not provide variety.

A food's variety is based on its display name (label), not its defName, so "apples" added by two different mods will only provide a single "apples" variety.

The above rules always apply. However, there are three options for how to handle meals (foods that display ingredients):

**[u]Option 1: Ingredients Only[/u]**

By default, ingredients are varieties, not the meals themselves. Grow different crops and hunt different animals and you'll have happy pawns. Feed your pawns nothing but rice, corn, and raiders, and you'll have unhappy pawns. There is NO benefit to providing a mix of pemmican/survival/nutrient paste/simple/fine/lavish meals. However, lavish meals provide an extra "lavishly-prepared" variety as well as the basic variety. Therefore, an all-lavish meal diet will provide twice the variety of a diet without lavish meals.

Meals that can have ingredients but don't (e.g. bought from traders, Replimat meals) are assigned a random "mystery-ingredient" variety when eaten. Fine and lavish meals also provide a mystery meat, and lavish meals provide a mystery lavish ingredient. The number of available mystery varieties depend on current expectations. It is balanced so a diet of all-fine mystery meals should approach a neutral (50%) variety need, while a diet of all-lavish mystery meals will slowly increase variety, but there is RNG involved.

**[u]Option 2: Ignore Ingredients[/u]**

If you choose to enable the option to ignore ingredients then varieties will be based solely on the final product consumed, ignoring ingredients. Your pawns will want a mix of simple, fine, and lavish meals, pemmican and nutrient paste, but won't care if they are made entirely from corn and chicken. 

This option is recommended only if you use mods with lots of new meals such as the Vanilla Cooking Expanded series or VGP Gourment Garden. 

**[u]Option 3: Track Ingredients and Meals[/u]**

This option causes pawns to track both the name of the meal and its ingredients. With this option you will need to both provide a variety of ingredients and a variety of meals. If, for example, you feed your pawns simple chicken meal and fine chicken/corn meal, you will have 4 varieties (rat, corn, simple meal, fine meal) compared to two varieties (rat/corn) with option 1 or two varieties (simple/fine meal) with option 2.

# How Much Variety Do Pawns Need?


The base variety expectation depends on a pawn's current thought expectation and is adjustable in settings.

The expectation is then modified based on a pawn's current variety level. The closer a pawn is to 100% variety, the harder it is to stay there. The variety expectation will also change based on the number of mod-added varieties available at load-time and slightly decrease when the weather isn't suitable for growing crops. 

# How do Pawns Track Varieties?


Pawns remember the foods they've eaten up to 2x their current variety expectation. That means half of a pawn's diet needs to be new varieties in order to meet their expectation. The other half can be anything. For example, a pawn with an expectation of 10 that eats 10 fine rice bowls, each with a unique type of meat, will have 11 varieties (rice + 10 meats).

Once a pawn's memory is full, it  will randomly forget a prior food to make room for the new meal. In the above example, if the pawn eats a simple, meatless rice bowl, there is a 50% chance it will replace one of the earlier rice meals and a 50% chance it will replace a meat memory, reducing variety to 10.

# Will Pawns Look for New Varieties When Eating?


Yes! When choosing meals, pawns will prefer meals with at least one new variety. How much extra preference they assign to new varieties depends on their expectation. Thus, in the early game when expectations are low, pawns might still prefer nearby or spoiling food, but when variety expectations are high, they will put more emphasis on eating new varieties. Pawns will still generally prefer lavish to fine to simple, except at the most extreme variety expectations.

# Will Pawns Use a Variety of Ingredients When Cooking?


Yes! New to this version, cooks will also look for new varieties when cooking meals. Similar to eating, cooks will base their ingredient choices on distance, variety, and (optionally) spoiling, with more emphasis on variety when expectations are high. 

I didn't have performance issues during testing, but if you experience late-game issues, shrink the ingredient radius of your bills and use https://steamcommunity.com/sharedfiles/filedetails/?id=2266068546] Variety Matters Stockpiles to ensure a variety of ingredients are available to your cooks. Alternatively, use option 2 (ignore ingredients) which disables this feature as unnecessary.

# Are There Any Other Features?


There is a (recommended) option to prevent meals with different ingredients from stacking, which increases the variety of foods available. The downside is the need for extra storage space. Alternatively, you can increase the number of ingredients the game tracks when stacking meals. That option was added by request and how it affects variety has not been tested. Both of these features are unnecessary and disabled if using Option 2.

By default, sick pawns will temporarily stop caring about varieties. By request, a toggle was added to disable this feature. If I recall correctly, certain non-human race mods apply a beneficial hediff that is treated as being sick.

# How is This Different than the Original Variety Matters?


The original version used a more robust tracking system with cyclical memory dumps that tried to reward pawns for eating new varieties and punish them for eating too much of the same thing

![Image](https://i.imgur.com/PwoNOj4.png)



-  See if the the error persists if you just have this mod and its requirements active.
-  If not, try adding your other mods until it happens again.
-  Post your error-log using https://steamcommunity.com/workshop/filedetails/?id=818773962]HugsLib and command Ctrl+F12
-  For best support, please use the Discord-channel for error-reporting.
-  Do not report errors by making a discussion-thread, I get no notification of that.
-  If you have the solution for a problem, please post it to the GitHub repository.



