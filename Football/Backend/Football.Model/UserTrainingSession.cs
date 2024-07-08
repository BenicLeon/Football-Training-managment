namespace Football.Model
{
    public class UserTrainingSession
    {
        public Guid TrainingSessionId { get; set; }
        public TrainingSession TrainingSession { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
