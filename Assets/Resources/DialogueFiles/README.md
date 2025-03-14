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

### Using In-line Arguments
The dialogue system has a custom-built in-line arguments system to allow for the change of certain settings during dialogue, such as character sprites and text speed. An in-line argument is enclosed with square brackets "[ ]" and follow a very specific format, which starts with what you want to change, e.g. "character", followed by equals "=", followed by the value you want to change that setting to, e.g. "Quinn"; for example:

> [character=Quinn]
> [textspeed=Normal]

You can also chain in-line arguments, which is ideal for cases such as if you want a specific emotion on a specific character; for example:

> [character=Quinn][sprite=Happy]Hello, traveller!

Also note that once you have declared a setting, it does not need to be redeclared unless you want to change it to something else. Please see "Further Notes" for how to change the settings of in-line arguments as well as things like character expressions.

### Best Practice for In-line Arguments
The best way to use the dialogue system is to make sure all parameters are set to what you want them to be—remember that they aren't set until you set them. For example, if you want a neutral Quinn for your dialogue line, make sure that you declare that at least on your first line, otherwise it may still be whatever was on the previous line of dialogue in the previous dialogue file.

### Attaching Your Dialogue File to the Game
Currently, attaching dialogue files to Cleanup Krue is only possible in the test scene while a proper interaction system has not yet been programmed. To test your dialogue file, find the "NPC" GameObject and attach your .txt file to the "On Interact()" event (make sure to click the + icon to add a new list entry if there isn't already one).

### Further Notes
- Settings for dialogue system components, e.g. in-line arguments or character expressions, can be changed on the DialogueManager object, which can usually be found under the Systems GameObject in the scene.
- Be advised that in-line arguments are a custom feature designed by Mitchel Smith. If an issue is encountered with using in-line arguments or you would like to request an extra feature, please speak to Mitchel.

## In-Line Argument Syntax
### Changing Characters
**Declaration:** "character"\
**Values:**
- "Quinn"
- "Caspian"
- "Kingg"
- "Dewdrop"\
**Default:** "Dewdrop"

### Changing Expressions
**Declaration:** "sprite"\
**Values:**
- "Neutral"
- "Happy"
- "Surprised"
- "Scared"
- "Unhappy"
- "Confused"\
**Default:** Neutral

### Changing Text Speed
**Declaration:** "textspeed"\
**Values:**
- "Slow"
- "Normal"
- "Fast"
- "Very fast"\
**Default:** Normal