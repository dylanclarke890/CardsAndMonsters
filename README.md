# CardsAndMonsters

## 24-12-2021 03:54PM (roughly 12 hours of progress)

I was curious how much work it would take to build my own card game so this is it! First attempting an MVP with a familiar language before rebuilding in C++ so that I can focus more on getting stuck in rather than dealing with memory allocation and other headaches I don't want to deal with quite yet. I think this will be a good learning experience, both because I've already found it really fun to plan out the rules for my own game (shamelessly stolen from other similar card games for now as this is just a learner project but I am planning on adding special rules as and when I find a good original one and who knows, maybe this will join the many shelves of Steam indie games gathering dust one day) and because, despite it being just a card game, there's a good few complex subjects I should be able to cover. Initial plan is to get a simple one-sided game going with the various rules I've picked for an MVP (mainly limits on how many cards can be played per turn, how the game ends, types of cards etc), then look at implementing an AI (in C# for now) to take over the opponents role. Hoping I can remember to keep up with this blog too but I suppose I'll add a reminder on my TODO...

## 25-12-2021 06:26PM (17ish hours)

Merry Christmas! Had some time to spare after a lovely day with the partner, so focused on switching the positions of cards. In this game, cards can be played face up/down and also horizontally/vertically which also is restricted based on the type of card. Managed to add the functionality but introduced a new bug that stops more than one monster card being played (always nice) so that'll be my next focus. Learned a lot more about how the equality comparer works (value equality vs reference equality) and so removed a redundant interface I implemented for the player class. The code is starting to get a little hard to follow (being tired doesn't help) but I'm going to crack on for now before I look at a refactor, took me longer than expected to get this feature added and I'm going to have to add spell and trap cards before I've got all the pieces I need to be able to look at the full game properly. Once I've fixed any bugs that are still hanging around I also need to look at the more advanced logic for attacking then I can see where to go from there. It's possible I'll stick with C# long term for the project, as I was unaware Unity allowed you to use that as well as C++. We'll see what happens though, in the meantime I think an early night is in order...

## 26-12-2021 11:21AM (about 21 hours)
Pretty good progress today, managed to add the logic for attacking (which involves having to deal with monsters before being able to attack directly, and added different rules for how the "battles" go when the cards are in different positions. Also added neccessary restrictions, dealt with some bugs that had creeped in and did some refactoring to break things into components, organised the code which on second glance looks fine, could do with having services to manage some of the functionality but not neccessary at present. Also added a mock AI for now which plays a random card from their hand and switches the positions of any monsters they have on the field each turn. Turned out to be a lot easier than I thought so hopefully that extends onto the actual AI build. Going to polish up the look of the app a little bit before I continue, but should be moving on to Spell cards after that. Managed to keep on top of my TODO pretty nicely so pretty pleased with overall progress :D.

## 27-12-2021 11:57AM (about 24 hours)
So just about hit a days worth of work, last few hours weren't too productive as I added toast notifications but they were really annoying and there needs to be separate types of displays for different situations (e.g switching phase should cause a small delay but a message stating a monster has been destroyed shouldn't, and they should appear differently too). Looking forward to getting this boxed off so I can look at touching up the graphics a bit. Also considering reducing the board size to 3 instead of 5 which would help with both keeping track of everything and fitting the entire game onto one screen (which it's not far off at the moment).

## 28-12-21 1:10PM (29 hours)
Spent a few hours touching up the design. Nothing fancy mind, but made the board into an actual board that doesn't change the layout based on how many cards are in play and moved some other components around to start building the rough outline of what the final board will look like. Cleaned up some code too as the game component was getting pretty bloated and there were some redundancies starting to creep in. I still need to add some more services, mainly one to handle the board, one to handle turns and one to fake the opponents turn. Once that's done I can add a new service to create the logs for the duel and (potentially separate) a notification service to display things like phase change etc. Managed to get some more experience using delegates so that's always useful.

## 29-12-21 11:15AM (33 hours)
Spent some more time cleaning up the code, managed to move all the code into a gameservice which will come in useful shortly as I continue to add features. Worked a lot more with Action and delegates today, which turned out to be quite fun. I had some issues figuring out where to put the calls to notify components that the phase had changed, at one point I had around a dozen invokes scattered throughout the gameservice "just to be safe", but it still wasn't giving me the results I wanted, then I added another delegate to to the phase service to fire whenever the phase had successfully changed and just linked the gameservice's action (which i subscribe components to and fire statehaschanged once invoked) to that and it worked very nicely so pretty happy with that.

## 29-12-21 15:48PM (35ish hours)
Just a quick update, added a logging service to keep track of everything that happens during a duel. Slowly getting everything ready to be able to resume a duel from where I left off (via localstorage) if disconnected which might cause a few issues with how things are currently set up, but in the process of enforcing Single Responsibility to stop this from becoming a long-term issue. Also getting to the point where some unit tests might be required to stop a load of headaches from happening so will see when I can find time to add those (also want to make sure services aren't going/likely to change before I do). 
