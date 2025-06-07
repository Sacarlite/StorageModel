namespace Warehouse.Domain.Models
{
    public class ExpirationDateResult
    {
        public bool HasValue { get; }
        public DateTime? Date { get; }

        private ExpirationDateResult(bool hasValue, DateTime? date)
        {
            HasValue = hasValue;
            Date = date;
        }

        public static ExpirationDateResult Empty => new(false, null);
        public static ExpirationDateResult FromDate(DateTime date) => new(true, date.Date);

        public override string ToString() => HasValue ? Date!.Value.ToString("yyyy-MM-dd") : "–";
    }
}
