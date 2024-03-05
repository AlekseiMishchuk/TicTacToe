using Enums;

namespace Interfaces
{
    public interface IBootstrappable
    {
        public BootPriority BootPriority { get;}
        void ManualStart();
    }
}