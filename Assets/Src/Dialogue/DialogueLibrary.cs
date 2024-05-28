using System.Collections.Generic;

public class DialogueLibrary {
    public static (string, string)[] GetDialogue (int dialogueContext) {
        (string, string)[] dialogueKey = null;
        Chap1.TryGetValue(dialogueContext, out dialogueKey);
        
        return dialogueKey;
    }

    public enum Char {Sai, Pik, Hels, Iska, Pom};
    public static Dictionary<int, (string character, string message)[]> Chap1 = new() {{
        1, //meeting with sai
        new(string, string)[] {
            ("Pik", "Hey."),
            ("Pik", "You needed me for something?"),

            ("Sai", "I need you to go gather a couple of rocks for me."),
            ("Sai", "I may have figured out the neccessary materials for a much safer base"),

            ("Pik", "Wait- Really?"),
            ("Pik", "That's great!"),

            ("Sai", "I visited a cave some time ago. Noticed that the Shades were..."),
            ("Sai", "guarding something. A pile of rocks?"),
            
            ("Pik", "... There must be some sort of reason why they're doing that..."),
            ("Pik", "Sure. I'll do it"),

            ("Sai", "Don't worry. You'll be rewarded handsomely for your efforts"),
            ("Pik", "Well, that just sweetens the deala lot more!"),
            
            ("Pik", "Right. I'll contact Hels. We'll start as soon as we can."),
            ("Sai", "Great"),

            ("Sai", "Good. The cave I went to was in China. Minerals are abundant there."),
            ("Sai", "... Be careful."),
            ("Sai", "Those monsters will surely be roaming around")
        }
    }};
}
