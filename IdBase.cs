using System;

namespace Sample
{
    public abstract class IdBase<T> : IEquatable<IdBase<T>>
    {
        protected IdBase()
        {
        }

        protected IdBase(T value)
        {
            Value = value;
        }

        public virtual T Value { get; protected set; }

        public virtual bool Equals(IdBase<T> other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IdBase<T>);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator T(IdBase<T> id)
        {
            return id.Value;
        }
    }
}