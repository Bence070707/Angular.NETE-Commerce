using Core.Entities;
using Core.Interfaces;

namespace InfraStructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        if (spec.Criteria is not null)
        {
            query = query.Where(spec.Criteria); // x => x.Brand == brand
        }

        if (spec.OrderBy is not null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if (spec.OrderDescending is not null)
        {
            query = query.OrderByDescending(spec.OrderDescending);
        }

        if (spec.IsDistinct)
        {
            query = query.Distinct();
        }
        return query;
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query, ISpecification<T, TResult> spec)
    {
        if (spec.Criteria is not null)
        {
            query = query.Where(spec.Criteria); // x => x.Brand == brand
        }

        if (spec.OrderBy is not null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if (spec.OrderDescending is not null)
        {
            query = query.OrderByDescending(spec.OrderDescending);
        }

        var selectQuery = query as IQueryable<TResult>;
        if (spec.Select is not null)
        {
            selectQuery = query.Select(spec.Select);
        }

        if (spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }
        return selectQuery ?? query.Cast<TResult>();
    }
}
