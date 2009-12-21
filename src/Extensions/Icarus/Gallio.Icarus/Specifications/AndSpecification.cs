﻿namespace Gallio.Icarus.Specifications
{
    public class AndSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> left;
        private readonly ISpecification<T> right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public bool Matches(T item)
        {
            return left.Matches(item) && right.Matches(item);
        }
    }
}