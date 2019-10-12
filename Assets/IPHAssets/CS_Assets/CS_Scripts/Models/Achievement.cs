using System;

[Serializable]
public class Achievement
{
    public int id;
    public string name;
    public string objective;
    public int currentGoal;
    public int finalGoal;
    public float progress;
    //public Rules rules;

    //Constructors
    public Achievement()
    {
        this.name = "";
        this.objective = "";
        this.progress = 0;
        this.currentGoal = 0;
        this.finalGoal = 10;
        this.progress = currentGoal / finalGoal;
    }

    public Achievement(string name, string objective, int currentGoal, int finalGoal, float progress = 0)
    {
        this.name = name;
        this.objective = objective;
        this.progress = progress;
        this.currentGoal = currentGoal;
        this.finalGoal = finalGoal;
        this.progress = currentGoal / finalGoal;
    }

    /*[Serializable]
    public class Rules
    {
        public string rule;
    }*/
}