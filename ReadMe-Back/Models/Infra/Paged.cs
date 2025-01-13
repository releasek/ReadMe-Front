namespace ReadMe_Back.Models.Infra
{
    public class Paged<T>
    {
        public Paged(IEnumerable<T> data, Pagination pagination, string selected, IEnumerable<string> options)
        {
            Data = data;
            Pagination = pagination;
            Selected = selected;
            Options = options;
        }
        public IEnumerable<T> Data { get; }
        public Pagination Pagination { get; }

        public string Selected { get; }

        public IEnumerable<string> Options { get; }
    }
}
