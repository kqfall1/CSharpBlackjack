I have reached a point where I am struggling to find ways to improve my codebase. I have created many more files to prevent any of my classes from being bloated and to maintain a clean separation of concerns. 
I believe I have done this cleverly with static Manager classes that hold logic for the game's showdowns, insurance betting, message output, and input validation. I did not even think to use inheritance and 
have the managers be concrete classes because they share few (if any) similarities and I don't see any point in instantiating them. I have created two concrete Controller classes that are responsible for 
the orchestration of the game. I believe I have abstracted cleverly and have designed a clean blackbox library. Furthermore, I have tested the engine quite thoroughly and have ironed out most of the bugs
in most edge cases. I documented some of my testing in versions 1.7 and 1.8. Before I push V1.91, I will test for an hour or two more with a CLI application to catch and resolve edge case bugs. I will also 
read through all the files in my codebase once more to put final touches on them, though I already did so last night and may not change much. 

I thought I would be at this point long before September 2nd. Looking back on my crude Python app, the fact that I was able to write a blackjack CLI game that mostly worked (albeit with many bugs) in one
file that had not even 700 lines of code goes to show why Python is such a popular programming language nowadays. You can undoubtedly accomplish tasks much quicker in Python than in strongly typed languages 
such as C# and Java (I have a bit of experience in Java as well, but that was years ago in high school. I have a Java class this upcoming college semester though!). However, the speed and ease of use in Python 
comes at the cost of semantic clarity and many more runtime errors (which can be particularly vexing -- ask me how I know). I have learned now that Python is a powerful tool for prototyping and writing 
quick-and-dirty code, but is generally not as reliable as C# or Java. 

I think I will expand and improve my backend at some point in the future by adding the ability for users to set the house rules for the game they are about to play. That sounds quite difficult and I wish to 
do some work on the other side of the stack after working on my codebase these past weeks, so I will put it off as a future idea for personal projects. I may also add XML comments, but my semantics-first style of 
coding should suffice for most members of my classes.

On an unrelated note: before I die, I wish to create and design a working game of chess (with a clean GUI and the works) by myself without using any libraries or modern abstractions that make development too easy. I understand this is a very difficult task that most 
exeperienced game developers would have some trouble with (because of the complexity of the gameplay of chess and the sheer amount of edge cases that alter the gameplay). I'll die trying to do it though. I have 
always been drawn to strategic games such as chess and poker and have had genuine fun in coding blackjack (which is more complicated than meets the eye but is baby food in complexity to a game like chess).  

I hope to start the GUI today or tomorrow. I am planning on building a WPF app rather than  a Windows Forms app. I built a Windows Forms app for my final project in last semester's C# OOP class and learned 
that the logic behind them gets complex and that the event handlers add up very quickly. I am not sure if WPF apps are any different in those regards, but I will find out soon enough! I hope to be mostly done in 
the next few weeks. Lord knows that these things never go as planned though. 

- Quinn Keenan
- 02/09/2025, 6:41PM