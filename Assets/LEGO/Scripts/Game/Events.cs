namespace Unity.LEGO.Game
{
    // The Game Events used across the Game.
    // Anytime there is a need for a new event, it should be added here.

    public static class Events
    {
        public static GameStartEvent GameStartEvent = new GameStartEvent();
        public static OptionsMenuEvent OptionsMenuEvent = new OptionsMenuEvent();
        public static ObjectiveAdded ObjectiveAddedEvent = new ObjectiveAdded();
        public static VariableAdded VariableAddedEvent = new VariableAdded();
        public static GameOverEvent GameOverEvent = new GameOverEvent();
        public static LookSensitivityUpdateEvent LookSensitivityUpdateEvent = new LookSensitivityUpdateEvent();
    }

    public class GameStartEvent: GameEvent
    {

    }

    // UI Events.
    public class OptionsMenuEvent: GameEvent
    {
        public bool Active;
    }

    // LEGOBehaviour Events.
    public class ObjectiveAdded : GameEvent
    {
        public IObjective Objective;
    }

    public class VariableAdded : GameEvent
    {
        public Variable Variable;
    }

    public class TimeStopEvent : GameEvent
    {
        public bool Stopped;
        public float LapTime;

        public TimeStopEvent() { }

        public TimeStopEvent(float laptime)
        {
            LapTime = laptime;
        }
    }

    public class GameOverEvent : GameEvent
    {
        public bool Win;
    }

    public class LookSensitivityUpdateEvent : GameEvent
    {
        public float Value;
    }
}
