# Cleanup Krue Dialogue System
*By Mitchel Smith*

## Usage Guide
(For just the syntax of in-line arguments, skip to "In-Line Argument Syntax")

### Introduction
This is the dialogue system to be used for NPC interaction in Cleanup Krue. The purpose of this README file is to explain how to structure your .txt files to insert into the dialogue system.

### Creating Your Dialogue Lines
The baseline functionality of this dialogue system is to take lines of dialogue that you've written in your .txt file and print it out line-by-line in the game. All you have to do is enter onto a new line to finish a line of dialogue, no special markings needed! Ideally, you want dialogue lines to be relatively short as to not overflow the text box in-game—remember that you can always split up your lines and don't have to finish them with full stops. 

### Where to Save Dialogue Files
Unity has a Resources folder feature that allows systems to read from standard files within the game's folder. Inside Unity's project folder, you should save your dialogue files in the "DialogueFiles" folder within the Resources folder. For example:

> ..\Cleanup-Krue\Assets\Resources\DialogueLines\Quinn\QuinnShopIntro.txt

### Formatting Text: Bold, Italics & Underlines
The dialogue system supports rich text functionality, which supports formatting specific parts of a text. The three formatting options include bolding, italicising and underlining. To do this, you must start with an opening format tag (in HTML style), e.g. "<b>" for bold, write your text and then close it with a closing format tag, e.g. "</b>". An example of the three formatting tags:

> <b>Good evening, Sir Longbottom!</b>
> <i>Ah, Mr. Goldsworth! Good to see you, my fellow chap!</i>
> <u>Same for you, my friend. Shall I fetch you a tea?</u>

### Declaring Characters for Initiation & Change
You can change whichever character is speaking on a new line by using an in-line argument. In-line arguments are enclosed with square brackets ("[ ]"). To change a character, you must specifically type "character=" followed by the character you want to change to, e.g. Quinn or Caspian. You can then put your line of dialogue immediately after this argument, <b>without a space preceeding it.</b> For example:

> [character=Quinn]Welcome to Watching Paint Dry: The Game
> [character=Caspian]In this game, you sit and watch paint dry.

Note that you do not have to redeclare a character that was already present on the last line of dialogue as it will automatically carry over into the next. In order to change the colour of the text box, find the "Dialogue Utils" component under the "DialogueManager" GameObject.

### Declaring Expressions for Initiation & Change
This feature allows you to change the expression of a character during a line of dialogue. Just like declaring characters, you must declare expressions with in-line arguments. These should come after declaring a character or on a new line where a character is not declared. To change expression, you must type "sprite=" followed by the emotion you want to change to, <b>without a space preceeding it.</b> For example:

> [character=Quinn][sprite=Happy]Oh hi there, how are ya?
> [sprite=Neutral]Aw, having a bad day? That's too bad.

Please note that expressions must be added to a character's list of sprites, which can be found on the "Dialogue Utils" component on the "DialogueManager" GameObject. To add a new expression, expand a character's sprite list, click the + button and drag the sprite you want to implement from the project folder into the field. For expressions not yet added to the dialogue system, please speak to Mitchel.

### Best Practice for In-line Arguments
The best way to use the dialogue system is to make sure all parameters are set to what you want them to be—remember that they aren't set until you set them. For example, if you want a neutral Quinn for your dialogue line, make sure that you declare that at least on your first line, otherwise it may still be whatever was on the previous line of dialogue in the previous dialogue file. For example:

> [character=Quinn][sprite=Neutral]Hi there, what can I get for ya?

### Attaching Your Dialogue File to the Game
Currently, attaching dialogue files to Cleanup Krue is only possible in the test scene while a proper interaction system has not yet been programmed. To test your dialogue file, find the "NPC" GameObject and attach your .txt file to the "On Interact()" event (make sure to click the + icon to add a new list entry if there isn't already one).

### Further Notes
- Be advised that in-line arguments are a custom feature designed by Mitchel Smith. If an issue is encountered with using in-line arguments or you would like to request an extra feature, please speak to Mitchel.

## In-Line Argument Syntax
### Changing Characters
**Declaration:** "character"\
**Values:**
- "Quinn"
- "Caspian"
- "Kingg"
- "Dewdrop"

### Changing Expressions
**Declaration:** "sprite"\
**Values:**
- "Neutral"
- "Happy"
- "Surprised"
- "Scared"
- "Unhappy"
- "Confused"

### Changing Text Speed
**Declaration:** "textspeed"\
**Values:**
- "Slow"
- "Normal"
- "Fast"
- "Very fast"