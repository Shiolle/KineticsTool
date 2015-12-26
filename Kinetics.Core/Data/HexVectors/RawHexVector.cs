using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Kinetics.Core.Data.HexVectors
{
    /// <summary>
    /// This class represents a hex vector without consolidation. There may be any number of components, including multiple
    /// components in the same direction.
    /// </summary>
    public class RawHexVector : IEquatable<RawHexVector>
    {
        protected readonly List<HexVectorComponent> _components;

        public RawHexVector()
        {
            _components = new List<HexVectorComponent>();
        }

        public RawHexVector(IEnumerable<HexVectorComponent> components)
            :this()
        {
            _components.AddRange(components);
            OnComponentsChanged();
        }

        public ReadOnlyCollection<HexVectorComponent> Components
        {
            get { return _components.AsReadOnly(); }
        }

        public static RawHexVector Zero
        {
            get
            {
                return new RawHexVector();
            }
        }

        #region Adding and removing components

        public void AddComponent(HexVectorComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            _components.Add(component);
            OnComponentsChanged();
        }

        public void AddComponent(HexAxis direction, int magnitude)
        {
            AddComponent(new HexVectorComponent(direction, magnitude));
        }

        public void AddComponents(IEnumerable<HexVectorComponent> components)
        {
            if (components == null)
            {
                throw new ArgumentNullException("components");
            }

            _components.AddRange(components);
            OnComponentsChanged();
        }

        public void RemoveComponent(HexVectorComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            _components.Remove(component);
            OnComponentsChanged();
        }

        public void RemoveComponents(IEnumerable<HexVectorComponent> components)
        {
            if (components == null)
            {
                throw new ArgumentNullException("components");
            }

            foreach (var component in components)
            {
                _components.Remove(component);
            }
        }

        public void ClearComponents()
        {
            _components.Clear();
            OnComponentsChanged();
        }

        #endregion

        #region Equality logic

        public bool Equals(RawHexVector other)
        {
            if (other == null)
            {
                // Null vecotre is equal to zero vector.
                return Components.Count(cn => cn.Magnitude != 0) == 0;
            }

            foreach (var component in Components)
            {
                if (!other.Components.Contains(component))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int result = 0;

            foreach (var component in Components)
            {
                result += component.GetHashCode();
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var other = obj as RawHexVector;

            if (other != null)
            {
                return this.Equals(other);
            }

            return false;
        }

        #endregion

        public override string ToString()
        {
            var renderedComponents = Components.Where(cn => cn.Magnitude != 0).Select(cn => cn.ToString()).ToArray();

            if (renderedComponents.Length == 0)
            {
                return "Zero";
            }
            return string.Join(string.Empty, renderedComponents);
        }

        protected virtual void OnComponentsChanged() { }
    }
}
