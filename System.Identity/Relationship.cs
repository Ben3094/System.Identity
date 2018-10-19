using System;
using System.Collections.Generic;
using System.Text;

namespace System.Identity
{
    public abstract class Relationship
    {
        public enum RelationshipType { Contact, Acquaintance, Friend, Met, CoWorker, Colleague, CoResident, Neighbor, Child, Parent, Sibling, Spouse, Kin, Muse, Crush, Date, Sweetheart, Me, Agent, Emergency }

        public Relationship(RelationshipType type)
        {
            this.Type = type;
        }

        public RelationshipType Type;
    }

    public class UriRelationship : Relationship
    {
        public UriRelationship(RelationshipType type) : base(type) { }
    }

    public class TextRelationship : Relationship
    {
        public TextRelationship(RelationshipType type) : base(type) { }
    }
}
