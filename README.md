# CardsAndMonsters

## 26-01-22 18:54PM (UPDATE)
It's been a lot longer than I planned and I'm still working on other projects so I've put this on the back burner for now. Definitely glad I tidied everything up the last time I came back to this so should be a lot easier to return to! Currently working on CipherSharp which is (currently) a bunch of classical and mechanical ciphers I've implemented in C#. I've got to a decent stage of being able to focus on individual algorithms and optimise them, so I'm going to continue with that in the next few weeks and fine-tune things there before I revisit this.

## 09-01-22 16:32PM (60ish hours)

It's been a little while since an update, took a small break to apply for jobs, attend interviews, complete tasks (one of which is now public on my repos and I'm planning on extending it from converting just CSV files to take any file (or as many as I can) and display the relevant options and possible file types it can convert to) and sleep because it's been pretty exhausting. Started on unit tests when I came back to this, partly so that I have demonstrable examples of tests using Moq and xUnit, partly because I didn't want to work on new features and partly because I didn't want to have to come back to it all later. Was a bit of a slog, went through all of the features and added error checks along with unit tests, managed to get 79.2% code coverage in the end and they're all passing (136) so was worth it. Managed to catch a few bugs as well so definitely glad I did it. Would like to get some documentation added before I go any further which I'll continue with in the next few days.


## 03-01-22 20:18PM (45ish hours)

Happy New Year! Haven't had enough updates for a post until now but managed to get quite a few issues sorted. First I boxed off the serializing issue, which just required some additional options passed to the serializer to include the type names automatically. Then I fixed an issue with the duel not resuming properly due to equality checks failing. This was further due to the equals check checking for equality of reference, not equality of value which would and did work with the initial set up, but as I now can save and load the duel and the deserializer creates new instances of each class it needs, it was no longer suitable. After that I did a bit of a more substantial graphics overhaul to include my own card cover and board (space themed for now but eventual plan to add more themes) and addressed some bugs that were mainly fixed by adding checks for whether certain actions had/hadn't been done yet. Still plenty of edge cases to cover I'm sure but pretty happy that I've ironed out the worst ones. Continuing with the rest of the project roadmap from tomorrow.


## 30-12-21 14:49PM (39 hours)

Added features for restarting the game, moved card logic into its own service and starting putting in the code for resuming duels from where they were left off. Successfully added a service for adding and retrieving items from local storage, only issue is deserializing the cards back into the lists properly. Due to them inheriting from BaseCard and the current hand being a list of basecard, the standard serializer doesn't know to serialize the cards as monster cards when being retrieved from localstorage. Should just require a custom deserializer to sort that out and then resuming duels should be fine/require minimal work to get running. Current roadmap is to get resuming duels to work, then moving onto adding delays for actions to add more of a game feel, adding features to watch a replay of the duel, save the duel for later (which'll require auth) and a few other features before another graphics touch up and getting some unit tests done as there's no code coverage currently. Hopefully less than a week's worth of work, possibly more with the tests but we'll see.


## 29-12-21 15:48PM (35ish hours)

Just a quick update, added a logging service to keep track of everything that happens during a duel. Slowly getting everything ready to be able to resume a duel from where I left off (via localstorage) if disconnected which might cause a few issues with how things are currently set up, but in the process of enforcing Single Responsibility to stop this from becoming a long-term issue. Also getting to the point where some unit tests might be required to stop a load of headaches from happening so will see when I can find time to add those (also want to make sure services aren't going/likely to change before I do). 


## 29-12-21 11:15AM (33 hours)

Spent some more time cleaning up the code, managed to move all the code into a gameservice which will come in useful shortly as I continue to add features. Worked a lot more with Action and delegates today, which turned out to be quite fun. I had some issues figuring out where to put the calls to notify components that the phase had changed, at one point I had around a dozen invokes scattered throughout the gameservice "just to be safe", but it still wasn't giving me the results I wanted, then I added another delegate to to the phase service to fire whenever the phase had successfully changed and just linked the gameservice's action (which i subscribe components to and fire statehaschanged once invoked) to that and it worked very nicely so pretty happy with that.


## 28-12-21 1:10PM (29 hours)

Spent a few hours touching up the design. Nothing fancy mind, but made the board into an actual board that doesn't change the layout based on how many cards are in play and moved some other components around to start building the rough outline of what the final board will look like. Cleaned up some code too as the game component was getting pretty bloated and there were some redundancies starting to creep in. I still need to add some more services, mainly one to handle the board, one to handle turns and one to fake the opponents turn. Once that's done I can add a new service to create the logs for the duel and (potentially separate) a notification service to display things like phase change etc. Managed to get some more experience using delegates so that's always useful.


## 27-12-2021 11:57AM (about 24 hours)

So just about hit a days worth of work, last few hours weren't too productive as I added toast notifications but they were really annoying and there needs to be separate types of displays for different situations (e.g switching phase should cause a small delay but a message stating a monster has been destroyed shouldn't, and they should appear differently too). Looking forward to getting this boxed off so I can look at touching up the graphics a bit. Also considering reducing the board size to 3 instead of 5 which would help with both keeping track of everything and fitting the entire game onto one screen (which it's not far off at the moment).


## 26-12-2021 11:21AM (about 21 hours)

Pretty good progress today, managed to add the logic for attacking (which involves having to deal with monsters before being able to attack directly, and added different rules for how the "battles" go when the cards are in different positions. Also added neccessary restrictions, dealt with some bugs that had creeped in and did some refactoring to break things into components, organised the code which on second glance looks fine, could do with having services to manage some of the functionality but not neccessary at present. Also added a mock AI for now which plays a random card from their hand and switches the positions of any monsters they have on the field each turn. Turned out to be a lot easier than I thought so hopefully that extends onto the actual AI build. Going to polish up the look of the app a little bit before I continue, but should be moving on to Spell cards after that. Managed to keep on top of my TODO pretty nicely so pretty pleased with overall progress :D.


## 25-12-2021 06:26PM (17ish hours)

Merry Christmas! Had some time to spare after a lovely day with the partner, so focused on switching the positions of cards. In this game, cards can be played face up/down and also horizontally/vertically which also is restricted based on the type of card. Managed to add the functionality but introduced a new bug that stops more than one monster card being played (always nice) so that'll be my next focus. Learned a lot more about how the equality comparer works (value equality vs reference equality) and so removed a redundant interface I implemented for the player class. The code is starting to get a little hard to follow (being tired doesn't help) but I'm going to crack on for now before I look at a refactor, took me longer than expected to get this feature added and I'm going to have to add spell and trap cards before I've got all the pieces I need to be able to look at the full game properly. Once I've fixed any bugs that are still hanging around I also need to look at the more advanced logic for attacking then I can see where to go from there. It's possible I'll stick with C# long term for the project, as I was unaware Unity allowed you to use that as well as C++. We'll see what happens though, in the meantime I think an early night is in order...


## 24-12-2021 03:54PM (roughly 12 hours of progress)

I was curious how much work it would take to build my own card game so this is it! First attempting an MVP with a familiar language before rebuilding in C++ so that I can focus more on getting stuck in rather than dealing with memory allocation and other headaches I don't want to deal with quite yet. I think this will be a good learning experience, both because I've already found it really fun to plan out the rules for my own game (shamelessly stolen from other similar card games for now as this is just a learner project but I am planning on adding special rules as and when I find a good original one and who knows, maybe this will join the many shelves of Steam indie games gathering dust one day) and because, despite it being just a card game, there's a good few complex subjects I should be able to cover. Initial plan is to get a simple one-sided game going with the various rules I've picked for an MVP (mainly limits on how many cards can be played per turn, how the game ends, types of cards etc), then look at implementing an AI (in C# for now) to take over the opponents role. Hoping I can remember to keep up with this blog too but I suppose I'll add a reminder on my TODO...
