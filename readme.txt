Four years ago, I created a simple console blackjack game in Python. I was very proud of it and thought of it as my greatest programmatic creation for years. In hindsight, the application was under-engineered and crude.
It also had some tendencies similar to those of spaghetti code. Furthermore, it has a few minor - but very noticeable - bugs.  In this project, I sought to demonstrate my growth as a developer after two semesters in 
college by creating a blackbox blackjack engine in C#. I have tested it quite thoroughly with a crude CLI application and have reached a point of stability (in that the engine and CLI application are running smoothly 
under pressure with few to no bugs) after many long hours. I intend to create a GUI application to utilize the library in the near future. 

Lots of my struggles in this project so far have arisen out of methods that try to behave dynamically depending on the game's state and methods that have side effects. This lead to confusion when I was debugging. I am
now starting to understand that the concept of the separation of concerns is so important, not only for classes and class behaviors, but for entire libraries and applications that combine to form complex systems. Even
source code files in libraries should be grouped together in a reasonable way via folders. Furthermore, this project would have been easier if I had decided to make a whitebox library. This would make more sense, 
considering the fact that blackjack has been programmed countless times before, reputable online blackjack engines already exist and are used by casinos (making my project worthless to any potential customer of mine), 
and I have no competitors watching me trying to reverse-engineer my intermediate-level C# project. Never hurts to challenge oneself though, right?  

Currently, my codebase works but needs to be cleaned up and refactored before I commence the GUI application. I learned from my last college C# project that building the frontend and the backend at the same time is not
a good idea because it complicates debugging and wastes time. It would not be unwise of me to begin the GUI application now because I am 95% sure my library suffers from no bugs, but I might as well fix it up before
moving forward. Here is my refactoring plan: 

- Add Dealer.DrawCard() that calls Deck.DrawCard() to return a Card (so as to mimic real-life Blackjack in accordance with the purpose of OOP). I also removed Game.Deck and instead had Deck encapsulated by Dealer.
- Remove any unecessary constants from HandStatus (I will definitley be removing HandStatus.Busted, for the computed property Hand.IsBusted works better). 
- Implement surrendering, an option offered in some blackjack games where the player can "fold" their hand and get paid half of their bet. This will be useful for when I get dealt 15s and 16s!
- Turn Player.InsuranceBetWon into a computed property that is inferred by the game's state. 
- Remove or alter Game.GetEntityHasBusted and Game.EntityHasBlackjack.
- Add more specific exceptions to the necessary try-catch blocks currently in PlayerController and clean up all of the try-catch blocks. 
- The Game and PlayerController class are bloated and are doing too much work. I will have to plan how to divide the PlayerController logic into multiple classes, because it is doing much more than driving the player's
 interaction with the game. I originally intended it for it to do just that, but I got carried away, no doubt. 
 
- Look through all other classes and validate whether the members belong (should the Game class have Pots if Bet already encapsulates an immutable Pot??).
-Double check all lines of code in the library to verify that they are consistent with each other and proper conventions. 
- Rename any variables/methods/classes/etc. necessary. Alter all access modifiers to be as strict as reasonably possible. 
- I may add null checks to some of my library class members. However, I designed the library with the idea that any input must enter through an interface class inside the library that connects the application to the 
library (currently BlackjackEntitiesController) and that all input will be validated there. 
- Change any readonly properties into readonly fields. My general philosophy is to make any piece of instance data that is supposed to be immutable into a field and any piece of data that is supposed to be mutable into a 
property. Many developers at Microsoft may take issue with that, but I am sure that I am not alone in following this convention. I am slightly sacrificing the ease of serialization and the scalability of my project 
for semantic clarity and readability. I believe that is reasonable considering that I don't plan on ever implementing serialization in any way for this project and have compensated for the scalability loss by my 
other design choices. 
- Group all library csharp source into appropriate folders. 

I am excited to demonstrate my ability to maintain a codebase of my own creation! I am looking forward to creating the GUI application as well! I may be reinventing the wheel in a way, but I have a feeling that my efforts
will be noted, even with such a "simple" project: a single-deck, single-player game of blackjack. 

- Quinn Keenan 
- 29/08/2025 12:38 AM