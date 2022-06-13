namespace QuizManager.Models.Helpers
{
    public static class AnswerIndexerHelper
    {
        public static string[] AnswerIndexes = { "A", "B", "C", "D", "E" };

        public static int GetIndex(string answerIndex)
        {
            for (int i = 0; i < AnswerIndexes.Length; i++)
                if (AnswerIndexes[i] == answerIndex)
                    return i;

            return 0;
        }
    }
}