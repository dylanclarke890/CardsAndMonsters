using System;

namespace CardsAndMonsters.Models.Base
{
    public abstract class BaseModel : IEquatable<BaseModel>
    {
        public BaseModel()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BaseModel model &&
                   Id.Equals(model.Id);
        }

        public bool Equals(BaseModel other)
        {
            return other is BaseModel model &&
                   Id.Equals(model.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public bool IsType(Type type)
        {
            return GetType() == type;
        }
    }
}
