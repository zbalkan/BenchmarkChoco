namespace BenchmarkChoco
{
    public class ListGenerationProgress
    {
        internal ListGenerationProgress(int currentCount, int totalCount, string message)
        {
            TotalCount = totalCount;
            Message = message;
            CurrentCount = currentCount;
        }

        public int CurrentCount { get; internal set; }

        /// <summary>
        /// -1 if unknown
        /// </summary>
        public int TotalCount { get; internal set; }

        public string Message { get; internal set; }

        //public GetUninstallerListProgress Clone() => (GetUninstallerListProgress)MemberwiseClone();

        public ListGenerationProgress Inner { get; internal set; }

        public delegate void ListGenerationCallback(ListGenerationProgress progressReport);
    }
}
