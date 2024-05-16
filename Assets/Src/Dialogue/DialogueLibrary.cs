using System;
using System.Collections.Generic;

public class DialogueLibrary {
    public static (string, string)[] GetDialogue (int chapter, int dialogueContext) {
        (string, string)[] dialogueKey = null;

        switch (chapter) {
            case 1:
                if(Chap1.TryGetValue(dialogueContext, out dialogueKey));
                break;
        }
        return dialogueKey;
    }

    public enum Char {Sai, Pik, Hels, Iska, Pom};
    public static Dictionary<int, (string character, string message)[]> Chap1 = new() {{
        1, //meeting with sai
        new(string, string)[] {
            ("Pik", "Hey."),
            ("Pik", "You needed me for something?"),

            ("Sai", "I need you to go gather a couple of rocks for me."),
            ("Pik", "Sure. What are they? I might have some left behind from-"),

            ("Sai", "No idea."),
            ("Pik", "Huh..?"),

            ("Sai", "I found some sort of writings in a cave. They described a group of rocks... for something. I managed to decipher it, but only its location."),
            ("Pik", "..."),
            ("Pik", "And I have to figure out where it is. Hey my job is to mine rocks not answer some riddles or somethi-"),

            ("Sai", "I'll pay you."),
            ("Pik", "Deal.")
        }
    }};
}
