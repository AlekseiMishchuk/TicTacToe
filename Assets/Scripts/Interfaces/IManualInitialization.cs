using Enums;

namespace Interfaces
{
    public interface IManualInitialization
    {
        public BootPriority BootPriority { get;}
        void ManualInit();
    }
}