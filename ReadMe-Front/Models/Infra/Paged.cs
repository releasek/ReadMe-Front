using System.Collections.Generic;

namespace ReadMe_Front.Models.Infra
{
    public class Paged<T>
    {
        public Paged(IEnumerable<T> data, Pagination pagination)
        {
            Data = data;
            Pagination = pagination;
        }
        public IEnumerable<T> Data { get; }
        public Pagination Pagination { get; }
    }
}