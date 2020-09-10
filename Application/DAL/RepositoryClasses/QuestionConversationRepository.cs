namespace DAL
{
    public class QuestionConversationRepository : Repository<Models.QuestionConversation>, IQuestionConversationRepository
    {
        public QuestionConversationRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
