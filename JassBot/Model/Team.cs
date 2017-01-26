namespace JassBot.Model
{
    public class Team
    {
        private string Name { get; set; }
        private int Points { get; set; }
        private int CurrentRoundPoints { get; set; }

        public override string ToString()
        {
            return "Team{" +
                "name='" + Name + '\'' +
                ", points=" + Points +
                ", currentRoundPoints=" + CurrentRoundPoints +
                '}';
        }
    }
}
